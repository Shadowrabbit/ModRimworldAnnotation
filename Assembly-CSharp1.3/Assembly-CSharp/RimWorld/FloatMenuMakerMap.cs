using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200132E RID: 4910
	public static class FloatMenuMakerMap
	{
		// Token: 0x060076C1 RID: 30401 RVA: 0x00293EC1 File Offset: 0x002920C1
		private static bool CanTakeOrder(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}

		// Token: 0x060076C2 RID: 30402 RVA: 0x00293ECC File Offset: 0x002920CC
		public static void TryMakeFloatMenu(Pawn pawn)
		{
			if (!FloatMenuMakerMap.CanTakeOrder(pawn))
			{
				return;
			}
			if (pawn.Downed)
			{
				Messages.Message("IsIncapped".Translate(pawn.LabelCap, pawn), pawn, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (pawn.Map != Find.CurrentMap)
			{
				return;
			}
			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual;
			if (lord != null && (lordJob_Ritual = (lord.LordJob as LordJob_Ritual)) != null)
			{
				Messages.Message("ParticipatingInRitual".Translate(pawn, lordJob_Ritual.RitualLabel), pawn, MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<FloatMenuOption> list = FloatMenuMakerMap.ChoicesAtFor(UI.MouseMapPosition(), pawn, false);
			if (list.Count == 0)
			{
				return;
			}
			bool flag = true;
			FloatMenuOption floatMenuOption = null;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Disabled || !list[i].autoTakeable)
				{
					flag = false;
					break;
				}
				if (floatMenuOption == null || list[i].autoTakeablePriority > floatMenuOption.autoTakeablePriority)
				{
					floatMenuOption = list[i];
				}
			}
			if (flag && floatMenuOption != null)
			{
				floatMenuOption.Chosen(true, null);
				return;
			}
			FloatMenuMap floatMenuMap = new FloatMenuMap(list, pawn.LabelCap, UI.MouseMapPosition());
			floatMenuMap.givesColonistOrders = true;
			Find.WindowStack.Add(floatMenuMap);
		}

		// Token: 0x060076C3 RID: 30403 RVA: 0x00294024 File Offset: 0x00292224
		public static bool TryMakeMultiSelectFloatMenu(List<Pawn> pawns)
		{
			FloatMenuMakerMap.tmpPawns.Clear();
			FloatMenuMakerMap.tmpPawns.AddRange(pawns);
			FloatMenuMakerMap.tmpPawns.RemoveAll(new Predicate<Pawn>(FloatMenuMakerMap.InvalidPawnForMultiSelectOption));
			if (!FloatMenuMakerMap.tmpPawns.Any<Pawn>())
			{
				return false;
			}
			List<FloatMenuOption> list = FloatMenuMakerMap.ChoicesAtForMultiSelect(UI.MouseMapPosition(), FloatMenuMakerMap.tmpPawns);
			if (!list.Any<FloatMenuOption>())
			{
				FloatMenuMakerMap.tmpPawns.Clear();
				return false;
			}
			FloatMenu window = new FloatMenu(list)
			{
				givesColonistOrders = true
			};
			Find.WindowStack.Add(window);
			FloatMenuMakerMap.tmpPawns.Clear();
			return true;
		}

		// Token: 0x060076C4 RID: 30404 RVA: 0x002940B4 File Offset: 0x002922B4
		public static bool InvalidPawnForMultiSelectOption(Pawn x)
		{
			Lord lord = x.GetLord();
			return !FloatMenuMakerMap.CanTakeOrder(x) || x.Downed || x.Map != Find.CurrentMap || (lord != null && lord.LordJob is LordJob_Ritual);
		}

		// Token: 0x060076C5 RID: 30405 RVA: 0x002940FC File Offset: 0x002922FC
		public static List<FloatMenuOption> ChoicesAtFor(Vector3 clickPos, Pawn pawn, bool suppressAutoTakeableGoto = false)
		{
			IntVec3 intVec = IntVec3.FromVector3(clickPos);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			Lord lord = pawn.GetLord();
			if (!intVec.InBounds(pawn.Map) || !FloatMenuMakerMap.CanTakeOrder(pawn) || (lord != null && lord.LordJob is LordJob_Ritual))
			{
				return list;
			}
			if (pawn.Map != Find.CurrentMap)
			{
				return list;
			}
			FloatMenuMakerMap.makingFor = pawn;
			try
			{
				if (intVec.Fogged(pawn.Map))
				{
					if (pawn.Drafted)
					{
						FloatMenuOption floatMenuOption = FloatMenuMakerMap.GotoLocationOption(intVec, pawn, suppressAutoTakeableGoto);
						if (floatMenuOption != null && !floatMenuOption.Disabled)
						{
							list.Add(floatMenuOption);
						}
					}
				}
				else
				{
					if (pawn.Drafted)
					{
						FloatMenuMakerMap.AddDraftedOrders(clickPos, pawn, list, suppressAutoTakeableGoto);
					}
					if (pawn.RaceProps.Humanlike)
					{
						FloatMenuMakerMap.AddHumanlikeOrders(clickPos, pawn, list);
					}
					if (!pawn.Drafted)
					{
						FloatMenuMakerMap.AddUndraftedOrders(clickPos, pawn, list);
					}
					foreach (FloatMenuOption item in pawn.GetExtraFloatMenuOptionsFor(intVec))
					{
						list.Add(item);
					}
				}
			}
			finally
			{
				FloatMenuMakerMap.makingFor = null;
			}
			return list;
		}

		// Token: 0x060076C6 RID: 30406 RVA: 0x00294224 File Offset: 0x00292424
		public static List<FloatMenuOption> ChoicesAtForMultiSelect(Vector3 clickPos, List<Pawn> pawns)
		{
			IntVec3 c = IntVec3.FromVector3(clickPos);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			Map map = pawns[0].Map;
			if (!c.InBounds(map) || map != Find.CurrentMap)
			{
				return list;
			}
			foreach (Thing thing in map.thingGrid.ThingsAt(c))
			{
				foreach (FloatMenuOption item in thing.GetMultiSelectFloatMenuOptions(pawns))
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x060076C7 RID: 30407 RVA: 0x002942E0 File Offset: 0x002924E0
		private static void AddDraftedOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts, bool suppressAutoTakeableGoto = false)
		{
			IntVec3 clickCell = IntVec3.FromVector3(clickPos);
			foreach (LocalTargetInfo attackTarg in GenUI.TargetsAt(clickPos, TargetingParameters.ForAttackHostile(), true, null))
			{
				FloatMenuMakerMap.<>c__DisplayClass8_1 CS$<>8__locals2 = new FloatMenuMakerMap.<>c__DisplayClass8_1();
				CS$<>8__locals2.attackTarg = attackTarg;
				if (pawn.equipment.Primary != null && !pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
				{
					string str;
					Action rangedAct = FloatMenuUtility.GetRangedAttackAction(pawn, CS$<>8__locals2.attackTarg, out str);
					string text = "FireAt".Translate(CS$<>8__locals2.attackTarg.Thing.Label, CS$<>8__locals2.attackTarg.Thing);
					FloatMenuOption floatMenuOption = new FloatMenuOption("", null, MenuOptionPriority.High, null, attackTarg.Thing, 0f, null, null, true, 0);
					if (rangedAct == null)
					{
						text = text + ": " + str;
					}
					else
					{
						floatMenuOption.autoTakeable = (!CS$<>8__locals2.attackTarg.HasThing || CS$<>8__locals2.attackTarg.Thing.HostileTo(Faction.OfPlayer));
						floatMenuOption.autoTakeablePriority = 40f;
						floatMenuOption.action = delegate()
						{
							FleckMaker.Static(CS$<>8__locals2.attackTarg.Thing.DrawPos, CS$<>8__locals2.attackTarg.Thing.Map, FleckDefOf.FeedbackShoot, 1f);
							rangedAct();
						};
					}
					floatMenuOption.Label = text;
					opts.Add(floatMenuOption);
				}
				string str2;
				Action meleeAct = FloatMenuUtility.GetMeleeAttackAction(pawn, CS$<>8__locals2.attackTarg, out str2);
				Pawn pawn2 = CS$<>8__locals2.attackTarg.Thing as Pawn;
				string text2;
				if (pawn2 != null && pawn2.Downed)
				{
					text2 = "MeleeAttackToDeath".Translate(CS$<>8__locals2.attackTarg.Thing.Label, CS$<>8__locals2.attackTarg.Thing);
				}
				else
				{
					text2 = "MeleeAttack".Translate(CS$<>8__locals2.attackTarg.Thing.Label, CS$<>8__locals2.attackTarg.Thing);
				}
				MenuOptionPriority priority = (CS$<>8__locals2.attackTarg.HasThing && pawn.HostileTo(CS$<>8__locals2.attackTarg.Thing)) ? MenuOptionPriority.AttackEnemy : MenuOptionPriority.VeryLow;
				FloatMenuOption floatMenuOption2 = new FloatMenuOption("", null, priority, null, CS$<>8__locals2.attackTarg.Thing, 0f, null, null, true, 0);
				if (meleeAct == null)
				{
					text2 = text2 + ": " + str2.CapitalizeFirst();
				}
				else
				{
					floatMenuOption2.autoTakeable = (!CS$<>8__locals2.attackTarg.HasThing || CS$<>8__locals2.attackTarg.Thing.HostileTo(Faction.OfPlayer));
					floatMenuOption2.autoTakeablePriority = 30f;
					floatMenuOption2.action = delegate()
					{
						FleckMaker.Static(CS$<>8__locals2.attackTarg.Thing.DrawPos, CS$<>8__locals2.attackTarg.Thing.Map, FleckDefOf.FeedbackMelee, 1f);
						meleeAct();
					};
				}
				floatMenuOption2.Label = text2;
				opts.Add(floatMenuOption2);
			}
			if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				foreach (LocalTargetInfo carryTarget2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForCarry(pawn), true, null))
				{
					LocalTargetInfo carryTarget = carryTarget2;
					FloatMenuOption item;
					if (!pawn.CanReach(carryTarget, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						item = new FloatMenuOption("CannotCarry".Translate(carryTarget.Thing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						item = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Carry".Translate(carryTarget.Thing), delegate()
						{
							carryTarget.Thing.SetForbidden(false, false);
							Job job = JobMaker.MakeJob(JobDefOf.CarryDownedPawnDrafted, carryTarget);
							job.count = 1;
							pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, carryTarget, "ReservedBy");
					}
					opts.Add(item);
				}
			}
			if (pawn.IsCarryingPawn(null))
			{
				Pawn carriedPawn = (Pawn)pawn.carryTracker.CarriedThing;
				if (!carriedPawn.IsPrisonerOfColony)
				{
					foreach (LocalTargetInfo destTarget3 in GenUI.TargetsAt(clickPos, TargetingParameters.ForDraftedCarryBed(carriedPawn, pawn, carriedPawn.GuestStatus), true, null))
					{
						LocalTargetInfo destTarget = destTarget3;
						FloatMenuOption item2;
						if (!pawn.CanReach(destTarget, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							item2 = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, destTarget.Thing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
						}
						else
						{
							item2 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(carriedPawn, destTarget.Thing), delegate()
							{
								destTarget.Thing.SetForbidden(false, false);
								Job job = JobMaker.MakeJob(JobDefOf.TakeDownedPawnToBedDrafted, pawn.carryTracker.CarriedThing, destTarget);
								job.count = 1;
								pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, destTarget, "ReservedBy");
						}
						opts.Add(item2);
					}
				}
				foreach (LocalTargetInfo destTarget2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForDraftedCarryBed(carriedPawn, pawn, new GuestStatus?(GuestStatus.Prisoner)), true, null))
				{
					LocalTargetInfo destTarget = destTarget2;
					FloatMenuOption item3;
					if (!pawn.CanReach(destTarget, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						item3 = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, destTarget.Thing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						TaggedString taggedString = "PlaceIn".Translate(carriedPawn, destTarget.Thing);
						if (!carriedPawn.IsPrisonerOfColony)
						{
							taggedString += ": " + "ArrestChance".Translate(carriedPawn.GetAcceptArrestChance(pawn).ToStringPercent());
						}
						item3 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
						{
							destTarget.Thing.SetForbidden(false, false);
							Job job = JobMaker.MakeJob(JobDefOf.CarryToPrisonerBedDrafted, pawn.carryTracker.CarriedThing, destTarget);
							job.count = 1;
							pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, destTarget, "ReservedBy");
					}
					opts.Add(item3);
				}
				foreach (LocalTargetInfo localTargetInfo in GenUI.TargetsAt(clickPos, TargetingParameters.ForDraftedCarryTransporter(carriedPawn), true, null))
				{
					Thing transporterThing = localTargetInfo.Thing;
					if (transporterThing != null)
					{
						CompTransporter compTransporter = transporterThing.TryGetComp<CompTransporter>();
						if (compTransporter.Shuttle == null || compTransporter.Shuttle.IsAllowedNow(carriedPawn))
						{
							if (!pawn.CanReach(transporterThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
							{
								opts.Add(new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, transporterThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							else if (compTransporter.Shuttle == null && !compTransporter.LeftToLoadContains(carriedPawn))
							{
								opts.Add(new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, transporterThing) + ": " + "NotPartOfLaunchGroup".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							else
							{
								string label = "PlaceIn".Translate(carriedPawn, transporterThing);
								Action action = delegate()
								{
									if (!compTransporter.LoadingInProgressOrReadyToLaunch)
									{
										TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(compTransporter));
									}
									Job job = JobMaker.MakeJob(JobDefOf.HaulToTransporter, carriedPawn, transporterThing);
									job.ignoreForbidden = true;
									job.count = 1;
									pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
								};
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, transporterThing, "ReservedBy"));
							}
						}
					}
				}
				foreach (LocalTargetInfo localTargetInfo2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForDraftedCarryCryptosleepCasket(pawn), true, null))
				{
					Thing casket = localTargetInfo2.Thing;
					TaggedString taggedString2 = "PlaceIn".Translate(carriedPawn, casket);
					if (((Building_CryptosleepCasket)casket).HasAnyContents)
					{
						opts.Add(new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketOccupied".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else if (carriedPawn.IsQuestLodger())
					{
						opts.Add(new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketGuestsNotAllowed".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else if (carriedPawn.GetExtraHostFaction(null) != null)
					{
						opts.Add(new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketGuestPrisonersNotAllowed".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else
					{
						Action action2 = delegate()
						{
							Job job = JobMaker.MakeJob(JobDefOf.CarryToCryptosleepCasketDrafted, carriedPawn, casket);
							job.count = 1;
							job.playerForced = true;
							pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						};
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString2, action2, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, casket, "ReservedBy"));
					}
				}
			}
			if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				foreach (LocalTargetInfo localTargetInfo3 in GenUI.TargetsAt(clickPos, TargetingParameters.ForTend(pawn), true, null))
				{
					Pawn tendTarget = (Pawn)localTargetInfo3.Thing;
					if (!HealthAIUtility.ShouldBeTendedNowByPlayer(tendTarget))
					{
						opts.Add(new FloatMenuOption("CannotTend".Translate(tendTarget) + ": " + "TendingNotRequired".Translate(tendTarget), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
					{
						opts.Add(new FloatMenuOption("CannotTend".Translate(tendTarget) + ": " + "CannotPrioritizeWorkTypeDisabled".Translate(WorkTypeDefOf.Doctor.gerundLabel), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else if (!pawn.CanReach(tendTarget, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotTend".Translate(tendTarget) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else
					{
						Thing medicine = HealthAIUtility.FindBestMedicine(pawn, tendTarget, true);
						TaggedString taggedString3 = "Tend".Translate(tendTarget);
						if (medicine == null)
						{
							taggedString3 += " (" + "WithoutMedicine".Translate() + ")";
						}
						FloatMenuOption item4 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString3, delegate()
						{
							Job job = JobMaker.MakeJob(JobDefOf.TendPatient, tendTarget, medicine);
							job.count = 1;
							job.draftedTend = true;
							pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, tendTarget, "ReservedBy");
						opts.Add(item4);
					}
				}
				if (!pawn.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled)
				{
					foreach (LocalTargetInfo localTargetInfo4 in GenUI.TargetsAt(clickPos, TargetingParameters.ForRepair(pawn), true, null))
					{
						Thing repairTarget = localTargetInfo4.Thing;
						if (!pawn.CanReach(repairTarget, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotRepair".Translate(repairTarget) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						else if (RepairUtility.PawnCanRepairNow(pawn, repairTarget))
						{
							FloatMenuOption item5 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RepairThing".Translate(repairTarget), delegate()
							{
								Job job = JobMaker.MakeJob(JobDefOf.Repair, repairTarget);
								pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, repairTarget, "ReservedBy");
							opts.Add(item5);
						}
					}
				}
			}
			FloatMenuMakerMap.AddJobGiverWorkOrders(clickPos, pawn, opts, true);
			FloatMenuOption floatMenuOption3 = FloatMenuMakerMap.GotoLocationOption(clickCell, pawn, suppressAutoTakeableGoto);
			if (floatMenuOption3 != null)
			{
				opts.Add(floatMenuOption3);
			}
		}

		// Token: 0x060076C8 RID: 30408 RVA: 0x00295540 File Offset: 0x00293740
		private static void AddHumanlikeOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			FloatMenuMakerMap.<>c__DisplayClass9_0 CS$<>8__locals1 = new FloatMenuMakerMap.<>c__DisplayClass9_0();
			CS$<>8__locals1.pawn = pawn;
			IntVec3 c = IntVec3.FromVector3(clickPos);
			using (List<Thing>.Enumerator enumerator = c.GetThingList(CS$<>8__locals1.pawn.Map).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn2;
					if ((pawn2 = (enumerator.Current as Pawn)) != null)
					{
						Lord lord = pawn2.GetLord();
						if (lord != null && lord.CurLordToil != null)
						{
							IEnumerable<FloatMenuOption> enumerable = lord.CurLordToil.ExtraFloatMenuOptions(pawn2, CS$<>8__locals1.pawn);
							if (enumerable != null)
							{
								foreach (FloatMenuOption item7 in enumerable)
								{
									opts.Add(item7);
								}
							}
						}
					}
				}
			}
			if (CS$<>8__locals1.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				foreach (LocalTargetInfo dest in GenUI.TargetsAt(clickPos, TargetingParameters.ForArrest(CS$<>8__locals1.pawn), true, null))
				{
					bool flag = dest.HasThing && dest.Thing is Pawn && ((Pawn)dest.Thing).IsWildMan();
					if (CS$<>8__locals1.pawn.Drafted || flag)
					{
						if (dest.Thing is Pawn && (CS$<>8__locals1.pawn.InSameExtraFaction((Pawn)dest.Thing, ExtraFactionType.HomeFaction, null) || CS$<>8__locals1.pawn.InSameExtraFaction((Pawn)dest.Thing, ExtraFactionType.MiniFaction, null)))
						{
							opts.Add(new FloatMenuOption("CannotArrest".Translate() + ": " + "SameFaction".Translate((Pawn)dest.Thing), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						else if (!CS$<>8__locals1.pawn.CanReach(dest, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotArrest".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						else
						{
							Pawn pTarg = (Pawn)dest.Thing;
							Action action = delegate()
							{
								Building_Bed building_Bed = RestUtility.FindBedFor(pTarg, CS$<>8__locals1.pawn, false, false, new GuestStatus?(GuestStatus.Prisoner));
								if (building_Bed == null)
								{
									building_Bed = RestUtility.FindBedFor(pTarg, CS$<>8__locals1.pawn, false, true, new GuestStatus?(GuestStatus.Prisoner));
								}
								if (building_Bed == null)
								{
									Messages.Message("CannotArrest".Translate() + ": " + "NoPrisonerBed".Translate(), pTarg, MessageTypeDefOf.RejectInput, false);
									return;
								}
								Job job = JobMaker.MakeJob(JobDefOf.Arrest, pTarg, building_Bed);
								job.count = 1;
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
								if (pTarg.Faction != null && ((pTarg.Faction != Faction.OfPlayer && !pTarg.Faction.Hidden) || pTarg.IsQuestLodger()))
								{
									TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.ArrestingCreatesEnemies, new string[]
									{
										pTarg.GetAcceptArrestChance(CS$<>8__locals1.pawn).ToStringPercent()
									});
								}
							};
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TryToArrest".Translate(dest.Thing.LabelCap, dest.Thing, pTarg.GetAcceptArrestChance(CS$<>8__locals1.pawn).ToStringPercent()), action, MenuOptionPriority.High, null, dest.Thing, 0f, null, null, true, 0), CS$<>8__locals1.pawn, pTarg, "ReservedBy"));
						}
					}
				}
			}
			foreach (Thing t4 in c.GetThingList(CS$<>8__locals1.pawn.Map))
			{
				Thing t = t4;
				if (t.def.ingestible != null && CS$<>8__locals1.pawn.RaceProps.CanEverEat(t) && t.IngestibleNow)
				{
					string text;
					if (t.def.ingestible.ingestCommandString.NullOrEmpty())
					{
						text = "ConsumeThing".Translate(t.LabelShort, t);
					}
					else
					{
						text = string.Format(t.def.ingestible.ingestCommandString, t.LabelShort);
					}
					if (!t.IsSociallyProper(CS$<>8__locals1.pawn))
					{
						text = text + ": " + "ReservedForPrisoners".Translate().CapitalizeFirst();
					}
					FloatMenuOption floatMenuOption;
					if ((!t.def.IsDrug || !ModsConfig.IdeologyActive || new HistoryEvent(HistoryEventDefOf.IngestedDrug, CS$<>8__locals1.pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo(out floatMenuOption, text)) && (!t.def.IsNonMedicalDrug || !ModsConfig.IdeologyActive || new HistoryEvent(HistoryEventDefOf.IngestedRecreationalDrug, CS$<>8__locals1.pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo(out floatMenuOption, text)) && (!t.def.IsDrug || !ModsConfig.IdeologyActive || t.def.ingestible.drugCategory != DrugCategory.Hard || new HistoryEvent(HistoryEventDefOf.IngestedHardDrug, CS$<>8__locals1.pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo(out floatMenuOption, text)))
					{
						if (t.def.IsNonMedicalDrug && CS$<>8__locals1.pawn.IsTeetotaler())
						{
							floatMenuOption = new FloatMenuOption(text + ": " + TraitDefOf.DrugDesire.DataAtDegree(-1).GetLabelCapFor(CS$<>8__locals1.pawn), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
						}
						else if (FoodUtility.InappropriateForTitle(t.def, CS$<>8__locals1.pawn, true))
						{
							floatMenuOption = new FloatMenuOption(text + ": " + "FoodBelowTitleRequirements".Translate(CS$<>8__locals1.pawn.royalty.MostSeniorTitle.def.GetLabelFor(CS$<>8__locals1.pawn)), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
						}
						else if (!CS$<>8__locals1.pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							floatMenuOption = new FloatMenuOption(text + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
						}
						else
						{
							MenuOptionPriority priority = (t is Corpse) ? MenuOptionPriority.Low : MenuOptionPriority.Default;
							bool maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(t, CS$<>8__locals1.pawn, FoodUtility.WillIngestStackCountOf(CS$<>8__locals1.pawn, t.def, t.GetStatValue(StatDefOf.Nutrition, true))) != 0;
							floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
							{
								int maxAmountToPickup2 = FoodUtility.GetMaxAmountToPickup(t, CS$<>8__locals1.pawn, FoodUtility.WillIngestStackCountOf(CS$<>8__locals1.pawn, t.def, t.GetStatValue(StatDefOf.Nutrition, true)));
								if (maxAmountToPickup2 == 0)
								{
									return;
								}
								t.SetForbidden(false, true);
								Job job = JobMaker.MakeJob(JobDefOf.Ingest, t);
								job.count = maxAmountToPickup2;
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							}, priority, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, t, "ReservedBy");
							if (!maxAmountToPickup)
							{
								floatMenuOption.action = null;
							}
						}
					}
					opts.Add(floatMenuOption);
				}
			}
			foreach (LocalTargetInfo dest2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForQuestPawnsWhoWillJoinColony(CS$<>8__locals1.pawn), true, null))
			{
				Pawn toHelpPawn = (Pawn)dest2.Thing;
				FloatMenuOption item2;
				if (!CS$<>8__locals1.pawn.CanReach(dest2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					item2 = new FloatMenuOption("CannotGoNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					item2 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(toHelpPawn.IsPrisoner ? "FreePrisoner".Translate() : "OfferHelp".Translate(), delegate()
					{
						CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.OfferHelp, toHelpPawn), new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.RescueOrCapture, null, toHelpPawn, 0f, null, null, true, 0), CS$<>8__locals1.pawn, toHelpPawn, "ReservedBy");
				}
				opts.Add(item2);
			}
			if (CS$<>8__locals1.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				List<Thing> thingList = c.GetThingList(CS$<>8__locals1.pawn.Map);
				foreach (Thing thing7 in thingList)
				{
					Corpse corpse = thing7 as Corpse;
					if (corpse != null && corpse.IsInValidStorage())
					{
						StoragePriority priority2 = StoreUtility.CurrentHaulDestinationOf(corpse).GetStoreSettings().Priority;
						IHaulDestination haulDestination;
						if (StoreUtility.TryFindBestBetterNonSlotGroupStorageFor(corpse, CS$<>8__locals1.pawn, CS$<>8__locals1.pawn.Map, priority2, Faction.OfPlayer, out haulDestination, true) && haulDestination.GetStoreSettings().Priority == priority2 && haulDestination is Building_Grave)
						{
							Building_Grave grave = haulDestination as Building_Grave;
							string label = "PrioritizeGeneric".Translate("Burying".Translate(), corpse.Label).CapitalizeFirst();
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, delegate()
							{
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(HaulAIUtility.HaulToContainerJob(CS$<>8__locals1.pawn, corpse, grave), new JobTag?(JobTag.Misc), false);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, new LocalTargetInfo(corpse), "ReservedBy"));
						}
					}
				}
				foreach (Thing thing2 in thingList)
				{
					Corpse corpse = thing2 as Corpse;
					if (corpse != null)
					{
						Building_GibbetCage cage = Building_GibbetCage.FindGibbetCageFor(corpse, CS$<>8__locals1.pawn, false);
						if (cage != null)
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(corpse, cage), delegate()
							{
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(HaulAIUtility.HaulToContainerJob(CS$<>8__locals1.pawn, corpse, cage), new JobTag?(JobTag.Misc), false);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, new LocalTargetInfo(corpse), "ReservedBy"));
						}
					}
				}
				foreach (LocalTargetInfo localTargetInfo in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(CS$<>8__locals1.pawn), true, null))
				{
					Pawn victim = (Pawn)localTargetInfo.Thing;
					if (!victim.InBed() && CS$<>8__locals1.pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && !victim.mindState.WillJoinColonyIfRescued)
					{
						if (!victim.IsPrisonerOfColony && !victim.IsSlaveOfColony && (!victim.InMentalState || victim.health.hediffSet.HasHediff(HediffDefOf.Scaria, false)) && (victim.Faction == Faction.OfPlayer || victim.Faction == null || !victim.Faction.HostileTo(Faction.OfPlayer)))
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Rescue".Translate(victim.LabelCap, victim), delegate()
							{
								Building_Bed building_Bed = RestUtility.FindBedFor(victim, CS$<>8__locals1.pawn, false, false, null);
								if (building_Bed == null)
								{
									building_Bed = RestUtility.FindBedFor(victim, CS$<>8__locals1.pawn, false, true, null);
								}
								if (building_Bed == null)
								{
									string t5;
									if (victim.RaceProps.Animal)
									{
										t5 = "NoAnimalBed".Translate();
									}
									else
									{
										t5 = "NoNonPrisonerBed".Translate();
									}
									Messages.Message("CannotRescue".Translate() + ": " + t5, victim, MessageTypeDefOf.RejectInput, false);
									return;
								}
								Job job = JobMaker.MakeJob(JobDefOf.Rescue, victim, building_Bed);
								job.count = 1;
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
								PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
							}, MenuOptionPriority.RescueOrCapture, null, victim, 0f, null, null, true, 0), CS$<>8__locals1.pawn, victim, "ReservedBy"));
						}
						if (victim.IsSlaveOfColony && !victim.InMentalState)
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ReturnToSlaveBed".Translate(), delegate()
							{
								Building_Bed building_Bed = RestUtility.FindBedFor(victim, CS$<>8__locals1.pawn, false, false, new GuestStatus?(GuestStatus.Slave));
								if (building_Bed == null)
								{
									building_Bed = RestUtility.FindBedFor(victim, CS$<>8__locals1.pawn, false, true, new GuestStatus?(GuestStatus.Slave));
								}
								if (building_Bed == null)
								{
									Messages.Message("CannotRescue".Translate() + ": " + "NoSlaveBed".Translate(), victim, MessageTypeDefOf.RejectInput, false);
									return;
								}
								Job job = JobMaker.MakeJob(JobDefOf.Rescue, victim, building_Bed);
								job.count = 1;
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
								PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
							}, MenuOptionPriority.RescueOrCapture, null, victim, 0f, null, null, true, 0), CS$<>8__locals1.pawn, victim, "ReservedBy"));
						}
						if (victim.RaceProps.Humanlike && (victim.InMentalState || victim.Faction != Faction.OfPlayer || (victim.Downed && (victim.guilt.IsGuilty || victim.IsPrisonerOfColony))))
						{
							TaggedString taggedString = "Capture".Translate(victim.LabelCap, victim);
							if (victim.Faction != null && victim.Faction != Faction.OfPlayer && !victim.Faction.Hidden && !victim.Faction.HostileTo(Faction.OfPlayer) && !victim.IsPrisonerOfColony)
							{
								taggedString += ": " + "AngersFaction".Translate().CapitalizeFirst();
							}
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
							{
								Building_Bed building_Bed = RestUtility.FindBedFor(victim, CS$<>8__locals1.pawn, false, false, new GuestStatus?(GuestStatus.Prisoner));
								if (building_Bed == null)
								{
									building_Bed = RestUtility.FindBedFor(victim, CS$<>8__locals1.pawn, false, true, new GuestStatus?(GuestStatus.Prisoner));
								}
								if (building_Bed == null)
								{
									Messages.Message("CannotCapture".Translate() + ": " + "NoPrisonerBed".Translate(), victim, MessageTypeDefOf.RejectInput, false);
									return;
								}
								Job job = JobMaker.MakeJob(JobDefOf.Capture, victim, building_Bed);
								job.count = 1;
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
								PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Capturing, KnowledgeAmount.Total);
								if (victim.Faction != null && victim.Faction != Faction.OfPlayer && !victim.Faction.Hidden && !victim.Faction.HostileTo(Faction.OfPlayer) && !victim.IsPrisonerOfColony)
								{
									Messages.Message("MessageCapturingWillAngerFaction".Translate(victim.Named("PAWN")).AdjustedFor(victim, "PAWN", true), victim, MessageTypeDefOf.CautionInput, false);
								}
							}, MenuOptionPriority.RescueOrCapture, null, victim, 0f, null, null, true, 0), CS$<>8__locals1.pawn, victim, "ReservedBy"));
						}
					}
				}
				foreach (LocalTargetInfo localTargetInfo2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(CS$<>8__locals1.pawn), true, null))
				{
					LocalTargetInfo localTargetInfo3 = localTargetInfo2;
					Pawn victim = (Pawn)localTargetInfo3.Thing;
					if (victim.Downed && CS$<>8__locals1.pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, CS$<>8__locals1.pawn, true) != null)
					{
						string text2 = "CarryToCryptosleepCasket".Translate(localTargetInfo3.Thing.LabelCap, localTargetInfo3.Thing);
						JobDef jDef = JobDefOf.CarryToCryptosleepCasket;
						Action action2 = delegate()
						{
							Building_CryptosleepCasket building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, CS$<>8__locals1.pawn, false);
							if (building_CryptosleepCasket == null)
							{
								building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, CS$<>8__locals1.pawn, true);
							}
							if (building_CryptosleepCasket == null)
							{
								Messages.Message("CannotCarryToCryptosleepCasket".Translate() + ": " + "NoCryptosleepCasket".Translate(), victim, MessageTypeDefOf.RejectInput, false);
								return;
							}
							Job job = JobMaker.MakeJob(jDef, victim, building_CryptosleepCasket);
							job.count = 1;
							CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						};
						if (victim.IsQuestLodger())
						{
							text2 += " (" + "CryptosleepCasketGuestsNotAllowed".Translate() + ")";
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text2, null, MenuOptionPriority.Default, null, victim, 0f, null, null, true, 0), CS$<>8__locals1.pawn, victim, "ReservedBy"));
						}
						else if (victim.GetExtraHostFaction(null) != null)
						{
							text2 += " (" + "CryptosleepCasketGuestPrisonersNotAllowed".Translate() + ")";
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text2, null, MenuOptionPriority.Default, null, victim, 0f, null, null, true, 0), CS$<>8__locals1.pawn, victim, "ReservedBy"));
						}
						else
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text2, action2, MenuOptionPriority.Default, null, victim, 0f, null, null, true, 0), CS$<>8__locals1.pawn, victim, "ReservedBy"));
						}
					}
				}
				if (ModsConfig.IdeologyActive)
				{
					foreach (LocalTargetInfo localTargetInfo4 in GenUI.TargetsAt(clickPos, TargetingParameters.ForCarryToBiosculpterPod(CS$<>8__locals1.pawn), true, null))
					{
						Pawn pawn3 = (Pawn)localTargetInfo4.Thing;
						if ((pawn3.IsColonist && pawn3.Downed) || pawn3.IsPrisonerOfColony)
						{
							CompBiosculpterPod.AddCarryToPodJobs(opts, CS$<>8__locals1.pawn, pawn3);
						}
					}
				}
				if (ModsConfig.RoyaltyActive)
				{
					foreach (LocalTargetInfo localTargetInfo5 in GenUI.TargetsAt(clickPos, TargetingParameters.ForShuttle(CS$<>8__locals1.pawn), true, null))
					{
						LocalTargetInfo localTargetInfo6 = localTargetInfo5;
						Pawn victim = (Pawn)localTargetInfo6.Thing;
						Predicate<Thing> validator = delegate(Thing thing)
						{
							CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
							return compShuttle != null && compShuttle.IsAllowedNow(victim);
						};
						Thing shuttleThing = GenClosest.ClosestThingReachable(victim.Position, victim.Map, ThingRequest.ForDef(ThingDefOf.Shuttle), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
						if (shuttleThing != null && CS$<>8__locals1.pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && !CS$<>8__locals1.pawn.WorkTypeIsDisabled(WorkTypeDefOf.Hauling))
						{
							string label2 = "CarryToShuttle".Translate(localTargetInfo6.Thing);
							Action action3 = delegate()
							{
								CompShuttle compShuttle = shuttleThing.TryGetComp<CompShuttle>();
								if (!compShuttle.LoadingInProgressOrReadyToLaunch)
								{
									TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(compShuttle.Transporter));
								}
								Job job = JobMaker.MakeJob(JobDefOf.HaulToTransporter, victim, shuttleThing);
								job.ignoreForbidden = true;
								job.count = 1;
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							};
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label2, action3, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, victim, "ReservedBy"));
						}
					}
				}
				if (ModsConfig.IdeologyActive)
				{
					using (List<Thing>.Enumerator enumerator = thingList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Thing thing = enumerator.Current;
							CompHackable compHackable = thing.TryGetComp<CompHackable>();
							if (compHackable != null)
							{
								if (compHackable.IsHacked)
								{
									opts.Add(new FloatMenuOption("CannotHack".Translate(thing.Label) + ": " + "AlreadyHacked".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
								else if (!HackUtility.IsCapableOfHacking(CS$<>8__locals1.pawn))
								{
									opts.Add(new FloatMenuOption("CannotHack".Translate(thing.Label) + ": " + "IncapableOfHacking".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
								else if (!CS$<>8__locals1.pawn.CanReach(thing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
								{
									opts.Add(new FloatMenuOption("CannotHack".Translate(thing.Label) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
								else if (thing.def == ThingDefOf.AncientEnemyTerminal)
								{
									Action <>9__12;
									opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Hack".Translate(thing.Label), delegate()
									{
										WindowStack windowStack = Find.WindowStack;
										TaggedString text6 = "ConfirmHackEnenyTerminal".Translate(ThingDefOf.AncientEnemyTerminal.label);
										Action confirmedAct;
										if ((confirmedAct = <>9__12) == null)
										{
											confirmedAct = (<>9__12 = delegate()
											{
												CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Hack, thing), new JobTag?(JobTag.Misc), false);
											});
										}
										windowStack.Add(Dialog_MessageBox.CreateConfirmation(text6, confirmedAct, false, null));
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, new LocalTargetInfo(thing), "ReservedBy"));
								}
								else
								{
									TaggedString taggedString2 = (thing.def == ThingDefOf.AncientCommsConsole) ? "Hack".Translate("ToDropSupplies".Translate()) : "Hack".Translate(thing.Label);
									opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString2, delegate()
									{
										CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Hack, thing), new JobTag?(JobTag.Misc), false);
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, new LocalTargetInfo(thing), "ReservedBy"));
								}
							}
						}
					}
					using (IEnumerator<LocalTargetInfo> enumerator3 = GenUI.TargetsAt(clickPos, TargetingParameters.ForBuilding(ThingDefOf.ArchonexusCore), false, null).GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							LocalTargetInfo thing = enumerator3.Current;
							if (!CS$<>8__locals1.pawn.CanReach(thing, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
							{
								opts.Add(new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							else if (!((Building_ArchonexusCore)((Thing)thing)).CanActivateNow)
							{
								opts.Add(new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "AlreadyInvoked".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							else
							{
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Invoke".Translate("Power".Translate()), delegate()
								{
									CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.ActivateArchonexusCore, thing), new JobTag?(JobTag.Misc), false);
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, thing, "ReservedBy"));
							}
						}
					}
				}
				if (ModsConfig.IdeologyActive)
				{
					using (List<Thing>.Enumerator enumerator = thingList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							FloatMenuMakerMap.<>c__DisplayClass9_13 CS$<>8__locals14 = new FloatMenuMakerMap.<>c__DisplayClass9_13();
							CS$<>8__locals14.CS$<>8__locals13 = CS$<>8__locals1;
							CS$<>8__locals14.thing = enumerator.Current;
							CompRelicContainer container = CS$<>8__locals14.thing.TryGetComp<CompRelicContainer>();
							if (container != null)
							{
								if (container.Full)
								{
									string text3 = "ExtractRelic".Translate(container.ContainedThing.Label);
									IntVec3 c2;
									IHaulDestination haulDestination2;
									if (!StoreUtility.TryFindBestBetterStorageFor(container.ContainedThing, CS$<>8__locals14.CS$<>8__locals13.pawn, CS$<>8__locals14.CS$<>8__locals13.pawn.Map, StoragePriority.Unstored, CS$<>8__locals14.CS$<>8__locals13.pawn.Faction, out c2, out haulDestination2, true))
									{
										opts.Add(new FloatMenuOption(text3 + " (" + HaulAIUtility.NoEmptyPlaceLowerTrans + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
									}
									else
									{
										Job job = JobMaker.MakeJob(JobDefOf.ExtractRelic, CS$<>8__locals14.thing, container.ContainedThing, c2);
										job.count = 1;
										opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text3, delegate()
										{
											CS$<>8__locals14.CS$<>8__locals13.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
										}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals14.CS$<>8__locals13.pawn, new LocalTargetInfo(CS$<>8__locals14.thing), "ReservedBy"));
									}
								}
								else
								{
									IEnumerable<Thing> allThings = CS$<>8__locals14.CS$<>8__locals13.pawn.Map.listerThings.AllThings;
									Func<Thing, bool> predicate;
									if ((predicate = CS$<>8__locals14.CS$<>8__locals13.<>9__17) == null)
									{
										predicate = (CS$<>8__locals14.CS$<>8__locals13.<>9__17 = ((Thing x) => CompRelicContainer.IsRelic(x) && CS$<>8__locals14.CS$<>8__locals13.pawn.CanReach(x, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn)));
									}
									IEnumerable<Thing> enumerable2 = allThings.Where(predicate);
									if (!enumerable2.Any<Thing>())
									{
										opts.Add(new FloatMenuOption("NoRelicToInstall".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
									}
									else
									{
										foreach (Thing thing3 in enumerable2)
										{
											Job job = JobMaker.MakeJob(JobDefOf.InstallRelic, thing3, CS$<>8__locals14.thing, CS$<>8__locals14.thing.InteractionCell);
											job.count = 1;
											opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("InstallRelic".Translate(thing3.Label), delegate()
											{
												CS$<>8__locals14.CS$<>8__locals13.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
											}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals14.CS$<>8__locals13.pawn, new LocalTargetInfo(CS$<>8__locals14.thing), "ReservedBy"));
										}
									}
								}
								if (!CS$<>8__locals14.CS$<>8__locals13.pawn.Map.IsPlayerHome && !CS$<>8__locals14.CS$<>8__locals13.pawn.IsFormingCaravan() && container.Full)
								{
									opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ExtractRelicToInventory".Translate(container.ContainedThing.Label, 300.ToStringTicksToPeriod(true, false, true, true)), delegate()
									{
										Job job = JobMaker.MakeJob(JobDefOf.ExtractToInventory, CS$<>8__locals14.thing, container.ContainedThing, CS$<>8__locals14.thing.InteractionCell);
										job.count = 1;
										CS$<>8__locals14.CS$<>8__locals13.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals14.CS$<>8__locals13.pawn, new LocalTargetInfo(CS$<>8__locals14.thing), "ReservedBy"));
								}
							}
						}
					}
					foreach (Thing thing4 in thingList)
					{
						if (CompRelicContainer.IsRelic(thing4))
						{
							IEnumerable<Thing> enumerable3 = from x in thing4.Map.listerThings.ThingsOfDef(ThingDefOf.Reliquary)
							where x.TryGetComp<CompRelicContainer>().ContainedThing == null
							select x;
							IntVec3 position = thing4.Position;
							Map map = thing4.Map;
							IEnumerable<Thing> searchSet = enumerable3;
							PathEndMode peMode = PathEndMode.Touch;
							TraverseParms traverseParams = TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
							float maxDistance = 9999f;
							Predicate<Thing> validator2;
							if ((validator2 = CS$<>8__locals1.<>9__20) == null)
							{
								validator2 = (CS$<>8__locals1.<>9__20 = ((Thing t) => CS$<>8__locals1.pawn.CanReserve(t, 1, -1, null, false)));
							}
							Thing thing5 = GenClosest.ClosestThing_Global_Reachable(position, map, searchSet, peMode, traverseParams, maxDistance, validator2, null);
							if (thing5 == null)
							{
								opts.Add(new FloatMenuOption("InstallInReliquary".Translate() + " (" + "NoEmptyReliquary".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							else
							{
								Job job = JobMaker.MakeJob(JobDefOf.InstallRelic, thing4, thing5, thing5.InteractionCell);
								job.count = 1;
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("InstallInReliquary".Translate(), delegate()
								{
									CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, new LocalTargetInfo(thing4), "ReservedBy"));
							}
						}
					}
				}
			}
			foreach (LocalTargetInfo stripTarg2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForStrip(CS$<>8__locals1.pawn), true, null))
			{
				LocalTargetInfo stripTarg = stripTarg2;
				FloatMenuOption item3;
				if (!CS$<>8__locals1.pawn.CanReach(stripTarg, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					item3 = new FloatMenuOption("CannotStrip".Translate(stripTarg.Thing.LabelCap, stripTarg.Thing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else if (stripTarg.Pawn != null && stripTarg.Pawn.HasExtraHomeFaction(null))
				{
					item3 = new FloatMenuOption("CannotStrip".Translate(stripTarg.Thing.LabelCap, stripTarg.Thing) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					item3 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Strip".Translate(stripTarg.Thing.LabelCap, stripTarg.Thing), delegate()
					{
						stripTarg.Thing.SetForbidden(false, false);
						CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Strip, stripTarg), new JobTag?(JobTag.Misc), false);
						StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(stripTarg.Thing);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, stripTarg, "ReservedBy");
				}
				opts.Add(item3);
			}
			if (CS$<>8__locals1.pawn.equipment != null)
			{
				ThingWithComps equipment = null;
				List<Thing> thingList2 = c.GetThingList(CS$<>8__locals1.pawn.Map);
				for (int i = 0; i < thingList2.Count; i++)
				{
					if (thingList2[i].TryGetComp<CompEquippable>() != null)
					{
						equipment = (ThingWithComps)thingList2[i];
						break;
					}
				}
				if (equipment != null)
				{
					string labelShort = equipment.LabelShort;
					FloatMenuOption item4;
					string str;
					if (equipment.def.IsWeapon && CS$<>8__locals1.pawn.WorkTagIsDisabled(WorkTags.Violent))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfViolenceLower".Translate(CS$<>8__locals1.pawn.LabelShort, CS$<>8__locals1.pawn), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (equipment.def.IsRangedWeapon && CS$<>8__locals1.pawn.WorkTagIsDisabled(WorkTags.Shooting))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfShootingLower".Translate(CS$<>8__locals1.pawn), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (!CS$<>8__locals1.pawn.CanReach(equipment, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (!CS$<>8__locals1.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "Incapable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (equipment.IsBurning())
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "BurningLower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (CS$<>8__locals1.pawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanEquip(equipment, CS$<>8__locals1.pawn))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (!EquipmentUtility.CanEquip(equipment, CS$<>8__locals1.pawn, out str, false))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + str.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						string text4 = "Equip".Translate(labelShort);
						if (equipment.def.IsRangedWeapon && CS$<>8__locals1.pawn.story != null && CS$<>8__locals1.pawn.story.traits.HasTrait(TraitDefOf.Brawler))
						{
							text4 += " " + "EquipWarningBrawler".Translate();
						}
						if (EquipmentUtility.AlreadyBondedToWeapon(equipment, CS$<>8__locals1.pawn))
						{
							text4 += " " + "BladelinkAlreadyBonded".Translate();
							TaggedString dialogText = "BladelinkAlreadyBondedDialog".Translate(CS$<>8__locals1.pawn.Named("PAWN"), equipment.Named("WEAPON"), CS$<>8__locals1.pawn.equipment.bondedWeapon.Named("BONDEDWEAPON"));
							item4 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text4, delegate()
							{
								Find.WindowStack.Add(new Dialog_MessageBox(dialogText, null, null, null, null, null, false, null, null));
							}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, equipment, "ReservedBy");
						}
						else
						{
							Action <>9__26;
							item4 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text4, delegate()
							{
								string personaWeaponConfirmationText = EquipmentUtility.GetPersonaWeaponConfirmationText(equipment, CS$<>8__locals1.pawn);
								if (!personaWeaponConfirmationText.NullOrEmpty())
								{
									WindowStack windowStack = Find.WindowStack;
									TaggedString text6 = personaWeaponConfirmationText;
									string buttonAText = "Yes".Translate();
									Action buttonAAction;
									if ((buttonAAction = <>9__26) == null)
									{
										buttonAAction = (<>9__26 = delegate()
										{
											base.<AddHumanlikeOrders>g__Equip|25();
										});
									}
									windowStack.Add(new Dialog_MessageBox(text6, buttonAText, buttonAAction, "No".Translate(), null, null, false, null, null));
									return;
								}
								base.<AddHumanlikeOrders>g__Equip|25();
							}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, equipment, "ReservedBy");
						}
					}
					opts.Add(item4);
				}
			}
			foreach (Pair<CompReloadable, Thing> pair in ReloadableUtility.FindPotentiallyReloadableGear(CS$<>8__locals1.pawn, c.GetThingList(CS$<>8__locals1.pawn.Map)))
			{
				CompReloadable comp = pair.First;
				Thing second = pair.Second;
				string text5 = "Reload".Translate(comp.parent.Named("GEAR"), comp.AmmoDef.Named("AMMO")) + " (" + comp.LabelRemaining + ")";
				List<Thing> chosenAmmo;
				if (!CS$<>8__locals1.pawn.CanReach(second, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					opts.Add(new FloatMenuOption(text5 + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else if (!comp.NeedsReload(true))
				{
					opts.Add(new FloatMenuOption(text5 + ": " + "ReloadFull".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else if ((chosenAmmo = ReloadableUtility.FindEnoughAmmo(CS$<>8__locals1.pawn, second.Position, comp, true)) == null)
				{
					opts.Add(new FloatMenuOption(text5 + ": " + "ReloadNotEnough".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else
				{
					Action action4 = delegate()
					{
						CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(JobGiver_Reload.MakeReloadJob(comp, chosenAmmo), new JobTag?(JobTag.Misc), false);
					};
					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text5, action4, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, second, "ReservedBy"));
				}
			}
			if (CS$<>8__locals1.pawn.apparel != null)
			{
				Apparel apparel = CS$<>8__locals1.pawn.Map.thingGrid.ThingAt<Apparel>(c);
				if (apparel != null)
				{
					string key = "CannotWear";
					string key2 = "ForceWear";
					if (apparel.def.apparel.LastLayer.IsUtilityLayer)
					{
						key = "CannotEquipApparel";
						key2 = "ForceEquipApparel";
					}
					FloatMenuOption item5;
					string t2;
					if (!CS$<>8__locals1.pawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						item5 = new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (apparel.IsBurning())
					{
						item5 = new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (CS$<>8__locals1.pawn.apparel.WouldReplaceLockedApparel(apparel))
					{
						item5 = new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (!ApparelUtility.HasPartsToWear(CS$<>8__locals1.pawn, apparel.def))
					{
						item5 = new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (!EquipmentUtility.CanEquip(apparel, CS$<>8__locals1.pawn, out t2, true))
					{
						item5 = new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + t2, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						item5 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(key2.Translate(apparel.LabelShort, apparel), delegate()
						{
							apparel.SetForbidden(false, true);
							Job job = JobMaker.MakeJob(JobDefOf.Wear, apparel);
							CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, apparel, "ReservedBy");
					}
					opts.Add(item5);
				}
			}
			if (CS$<>8__locals1.pawn.IsFormingCaravan())
			{
				Thing item = c.GetFirstItem(CS$<>8__locals1.pawn.Map);
				if (item != null && item.def.EverHaulable && item.def.canLoadIntoCaravan)
				{
					Pawn packTarget = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(CS$<>8__locals1.pawn) ?? CS$<>8__locals1.pawn;
					JobDef jobDef = (packTarget == CS$<>8__locals1.pawn) ? JobDefOf.TakeInventory : JobDefOf.GiveToPackAnimal;
					if (!CS$<>8__locals1.pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotLoadIntoCaravan".Translate(item.Label, item) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else if (MassUtility.WillBeOverEncumberedAfterPickingUp(packTarget, item, 1))
					{
						opts.Add(new FloatMenuOption("CannotLoadIntoCaravan".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else
					{
						LordJob_FormAndSendCaravan lordJob = (LordJob_FormAndSendCaravan)CS$<>8__locals1.pawn.GetLord().LordJob;
						float capacityLeft = CaravanFormingUtility.CapacityLeft(lordJob);
						if (item.stackCount == 1)
						{
							float capacityLeft4 = capacityLeft - item.GetStatValue(StatDefOf.Mass, true);
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravan".Translate(item.Label, item), capacityLeft4), delegate()
							{
								item.SetForbidden(false, false);
								Job job = JobMaker.MakeJob(jobDef, item);
								job.count = 1;
								job.checkEncumbrance = (packTarget == CS$<>8__locals1.pawn);
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, item, "ReservedBy"));
						}
						else
						{
							if (MassUtility.WillBeOverEncumberedAfterPickingUp(packTarget, item, item.stackCount))
							{
								opts.Add(new FloatMenuOption("CannotLoadIntoCaravanAll".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							else
							{
								float capacityLeft2 = capacityLeft - (float)item.stackCount * item.GetStatValue(StatDefOf.Mass, true);
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravanAll".Translate(item.Label, item), capacityLeft2), delegate()
								{
									item.SetForbidden(false, false);
									Job job = JobMaker.MakeJob(jobDef, item);
									job.count = item.stackCount;
									job.checkEncumbrance = (packTarget == CS$<>8__locals1.pawn);
									CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
								}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, item, "ReservedBy"));
							}
							Action<int> <>9__33;
							Func<int, string> <>9__32;
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("LoadIntoCaravanSome".Translate(item.LabelNoCount, item), delegate()
							{
								int num = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(packTarget, item), item.stackCount);
								Func<int, string> textGetter;
								if ((textGetter = <>9__32) == null)
								{
									textGetter = (<>9__32 = delegate(int val)
									{
										float capacityLeft3 = capacityLeft - (float)val * item.GetStatValue(StatDefOf.Mass, true);
										return CaravanFormingUtility.AppendOverweightInfo(string.Format("LoadIntoCaravanCount".Translate(item.LabelNoCount, item), val), capacityLeft3);
									});
								}
								int from = 1;
								int to = num;
								Action<int> confirmAction;
								if ((confirmAction = <>9__33) == null)
								{
									confirmAction = (<>9__33 = delegate(int count)
									{
										item.SetForbidden(false, false);
										Job job = JobMaker.MakeJob(jobDef, item);
										job.count = count;
										job.checkEncumbrance = (packTarget == CS$<>8__locals1.pawn);
										CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
									});
								}
								Dialog_Slider window = new Dialog_Slider(textGetter, from, to, confirmAction, int.MinValue, 1f);
								Find.WindowStack.Add(window);
							}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, item, "ReservedBy"));
						}
					}
				}
			}
			if (!CS$<>8__locals1.pawn.IsFormingCaravan())
			{
				Thing item = c.GetFirstItem(CS$<>8__locals1.pawn.Map);
				if (item != null && item.def.EverHaulable && PawnUtility.CanPickUp(CS$<>8__locals1.pawn, item.def))
				{
					if (!CS$<>8__locals1.pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotPickUp".Translate(item.Label, item) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else if (MassUtility.WillBeOverEncumberedAfterPickingUp(CS$<>8__locals1.pawn, item, 1))
					{
						opts.Add(new FloatMenuOption("CannotPickUp".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else
					{
						int maxAllowedToPickUp = PawnUtility.GetMaxAllowedToPickUp(CS$<>8__locals1.pawn, item.def);
						if (maxAllowedToPickUp == 0)
						{
							opts.Add(new FloatMenuOption("CannotPickUp".Translate(item.Label, item) + ": " + "MaxPickUpAllowed".Translate(item.def.orderedTakeGroup.max, item.def.orderedTakeGroup.label), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						else if (item.stackCount == 1 || maxAllowedToPickUp == 1)
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUp".Translate(item.Label, item), delegate()
							{
								item.SetForbidden(false, false);
								Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, item);
								job.count = 1;
								job.checkEncumbrance = true;
								job.takeInventoryDelay = 120;
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, item, "ReservedBy"));
						}
						else
						{
							if (maxAllowedToPickUp < item.stackCount)
							{
								opts.Add(new FloatMenuOption("CannotPickUpAll".Translate(item.Label, item) + ": " + "MaxPickUpAllowed".Translate(item.def.orderedTakeGroup.max, item.def.orderedTakeGroup.label), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							else if (MassUtility.WillBeOverEncumberedAfterPickingUp(CS$<>8__locals1.pawn, item, item.stackCount))
							{
								opts.Add(new FloatMenuOption("CannotPickUpAll".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							else
							{
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpAll".Translate(item.Label, item), delegate()
								{
									item.SetForbidden(false, false);
									Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, item);
									job.count = item.stackCount;
									job.checkEncumbrance = true;
									job.takeInventoryDelay = 120;
									CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
								}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, item, "ReservedBy"));
							}
							Action<int> <>9__37;
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpSome".Translate(item.LabelNoCount, item), delegate()
							{
								int b = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(CS$<>8__locals1.pawn, item), item.stackCount);
								int num = Mathf.Min(maxAllowedToPickUp, b);
								string text6 = "PickUpCount".Translate(item.LabelNoCount, item);
								int from = 1;
								int to = num;
								Action<int> confirmAction;
								if ((confirmAction = <>9__37) == null)
								{
									confirmAction = (<>9__37 = delegate(int count)
									{
										item.SetForbidden(false, false);
										Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, item);
										job.count = count;
										job.checkEncumbrance = true;
										job.takeInventoryDelay = 120;
										CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
									});
								}
								Dialog_Slider window = new Dialog_Slider(text6, from, to, confirmAction, int.MinValue, 1f);
								Find.WindowStack.Add(window);
							}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, item, "ReservedBy"));
						}
					}
				}
			}
			if (!CS$<>8__locals1.pawn.Map.IsPlayerHome && !CS$<>8__locals1.pawn.IsFormingCaravan())
			{
				Thing item = c.GetFirstItem(CS$<>8__locals1.pawn.Map);
				if (item != null && item.def.EverHaulable)
				{
					Pawn bestPackAnimal = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(CS$<>8__locals1.pawn);
					if (bestPackAnimal != null)
					{
						if (!CS$<>8__locals1.pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotGiveToPackAnimal".Translate(item.Label, item) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						else if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, item, 1))
						{
							opts.Add(new FloatMenuOption("CannotGiveToPackAnimal".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						else if (item.stackCount == 1)
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimal".Translate(item.Label, item), delegate()
							{
								item.SetForbidden(false, false);
								Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, item);
								job.count = 1;
								CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, item, "ReservedBy"));
						}
						else
						{
							if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, item, item.stackCount))
							{
								opts.Add(new FloatMenuOption("CannotGiveToPackAnimalAll".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							else
							{
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalAll".Translate(item.Label, item), delegate()
								{
									item.SetForbidden(false, false);
									Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, item);
									job.count = item.stackCount;
									CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
								}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, item, "ReservedBy"));
							}
							Action<int> <>9__41;
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalSome".Translate(item.LabelNoCount, item), delegate()
							{
								int num = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(bestPackAnimal, item), item.stackCount);
								string text6 = "GiveToPackAnimalCount".Translate(item.LabelNoCount, item);
								int from = 1;
								int to = num;
								Action<int> confirmAction;
								if ((confirmAction = <>9__41) == null)
								{
									confirmAction = (<>9__41 = delegate(int count)
									{
										item.SetForbidden(false, false);
										Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, item);
										job.count = count;
										CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
									});
								}
								Dialog_Slider window = new Dialog_Slider(text6, from, to, confirmAction, int.MinValue, 1f);
								Find.WindowStack.Add(window);
							}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, item, "ReservedBy"));
						}
					}
				}
			}
			if (!CS$<>8__locals1.pawn.Map.IsPlayerHome && CS$<>8__locals1.pawn.Map.exitMapGrid.MapUsesExitGrid)
			{
				foreach (LocalTargetInfo target in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(CS$<>8__locals1.pawn), true, null))
				{
					Pawn p = (Pawn)target.Thing;
					if (p.Faction == Faction.OfPlayer || p.IsPrisonerOfColony || CaravanUtility.ShouldAutoCapture(p, Faction.OfPlayer))
					{
						if (!CS$<>8__locals1.pawn.CanReach(p, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotCarryToExit".Translate(p.Label, p) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						else
						{
							IntVec3 exitSpot;
							if (!RCellFinder.TryFindBestExitSpot(CS$<>8__locals1.pawn, out exitSpot, TraverseMode.ByPawn))
							{
								opts.Add(new FloatMenuOption("CannotCarryToExit".Translate(p.Label, p) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							else
							{
								TaggedString taggedString3 = (p.Faction == Faction.OfPlayer || p.IsPrisonerOfColony) ? "CarryToExit".Translate(p.Label, p) : "CarryToExitAndCapture".Translate(p.Label, p);
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString3, delegate()
								{
									Job job = JobMaker.MakeJob(JobDefOf.CarryDownedPawnToExit, p, exitSpot);
									job.count = 1;
									job.failIfCantJoinOrCreateCaravan = true;
									CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
								}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, target, "ReservedBy"));
							}
						}
					}
				}
			}
			if (CS$<>8__locals1.pawn.equipment != null && CS$<>8__locals1.pawn.equipment.Primary != null && GenUI.TargetsAt(clickPos, TargetingParameters.ForSelf(CS$<>8__locals1.pawn), true, null).Any<LocalTargetInfo>())
			{
				if (CS$<>8__locals1.pawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanUnequip(CS$<>8__locals1.pawn.equipment.Primary, CS$<>8__locals1.pawn))
				{
					opts.Add(new FloatMenuOption("CannotDrop".Translate(CS$<>8__locals1.pawn.equipment.Primary.Label, CS$<>8__locals1.pawn.equipment.Primary) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else
				{
					Action action5 = delegate()
					{
						CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DropEquipment, CS$<>8__locals1.pawn.equipment.Primary), new JobTag?(JobTag.Misc), false);
					};
					opts.Add(new FloatMenuOption("Drop".Translate(CS$<>8__locals1.pawn.equipment.Primary.Label, CS$<>8__locals1.pawn.equipment.Primary), action5, MenuOptionPriority.Default, null, CS$<>8__locals1.pawn, 0f, null, null, true, 0));
				}
			}
			foreach (LocalTargetInfo dest3 in GenUI.TargetsAt(clickPos, TargetingParameters.ForTrade(), true, null))
			{
				if (!CS$<>8__locals1.pawn.CanReach(dest3, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					opts.Add(new FloatMenuOption("CannotTrade".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else if (CS$<>8__locals1.pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
				{
					opts.Add(new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(SkillDefOf.Social.LabelCap), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else if (!CS$<>8__locals1.pawn.CanTradeWith(((Pawn)dest3.Thing).Faction, ((Pawn)dest3.Thing).TraderKind).Accepted)
				{
					opts.Add(new FloatMenuOption("CannotTrade".Translate() + ": " + "MissingTitleAbility".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else
				{
					Pawn pTarg = (Pawn)dest3.Thing;
					Action action6 = delegate()
					{
						Job job = JobMaker.MakeJob(JobDefOf.TradeWithPawn, pTarg);
						job.playerForced = true;
						CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InteractingWithTraders, KnowledgeAmount.Total);
					};
					string t3 = "";
					if (pTarg.Faction != null)
					{
						t3 = " (" + pTarg.Faction.Name + ")";
					}
					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TradeWith".Translate(pTarg.LabelShort + ", " + pTarg.TraderKind.label) + t3, action6, MenuOptionPriority.InitiateSocial, null, dest3.Thing, 0f, null, null, true, 0), CS$<>8__locals1.pawn, pTarg, "ReservedBy"));
				}
			}
			using (IEnumerator<LocalTargetInfo> enumerator3 = GenUI.TargetsAt(clickPos, TargetingParameters.ForOpen(CS$<>8__locals1.pawn), true, null).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					LocalTargetInfo casket = enumerator3.Current;
					if (!CS$<>8__locals1.pawn.CanReach(casket, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotOpen".Translate(casket.Thing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else if (!CS$<>8__locals1.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						opts.Add(new FloatMenuOption("CannotOpen".Translate(casket.Thing) + ": " + "Incapable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else if (casket.Thing.Map.designationManager.DesignationOn(casket.Thing, DesignationDefOf.Open) == null)
					{
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Open".Translate(casket.Thing), delegate()
						{
							Job job = JobMaker.MakeJob(JobDefOf.Open, casket.Thing);
							job.ignoreDesignations = true;
							CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals1.pawn, casket.Thing, "ReservedBy"));
					}
				}
			}
			foreach (Thing thing6 in CS$<>8__locals1.pawn.Map.thingGrid.ThingsAt(c))
			{
				foreach (FloatMenuOption item6 in thing6.GetFloatMenuOptions(CS$<>8__locals1.pawn))
				{
					opts.Add(item6);
				}
			}
		}

		// Token: 0x060076C9 RID: 30409 RVA: 0x00299A98 File Offset: 0x00297C98
		private static void AddUndraftedOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			if (FloatMenuMakerMap.equivalenceGroupTempStorage == null || FloatMenuMakerMap.equivalenceGroupTempStorage.Length != DefDatabase<WorkGiverEquivalenceGroupDef>.DefCount)
			{
				FloatMenuMakerMap.equivalenceGroupTempStorage = new FloatMenuOption[DefDatabase<WorkGiverEquivalenceGroupDef>.DefCount];
			}
			IntVec3 c = IntVec3.FromVector3(clickPos);
			bool flag = false;
			bool flag2 = false;
			foreach (Thing t in pawn.Map.thingGrid.ThingsAt(c))
			{
				flag2 = true;
				if (pawn.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					flag = true;
					break;
				}
			}
			if (flag2 && !flag)
			{
				return;
			}
			FloatMenuMakerMap.AddJobGiverWorkOrders(clickPos, pawn, opts, false);
		}

		// Token: 0x060076CA RID: 30410 RVA: 0x00299B44 File Offset: 0x00297D44
		private static void AddJobGiverWorkOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts, bool drafted)
		{
			if (pawn.thinker.TryGetMainTreeThinkNode<JobGiver_Work>() == null)
			{
				return;
			}
			IntVec3 clickCell = IntVec3.FromVector3(clickPos);
			foreach (Thing thing in GenUI.ThingsUnderMouse(clickPos, 1f, new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = true,
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = false
			}, null))
			{
				bool flag = false;
				foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefsListForReading)
				{
					for (int i = 0; i < workTypeDef.workGiversByPriority.Count; i++)
					{
						WorkGiverDef workGiver = workTypeDef.workGiversByPriority[i];
						if (!drafted || workGiver.canBeDoneWhileDrafted)
						{
							WorkGiver_Scanner workGiver_Scanner = workGiver.Worker as WorkGiver_Scanner;
							if (workGiver_Scanner != null && workGiver_Scanner.def.directOrderable)
							{
								JobFailReason.Clear();
								if ((workGiver_Scanner.PotentialWorkThingRequest.Accepts(thing) || (workGiver_Scanner.PotentialWorkThingsGlobal(pawn) != null && workGiver_Scanner.PotentialWorkThingsGlobal(pawn).Contains(thing))) && !workGiver_Scanner.ShouldSkip(pawn, true))
								{
									Action action = null;
									PawnCapacityDef pawnCapacityDef = workGiver_Scanner.MissingRequiredCapacity(pawn);
									string text;
									if (pawnCapacityDef != null)
									{
										text = "CannotMissingHealthActivities".Translate(pawnCapacityDef.label);
									}
									else
									{
										Job job;
										if (!workGiver_Scanner.HasJobOnThing(pawn, thing, true))
										{
											job = null;
										}
										else
										{
											job = workGiver_Scanner.JobOnThing(pawn, thing, true);
										}
										if (job == null)
										{
											if (JobFailReason.HaveReason)
											{
												if (!JobFailReason.CustomJobString.NullOrEmpty())
												{
													text = "CannotGenericWorkCustom".Translate(JobFailReason.CustomJobString);
												}
												else
												{
													text = "CannotGenericWork".Translate(workGiver_Scanner.def.verb, thing.LabelShort, thing);
												}
												text = text + ": " + JobFailReason.Reason.CapitalizeFirst();
											}
											else
											{
												if (!thing.IsForbidden(pawn))
												{
													goto IL_710;
												}
												if (!thing.Position.InAllowedArea(pawn))
												{
													text = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + pawn.playerSettings.EffectiveAreaRestriction.Label;
												}
												else
												{
													text = "CannotPrioritizeForbidden".Translate(thing.Label, thing);
												}
											}
										}
										else
										{
											WorkTypeDef workType = workGiver_Scanner.def.workType;
											if (pawn.WorkTagIsDisabled(workGiver_Scanner.def.workTags))
											{
												text = "CannotPrioritizeWorkGiverDisabled".Translate(workGiver_Scanner.def.label);
											}
											else if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(job))
											{
												text = "CannotGenericAlreadyAm".Translate(workGiver_Scanner.PostProcessedGerund(job), thing.LabelShort, thing);
											}
											else if (pawn.workSettings.GetPriority(workType) == 0)
											{
												if (pawn.WorkTypeIsDisabled(workType))
												{
													text = "CannotPrioritizeWorkTypeDisabled".Translate(workType.gerundLabel);
												}
												else if ("CannotPrioritizeNotAssignedToWorkType".CanTranslate())
												{
													text = "CannotPrioritizeNotAssignedToWorkType".Translate(workType.gerundLabel);
												}
												else
												{
													text = "CannotPrioritizeWorkTypeDisabled".Translate(workType.pawnLabel);
												}
											}
											else if (job.def == JobDefOf.Research && thing is Building_ResearchBench)
											{
												text = "CannotPrioritizeResearch".Translate();
											}
											else if (thing.IsForbidden(pawn))
											{
												if (!thing.Position.InAllowedArea(pawn))
												{
													text = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + pawn.playerSettings.EffectiveAreaRestriction.Label;
												}
												else
												{
													text = "CannotPrioritizeForbidden".Translate(thing.Label, thing);
												}
											}
											else if (!pawn.CanReach(thing, workGiver_Scanner.PathEndMode, Danger.Deadly, false, false, TraverseMode.ByPawn))
											{
												text = (thing.Label + ": " + "NoPath".Translate().CapitalizeFirst()).CapitalizeFirst();
											}
											else
											{
												text = "PrioritizeGeneric".Translate(workGiver_Scanner.PostProcessedGerund(job), thing.Label).CapitalizeFirst();
												Job localJob = job;
												WorkGiver_Scanner localScanner = workGiver_Scanner;
												job.workGiverDef = workGiver_Scanner.def;
												action = delegate()
												{
													if (pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, clickCell))
													{
														if (workGiver.forceMote != null)
														{
															MoteMaker.MakeStaticMote(clickCell, pawn.Map, workGiver.forceMote, 1f);
														}
														if (workGiver.forceFleck != null)
														{
															FleckMaker.Static(clickCell, pawn.Map, workGiver.forceFleck, 1f);
														}
													}
												};
											}
										}
									}
									if (DebugViewSettings.showFloatMenuWorkGivers)
									{
										text += string.Format(" (from {0})", workGiver.defName);
									}
									FloatMenuOption menuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, thing, "ReservedBy");
									if (drafted && workGiver.autoTakeablePriorityDrafted != -1)
									{
										menuOption.autoTakeable = true;
										menuOption.autoTakeablePriority = (float)workGiver.autoTakeablePriorityDrafted;
									}
									if (!opts.Any((FloatMenuOption op) => op.Label == menuOption.Label))
									{
										if (workGiver.equivalenceGroup != null)
										{
											if (FloatMenuMakerMap.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index] == null || (FloatMenuMakerMap.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index].Disabled && !menuOption.Disabled))
											{
												FloatMenuMakerMap.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index] = menuOption;
												flag = true;
											}
										}
										else
										{
											opts.Add(menuOption);
										}
									}
								}
							}
						}
						IL_710:;
					}
				}
				if (flag)
				{
					for (int j = 0; j < FloatMenuMakerMap.equivalenceGroupTempStorage.Length; j++)
					{
						if (FloatMenuMakerMap.equivalenceGroupTempStorage[j] != null)
						{
							opts.Add(FloatMenuMakerMap.equivalenceGroupTempStorage[j]);
							FloatMenuMakerMap.equivalenceGroupTempStorage[j] = null;
						}
					}
				}
			}
			foreach (WorkTypeDef workTypeDef2 in DefDatabase<WorkTypeDef>.AllDefsListForReading)
			{
				for (int k = 0; k < workTypeDef2.workGiversByPriority.Count; k++)
				{
					WorkGiverDef workGiver = workTypeDef2.workGiversByPriority[k];
					if (!drafted || workGiver.canBeDoneWhileDrafted)
					{
						WorkGiver_Scanner workGiver_Scanner2 = workGiver.Worker as WorkGiver_Scanner;
						if (workGiver_Scanner2 != null && workGiver_Scanner2.def.directOrderable)
						{
							JobFailReason.Clear();
							if (workGiver_Scanner2.PotentialWorkCellsGlobal(pawn).Contains(clickCell) && !workGiver_Scanner2.ShouldSkip(pawn, true))
							{
								Action action2 = null;
								string label = null;
								PawnCapacityDef pawnCapacityDef2 = workGiver_Scanner2.MissingRequiredCapacity(pawn);
								if (pawnCapacityDef2 != null)
								{
									label = "CannotMissingHealthActivities".Translate(pawnCapacityDef2.label);
								}
								else
								{
									Job job2;
									if (!workGiver_Scanner2.HasJobOnCell(pawn, clickCell, true))
									{
										job2 = null;
									}
									else
									{
										job2 = workGiver_Scanner2.JobOnCell(pawn, clickCell, true);
									}
									if (job2 == null)
									{
										if (JobFailReason.HaveReason)
										{
											if (!JobFailReason.CustomJobString.NullOrEmpty())
											{
												label = "CannotGenericWorkCustom".Translate(JobFailReason.CustomJobString);
											}
											else
											{
												label = "CannotGenericWork".Translate(workGiver_Scanner2.def.verb, "AreaLower".Translate());
											}
											label = label + ": " + JobFailReason.Reason.CapitalizeFirst();
										}
										else
										{
											if (!clickCell.IsForbidden(pawn))
											{
												goto IL_D6A;
											}
											if (!clickCell.InAllowedArea(pawn))
											{
												label = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + pawn.playerSettings.EffectiveAreaRestriction.Label;
											}
											else
											{
												label = "CannotPrioritizeCellForbidden".Translate();
											}
										}
									}
									else
									{
										WorkTypeDef workType2 = workGiver_Scanner2.def.workType;
										if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(job2))
										{
											label = "CannotGenericAlreadyAmCustom".Translate(workGiver_Scanner2.PostProcessedGerund(job2));
										}
										else if (pawn.workSettings.GetPriority(workType2) == 0)
										{
											if (pawn.WorkTypeIsDisabled(workType2))
											{
												label = "CannotPrioritizeWorkTypeDisabled".Translate(workType2.gerundLabel);
											}
											else if ("CannotPrioritizeNotAssignedToWorkType".CanTranslate())
											{
												label = "CannotPrioritizeNotAssignedToWorkType".Translate(workType2.gerundLabel);
											}
											else
											{
												label = "CannotPrioritizeWorkTypeDisabled".Translate(workType2.pawnLabel);
											}
										}
										else if (clickCell.IsForbidden(pawn))
										{
											if (!clickCell.InAllowedArea(pawn))
											{
												label = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + pawn.playerSettings.EffectiveAreaRestriction.Label;
											}
											else
											{
												label = "CannotPrioritizeCellForbidden".Translate();
											}
										}
										else if (!pawn.CanReach(clickCell, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
										{
											label = "AreaLower".Translate().CapitalizeFirst() + ": " + "NoPath".Translate().CapitalizeFirst();
										}
										else
										{
											label = "PrioritizeGeneric".Translate(workGiver_Scanner2.PostProcessedGerund(job2), "AreaLower".Translate()).CapitalizeFirst();
											Job localJob = job2;
											WorkGiver_Scanner localScanner = workGiver_Scanner2;
											job2.workGiverDef = workGiver_Scanner2.def;
											action2 = delegate()
											{
												if (pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, clickCell))
												{
													if (workGiver.forceMote != null)
													{
														MoteMaker.MakeStaticMote(clickCell, pawn.Map, workGiver.forceMote, 1f);
													}
													if (workGiver.forceFleck != null)
													{
														FleckMaker.Static(clickCell, pawn.Map, workGiver.forceFleck, 1f);
													}
												}
											};
										}
									}
								}
								if (!opts.Any((FloatMenuOption op) => op.Label == label.TrimEnd(Array.Empty<char>())))
								{
									FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action2, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, clickCell, "ReservedBy");
									if (drafted && workGiver.autoTakeablePriorityDrafted != -1)
									{
										floatMenuOption.autoTakeable = true;
										floatMenuOption.autoTakeablePriority = (float)workGiver.autoTakeablePriorityDrafted;
									}
									opts.Add(floatMenuOption);
								}
							}
						}
					}
					IL_D6A:;
				}
			}
		}

		// Token: 0x060076CB RID: 30411 RVA: 0x0029A93C File Offset: 0x00298B3C
		private static FloatMenuOption GotoLocationOption(IntVec3 clickCell, Pawn pawn, bool suppressAutoTakeableGoto)
		{
			if (suppressAutoTakeableGoto)
			{
				return null;
			}
			IntVec3 curLoc = CellFinder.StandableCellNear(clickCell, pawn.Map, 2.9f);
			if (!curLoc.IsValid || !(curLoc != pawn.Position))
			{
				return null;
			}
			if (!pawn.CanReach(curLoc, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotGoNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Action action = delegate()
			{
				FloatMenuMakerMap.PawnGotoAction(clickCell, pawn, RCellFinder.BestOrderedGotoDestNear(curLoc, pawn, null));
			};
			return new FloatMenuOption("GoHere".Translate(), action, MenuOptionPriority.GoHere, null, null, 0f, null, null, true, 0)
			{
				autoTakeable = true,
				autoTakeablePriority = 10f
			};
		}

		// Token: 0x060076CC RID: 30412 RVA: 0x0029AA30 File Offset: 0x00298C30
		public static void PawnGotoAction(IntVec3 clickCell, Pawn pawn, IntVec3 gotoLoc)
		{
			bool flag;
			if (pawn.Position == gotoLoc || (pawn.CurJobDef == JobDefOf.Goto && pawn.CurJob.targetA.Cell == gotoLoc))
			{
				flag = true;
			}
			else
			{
				Job job = JobMaker.MakeJob(JobDefOf.Goto, gotoLoc);
				if (pawn.Map.exitMapGrid.IsExitCell(clickCell))
				{
					job.exitMapOnArrival = true;
				}
				else if (!pawn.Map.IsPlayerHome && !pawn.Map.exitMapGrid.MapUsesExitGrid && CellRect.WholeMap(pawn.Map).IsOnEdge(clickCell, 3) && pawn.Map.Parent.GetComponent<FormCaravanComp>() != null && MessagesRepeatAvoider.MessageShowAllowed("MessagePlayerTriedToLeaveMapViaExitGrid-" + pawn.Map.uniqueID, 60f))
				{
					if (pawn.Map.Parent.GetComponent<FormCaravanComp>().CanFormOrReformCaravanNow)
					{
						Messages.Message("MessagePlayerTriedToLeaveMapViaExitGrid_CanReform".Translate(), pawn.Map.Parent, MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						Messages.Message("MessagePlayerTriedToLeaveMapViaExitGrid_CantReform".Translate(), pawn.Map.Parent, MessageTypeDefOf.RejectInput, false);
					}
				}
				flag = pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}
			if (flag)
			{
				FleckMaker.Static(gotoLoc, pawn.Map, FleckDefOf.FeedbackGoto, 1f);
			}
		}

		// Token: 0x040041FB RID: 16891
		public static Pawn makingFor;

		// Token: 0x040041FC RID: 16892
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040041FD RID: 16893
		private static FloatMenuOption[] equivalenceGroupTempStorage;
	}
}
