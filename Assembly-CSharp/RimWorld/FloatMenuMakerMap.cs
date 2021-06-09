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
	// Token: 0x02001AB0 RID: 6832
	public static class FloatMenuMakerMap
	{
		// Token: 0x060096F3 RID: 38643 RVA: 0x00064CC0 File Offset: 0x00062EC0
		private static bool CanTakeOrder(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}

		// Token: 0x060096F4 RID: 38644 RVA: 0x002C0780 File Offset: 0x002BE980
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
			List<FloatMenuOption> list = FloatMenuMakerMap.ChoicesAtFor(UI.MouseMapPosition(), pawn);
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

		// Token: 0x060096F5 RID: 38645 RVA: 0x002C0884 File Offset: 0x002BEA84
		public static bool TryMakeMultiSelectFloatMenu(List<Pawn> pawns)
		{
			FloatMenuMakerMap.tmpPawns.Clear();
			FloatMenuMakerMap.tmpPawns.AddRange(pawns);
			FloatMenuMakerMap.tmpPawns.RemoveAll((Pawn x) => !FloatMenuMakerMap.CanTakeOrder(x) || x.Downed || x.Map != Find.CurrentMap);
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

		// Token: 0x060096F6 RID: 38646 RVA: 0x002C0928 File Offset: 0x002BEB28
		public static List<FloatMenuOption> ChoicesAtFor(Vector3 clickPos, Pawn pawn)
		{
			IntVec3 intVec = IntVec3.FromVector3(clickPos);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (!intVec.InBounds(pawn.Map) || !FloatMenuMakerMap.CanTakeOrder(pawn))
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
						FloatMenuOption floatMenuOption = FloatMenuMakerMap.GotoLocationOption(intVec, pawn);
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
						FloatMenuMakerMap.AddDraftedOrders(clickPos, pawn, list);
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

		// Token: 0x060096F7 RID: 38647 RVA: 0x002C0A34 File Offset: 0x002BEC34
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

		// Token: 0x060096F8 RID: 38648 RVA: 0x002C0AF0 File Offset: 0x002BECF0
		private static void AddDraftedOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			IntVec3 clickCell = IntVec3.FromVector3(clickPos);
			foreach (LocalTargetInfo attackTarg in GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForAttackHostile(), true, null))
			{
				FloatMenuMakerMap.<>c__DisplayClass7_0 CS$<>8__locals1 = new FloatMenuMakerMap.<>c__DisplayClass7_0();
				CS$<>8__locals1.attackTarg = attackTarg;
				if (pawn.equipment.Primary != null && !pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
				{
					string str;
					Action rangedAct = FloatMenuUtility.GetRangedAttackAction(pawn, CS$<>8__locals1.attackTarg, out str);
					string text = "FireAt".Translate(CS$<>8__locals1.attackTarg.Thing.Label, CS$<>8__locals1.attackTarg.Thing);
					FloatMenuOption floatMenuOption = new FloatMenuOption("", null, MenuOptionPriority.High, null, attackTarg.Thing, 0f, null, null);
					if (rangedAct == null)
					{
						text = text + ": " + str;
					}
					else
					{
						floatMenuOption.autoTakeable = (!CS$<>8__locals1.attackTarg.HasThing || CS$<>8__locals1.attackTarg.Thing.HostileTo(Faction.OfPlayer));
						floatMenuOption.autoTakeablePriority = 40f;
						floatMenuOption.action = delegate()
						{
							MoteMaker.MakeStaticMote(CS$<>8__locals1.attackTarg.Thing.DrawPos, CS$<>8__locals1.attackTarg.Thing.Map, ThingDefOf.Mote_FeedbackShoot, 1f);
							rangedAct();
						};
					}
					floatMenuOption.Label = text;
					opts.Add(floatMenuOption);
				}
				string str2;
				Action meleeAct = FloatMenuUtility.GetMeleeAttackAction(pawn, CS$<>8__locals1.attackTarg, out str2);
				Pawn pawn2 = CS$<>8__locals1.attackTarg.Thing as Pawn;
				string text2;
				if (pawn2 != null && pawn2.Downed)
				{
					text2 = "MeleeAttackToDeath".Translate(CS$<>8__locals1.attackTarg.Thing.Label, CS$<>8__locals1.attackTarg.Thing);
				}
				else
				{
					text2 = "MeleeAttack".Translate(CS$<>8__locals1.attackTarg.Thing.Label, CS$<>8__locals1.attackTarg.Thing);
				}
				MenuOptionPriority priority = (CS$<>8__locals1.attackTarg.HasThing && pawn.HostileTo(CS$<>8__locals1.attackTarg.Thing)) ? MenuOptionPriority.AttackEnemy : MenuOptionPriority.VeryLow;
				FloatMenuOption floatMenuOption2 = new FloatMenuOption("", null, priority, null, CS$<>8__locals1.attackTarg.Thing, 0f, null, null);
				if (meleeAct == null)
				{
					text2 = text2 + ": " + str2.CapitalizeFirst();
				}
				else
				{
					floatMenuOption2.autoTakeable = (!CS$<>8__locals1.attackTarg.HasThing || CS$<>8__locals1.attackTarg.Thing.HostileTo(Faction.OfPlayer));
					floatMenuOption2.autoTakeablePriority = 30f;
					floatMenuOption2.action = delegate()
					{
						MoteMaker.MakeStaticMote(CS$<>8__locals1.attackTarg.Thing.DrawPos, CS$<>8__locals1.attackTarg.Thing.Map, ThingDefOf.Mote_FeedbackMelee, 1f);
						meleeAct();
					};
				}
				floatMenuOption2.Label = text2;
				opts.Add(floatMenuOption2);
			}
			FloatMenuMakerMap.AddJobGiverWorkOrders_NewTmp(clickPos, pawn, opts, true);
			FloatMenuOption floatMenuOption3 = FloatMenuMakerMap.GotoLocationOption(clickCell, pawn);
			if (floatMenuOption3 != null)
			{
				opts.Add(floatMenuOption3);
			}
		}

		// Token: 0x060096F9 RID: 38649 RVA: 0x002C0E78 File Offset: 0x002BF078
		private static void AddHumanlikeOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			IntVec3 c = IntVec3.FromVector3(clickPos);
			using (List<Thing>.Enumerator enumerator = c.GetThingList(pawn.Map).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn2;
					if ((pawn2 = (enumerator.Current as Pawn)) != null)
					{
						Lord lord = pawn2.GetLord();
						if (lord != null && lord.CurLordToil != null)
						{
							IEnumerable<FloatMenuOption> enumerable = lord.CurLordToil.ExtraFloatMenuOptions(pawn2, pawn);
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
			if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				foreach (LocalTargetInfo dest in GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForArrest(pawn), true, null))
				{
					bool flag = dest.HasThing && dest.Thing is Pawn && ((Pawn)dest.Thing).IsWildMan();
					if (pawn.Drafted || flag)
					{
						if (dest.Thing is Pawn && (pawn.InSameExtraFaction((Pawn)dest.Thing, ExtraFactionType.HomeFaction, null) || pawn.InSameExtraFaction((Pawn)dest.Thing, ExtraFactionType.MiniFaction, null)))
						{
							opts.Add(new FloatMenuOption("CannotArrest".Translate() + ": " + "SameFaction".Translate((Pawn)dest.Thing), null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else if (!pawn.CanReach(dest, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotArrest".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else
						{
							Pawn pTarg = (Pawn)dest.Thing;
							Action action = delegate()
							{
								Building_Bed building_Bed = RestUtility.FindBedFor(pTarg, pawn, true, false, false);
								if (building_Bed == null)
								{
									building_Bed = RestUtility.FindBedFor(pTarg, pawn, true, false, true);
								}
								if (building_Bed == null)
								{
									Messages.Message("CannotArrest".Translate() + ": " + "NoPrisonerBed".Translate(), pTarg, MessageTypeDefOf.RejectInput, false);
									return;
								}
								Job job = JobMaker.MakeJob(JobDefOf.Arrest, pTarg, building_Bed);
								job.count = 1;
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								if (pTarg.Faction != null && ((pTarg.Faction != Faction.OfPlayer && !pTarg.Faction.Hidden) || pTarg.IsQuestLodger()))
								{
									TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.ArrestingCreatesEnemies, new string[]
									{
										pTarg.GetAcceptArrestChance(pawn).ToStringPercent()
									});
								}
							};
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TryToArrest".Translate(dest.Thing.LabelCap, dest.Thing, pTarg.GetAcceptArrestChance(pawn).ToStringPercent()), action, MenuOptionPriority.High, null, dest.Thing, 0f, null, null), pawn, pTarg, "ReservedBy"));
						}
					}
				}
			}
			foreach (Thing t4 in c.GetThingList(pawn.Map))
			{
				Thing t = t4;
				if (t.def.ingestible != null && pawn.RaceProps.CanEverEat(t) && t.IngestibleNow)
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
					if (!t.IsSociallyProper(pawn))
					{
						text = text + ": " + "ReservedForPrisoners".Translate().CapitalizeFirst();
					}
					FloatMenuOption floatMenuOption;
					if (t.def.IsNonMedicalDrug && pawn.IsTeetotaler())
					{
						floatMenuOption = new FloatMenuOption(text + ": " + TraitDefOf.DrugDesire.DataAtDegree(-1).GetLabelCapFor(pawn), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (FoodUtility.InappropriateForTitle(t.def, pawn, true))
					{
						floatMenuOption = new FloatMenuOption(text + ": " + "FoodBelowTitleRequirements".Translate(pawn.royalty.MostSeniorTitle.def.GetLabelFor(pawn)), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						floatMenuOption = new FloatMenuOption(text + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						MenuOptionPriority priority = (t is Corpse) ? MenuOptionPriority.Low : MenuOptionPriority.Default;
						bool maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(t, pawn, FoodUtility.WillIngestStackCountOf(pawn, t.def, t.GetStatValue(StatDefOf.Nutrition, true))) != 0;
						floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
						{
							int maxAmountToPickup2 = FoodUtility.GetMaxAmountToPickup(t, pawn, FoodUtility.WillIngestStackCountOf(pawn, t.def, t.GetStatValue(StatDefOf.Nutrition, true)));
							if (maxAmountToPickup2 == 0)
							{
								return;
							}
							t.SetForbidden(false, true);
							Job job = JobMaker.MakeJob(JobDefOf.Ingest, t);
							job.count = maxAmountToPickup2;
							pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						}, priority, null, null, 0f, null, null), pawn, t, "ReservedBy");
						if (!maxAmountToPickup)
						{
							floatMenuOption.action = null;
						}
					}
					opts.Add(floatMenuOption);
				}
			}
			foreach (LocalTargetInfo dest2 in GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForQuestPawnsWhoWillJoinColony(pawn), true, null))
			{
				Pawn toHelpPawn = (Pawn)dest2.Thing;
				FloatMenuOption item2;
				if (!pawn.CanReach(dest2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					item2 = new FloatMenuOption("CannotGoNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					item2 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(toHelpPawn.IsPrisoner ? "FreePrisoner".Translate() : "OfferHelp".Translate(), delegate()
					{
						pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.OfferHelp, toHelpPawn), JobTag.Misc);
					}, MenuOptionPriority.RescueOrCapture, null, toHelpPawn, 0f, null, null), pawn, toHelpPawn, "ReservedBy");
				}
				opts.Add(item2);
			}
			if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				foreach (Thing thing3 in c.GetThingList(pawn.Map))
				{
					Corpse corpse = thing3 as Corpse;
					if (corpse != null && corpse.IsInValidStorage())
					{
						StoragePriority priority2 = StoreUtility.CurrentHaulDestinationOf(corpse).GetStoreSettings().Priority;
						IHaulDestination haulDestination;
						if (StoreUtility.TryFindBestBetterNonSlotGroupStorageFor(corpse, pawn, pawn.Map, priority2, Faction.OfPlayer, out haulDestination, true) && haulDestination.GetStoreSettings().Priority == priority2 && haulDestination is Building_Grave)
						{
							Building_Grave grave = haulDestination as Building_Grave;
							string label = "PrioritizeGeneric".Translate("Burying".Translate(), corpse.Label);
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, delegate()
							{
								pawn.jobs.TryTakeOrderedJob(HaulAIUtility.HaulToContainerJob(pawn, corpse, grave), JobTag.Misc);
							}, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, new LocalTargetInfo(corpse), "ReservedBy"));
						}
					}
				}
				foreach (LocalTargetInfo localTargetInfo in GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForRescue(pawn), true, null))
				{
					Pawn victim = (Pawn)localTargetInfo.Thing;
					if (!victim.InBed() && pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && !victim.mindState.WillJoinColonyIfRescued)
					{
						if (!victim.IsPrisonerOfColony && (!victim.InMentalState || victim.health.hediffSet.HasHediff(HediffDefOf.Scaria, false)) && (victim.Faction == Faction.OfPlayer || victim.Faction == null || !victim.Faction.HostileTo(Faction.OfPlayer)))
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Rescue".Translate(victim.LabelCap, victim), delegate()
							{
								Building_Bed building_Bed = RestUtility.FindBedFor(victim, pawn, false, false, false);
								if (building_Bed == null)
								{
									building_Bed = RestUtility.FindBedFor(victim, pawn, false, false, true);
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
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
							}, MenuOptionPriority.RescueOrCapture, null, victim, 0f, null, null), pawn, victim, "ReservedBy"));
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
								Building_Bed building_Bed = RestUtility.FindBedFor(victim, pawn, true, false, false);
								if (building_Bed == null)
								{
									building_Bed = RestUtility.FindBedFor(victim, pawn, true, false, true);
								}
								if (building_Bed == null)
								{
									Messages.Message("CannotCapture".Translate() + ": " + "NoPrisonerBed".Translate(), victim, MessageTypeDefOf.RejectInput, false);
									return;
								}
								Job job = JobMaker.MakeJob(JobDefOf.Capture, victim, building_Bed);
								job.count = 1;
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Capturing, KnowledgeAmount.Total);
								if (victim.Faction != null && victim.Faction != Faction.OfPlayer && !victim.Faction.Hidden && !victim.Faction.HostileTo(Faction.OfPlayer) && !victim.IsPrisonerOfColony)
								{
									Messages.Message("MessageCapturingWillAngerFaction".Translate(victim.Named("PAWN")).AdjustedFor(victim, "PAWN", true), victim, MessageTypeDefOf.CautionInput, false);
								}
							}, MenuOptionPriority.RescueOrCapture, null, victim, 0f, null, null), pawn, victim, "ReservedBy"));
						}
					}
				}
				foreach (LocalTargetInfo localTargetInfo2 in GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForRescue(pawn), true, null))
				{
					LocalTargetInfo localTargetInfo3 = localTargetInfo2;
					Pawn victim = (Pawn)localTargetInfo3.Thing;
					if (victim.Downed && pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, pawn, true) != null)
					{
						string text2 = "CarryToCryptosleepCasket".Translate(localTargetInfo3.Thing.LabelCap, localTargetInfo3.Thing);
						JobDef jDef = JobDefOf.CarryToCryptosleepCasket;
						Action action2 = delegate()
						{
							Building_CryptosleepCasket building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, pawn, false);
							if (building_CryptosleepCasket == null)
							{
								building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, pawn, true);
							}
							if (building_CryptosleepCasket == null)
							{
								Messages.Message("CannotCarryToCryptosleepCasket".Translate() + ": " + "NoCryptosleepCasket".Translate(), victim, MessageTypeDefOf.RejectInput, false);
								return;
							}
							Job job = JobMaker.MakeJob(jDef, victim, building_CryptosleepCasket);
							job.count = 1;
							pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						};
						if (victim.IsQuestLodger())
						{
							text2 += " (" + "CryptosleepCasketGuestsNotAllowed".Translate() + ")";
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text2, null, MenuOptionPriority.Default, null, victim, 0f, null, null), pawn, victim, "ReservedBy"));
						}
						else if (victim.GetExtraHostFaction(null) != null)
						{
							text2 += " (" + "CryptosleepCasketGuestPrisonersNotAllowed".Translate() + ")";
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text2, null, MenuOptionPriority.Default, null, victim, 0f, null, null), pawn, victim, "ReservedBy"));
						}
						else
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text2, action2, MenuOptionPriority.Default, null, victim, 0f, null, null), pawn, victim, "ReservedBy"));
						}
					}
				}
				if (ModsConfig.RoyaltyActive)
				{
					foreach (LocalTargetInfo localTargetInfo4 in GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForShuttle(pawn), true, null))
					{
						LocalTargetInfo localTargetInfo5 = localTargetInfo4;
						Pawn victim = (Pawn)localTargetInfo5.Thing;
						Predicate<Thing> validator = delegate(Thing thing)
						{
							CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
							return compShuttle != null && compShuttle.IsAllowedNow(victim);
						};
						Thing shuttleThing = GenClosest.ClosestThingReachable(victim.Position, victim.Map, ThingRequest.ForDef(ThingDefOf.Shuttle), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
						if (shuttleThing != null && pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && !pawn.WorkTypeIsDisabled(WorkTypeDefOf.Hauling))
						{
							string label2 = "CarryToShuttle".Translate(localTargetInfo5.Thing);
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
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							};
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label2, action3, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, victim, "ReservedBy"));
						}
					}
				}
			}
			foreach (LocalTargetInfo stripTarg2 in GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForStrip(pawn), true, null))
			{
				LocalTargetInfo stripTarg = stripTarg2;
				FloatMenuOption item3;
				if (!pawn.CanReach(stripTarg, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					item3 = new FloatMenuOption("CannotStrip".Translate(stripTarg.Thing.LabelCap, stripTarg.Thing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else if (stripTarg.Pawn != null && stripTarg.Pawn.HasExtraHomeFaction(null))
				{
					item3 = new FloatMenuOption("CannotStrip".Translate(stripTarg.Thing.LabelCap, stripTarg.Thing) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					item3 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Strip".Translate(stripTarg.Thing.LabelCap, stripTarg.Thing), delegate()
					{
						stripTarg.Thing.SetForbidden(false, false);
						pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Strip, stripTarg), JobTag.Misc);
						StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(stripTarg.Thing);
					}, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, stripTarg, "ReservedBy");
				}
				opts.Add(item3);
			}
			if (pawn.equipment != null)
			{
				ThingWithComps equipment = null;
				List<Thing> thingList = c.GetThingList(pawn.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].TryGetComp<CompEquippable>() != null)
					{
						equipment = (ThingWithComps)thingList[i];
						break;
					}
				}
				if (equipment != null)
				{
					string labelShort = equipment.LabelShort;
					FloatMenuOption item4;
					string str;
					if (equipment.def.IsWeapon && pawn.WorkTagIsDisabled(WorkTags.Violent))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfViolenceLower".Translate(pawn.LabelShort, pawn), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!pawn.CanReach(equipment, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "Incapable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (equipment.IsBurning())
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "BurningLower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (pawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanEquip(equipment, pawn))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!EquipmentUtility.CanEquip_NewTmp(equipment, pawn, out str, false))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + str.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						string text3 = "Equip".Translate(labelShort);
						if (equipment.def.IsRangedWeapon && pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.Brawler))
						{
							text3 += " " + "EquipWarningBrawler".Translate();
						}
						if (EquipmentUtility.AlreadyBondedToWeapon(equipment, pawn))
						{
							text3 += " " + "BladelinkAlreadyBonded".Translate();
							TaggedString dialogText = "BladelinkAlreadyBondedDialog".Translate(pawn.Named("PAWN"), equipment.Named("WEAPON"), pawn.equipment.bondedWeapon.Named("BONDEDWEAPON"));
							item4 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text3, delegate()
							{
								Find.WindowStack.Add(new Dialog_MessageBox(dialogText, null, null, null, null, null, false, null, null));
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, equipment, "ReservedBy");
						}
						else
						{
							Action <>9__13;
							item4 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text3, delegate()
							{
								string personaWeaponConfirmationText = EquipmentUtility.GetPersonaWeaponConfirmationText(equipment, pawn);
								if (!personaWeaponConfirmationText.NullOrEmpty())
								{
									WindowStack windowStack = Find.WindowStack;
									TaggedString text5 = personaWeaponConfirmationText;
									string buttonAText = "Yes".Translate();
									Action buttonAAction;
									if ((buttonAAction = <>9__13) == null)
									{
										buttonAAction = (<>9__13 = delegate()
										{
											base.<AddHumanlikeOrders>g__Equip|12();
										});
									}
									windowStack.Add(new Dialog_MessageBox(text5, buttonAText, buttonAAction, "No".Translate(), null, null, false, null, null));
									return;
								}
								base.<AddHumanlikeOrders>g__Equip|12();
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, equipment, "ReservedBy");
						}
					}
					opts.Add(item4);
				}
			}
			foreach (Pair<CompReloadable, Thing> pair in ReloadableUtility.FindPotentiallyReloadableGear(pawn, c.GetThingList(pawn.Map)))
			{
				CompReloadable comp = pair.First;
				Thing second = pair.Second;
				string text4 = "Reload".Translate(comp.parent.Named("GEAR"), comp.AmmoDef.Named("AMMO")) + " (" + comp.LabelRemaining + ")";
				List<Thing> chosenAmmo;
				if (!pawn.CanReach(second, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					opts.Add(new FloatMenuOption(text4 + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else if (!comp.NeedsReload(true))
				{
					opts.Add(new FloatMenuOption(text4 + ": " + "ReloadFull".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else if ((chosenAmmo = ReloadableUtility.FindEnoughAmmo(pawn, second.Position, comp, true)) == null)
				{
					opts.Add(new FloatMenuOption(text4 + ": " + "ReloadNotEnough".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else
				{
					Action action4 = delegate()
					{
						pawn.jobs.TryTakeOrderedJob(JobGiver_Reload.MakeReloadJob(comp, chosenAmmo), JobTag.Misc);
					};
					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text4, action4, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, second, "ReservedBy"));
				}
			}
			if (pawn.apparel != null)
			{
				Apparel apparel = pawn.Map.thingGrid.ThingAt<Apparel>(c);
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
					if (!pawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						item5 = new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (apparel.IsBurning())
					{
						item5 = new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (pawn.apparel.WouldReplaceLockedApparel(apparel))
					{
						item5 = new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!ApparelUtility.HasPartsToWear(pawn, apparel.def))
					{
						item5 = new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!EquipmentUtility.CanEquip_NewTmp(apparel, pawn, out t2, true))
					{
						item5 = new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + t2, null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						item5 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(key2.Translate(apparel.LabelShort, apparel), delegate()
						{
							apparel.SetForbidden(false, true);
							Job job = JobMaker.MakeJob(JobDefOf.Wear, apparel);
							pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, apparel, "ReservedBy");
					}
					opts.Add(item5);
				}
			}
			if (pawn.IsFormingCaravan())
			{
				Thing item = c.GetFirstItem(pawn.Map);
				if (item != null && item.def.EverHaulable && item.def.canLoadIntoCaravan)
				{
					Pawn packTarget = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(pawn) ?? pawn;
					JobDef jobDef = (packTarget == pawn) ? JobDefOf.TakeInventory : JobDefOf.GiveToPackAnimal;
					if (!pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotLoadIntoCaravan".Translate(item.Label, item) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else if (MassUtility.WillBeOverEncumberedAfterPickingUp(packTarget, item, 1))
					{
						opts.Add(new FloatMenuOption("CannotLoadIntoCaravan".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else
					{
						LordJob_FormAndSendCaravan lordJob = (LordJob_FormAndSendCaravan)pawn.GetLord().LordJob;
						float capacityLeft = CaravanFormingUtility.CapacityLeft(lordJob);
						if (item.stackCount == 1)
						{
							float capacityLeft4 = capacityLeft - item.GetStatValue(StatDefOf.Mass, true);
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravan".Translate(item.Label, item), capacityLeft4), delegate()
							{
								item.SetForbidden(false, false);
								Job job = JobMaker.MakeJob(jobDef, item);
								job.count = 1;
								job.checkEncumbrance = (packTarget == pawn);
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
						else
						{
							if (MassUtility.WillBeOverEncumberedAfterPickingUp(packTarget, item, item.stackCount))
							{
								opts.Add(new FloatMenuOption("CannotLoadIntoCaravanAll".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
							}
							else
							{
								float capacityLeft2 = capacityLeft - (float)item.stackCount * item.GetStatValue(StatDefOf.Mass, true);
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravanAll".Translate(item.Label, item), capacityLeft2), delegate()
								{
									item.SetForbidden(false, false);
									Job job = JobMaker.MakeJob(jobDef, item);
									job.count = item.stackCount;
									job.checkEncumbrance = (packTarget == pawn);
									pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
							}
							Action<int> <>9__20;
							Func<int, string> <>9__19;
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("LoadIntoCaravanSome".Translate(item.LabelNoCount, item), delegate()
							{
								int num = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(packTarget, item), item.stackCount);
								Func<int, string> textGetter;
								if ((textGetter = <>9__19) == null)
								{
									textGetter = (<>9__19 = delegate(int val)
									{
										float capacityLeft3 = capacityLeft - (float)val * item.GetStatValue(StatDefOf.Mass, true);
										return CaravanFormingUtility.AppendOverweightInfo(string.Format("LoadIntoCaravanCount".Translate(item.LabelNoCount, item), val), capacityLeft3);
									});
								}
								int from = 1;
								int to = num;
								Action<int> confirmAction;
								if ((confirmAction = <>9__20) == null)
								{
									confirmAction = (<>9__20 = delegate(int count)
									{
										item.SetForbidden(false, false);
										Job job = JobMaker.MakeJob(jobDef, item);
										job.count = count;
										job.checkEncumbrance = (packTarget == pawn);
										pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
									});
								}
								Dialog_Slider window = new Dialog_Slider(textGetter, from, to, confirmAction, int.MinValue);
								Find.WindowStack.Add(window);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
					}
				}
			}
			if (!pawn.Map.IsPlayerHome && !pawn.IsFormingCaravan())
			{
				Thing item = c.GetFirstItem(pawn.Map);
				if (item != null && item.def.EverHaulable)
				{
					if (!pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotPickUp".Translate(item.Label, item) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else if (MassUtility.WillBeOverEncumberedAfterPickingUp(pawn, item, 1))
					{
						opts.Add(new FloatMenuOption("CannotPickUp".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else if (item.stackCount == 1)
					{
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUp".Translate(item.Label, item), delegate()
						{
							item.SetForbidden(false, false);
							Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, item);
							job.count = 1;
							job.checkEncumbrance = true;
							pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
					}
					else
					{
						if (MassUtility.WillBeOverEncumberedAfterPickingUp(pawn, item, item.stackCount))
						{
							opts.Add(new FloatMenuOption("CannotPickUpAll".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpAll".Translate(item.Label, item), delegate()
							{
								item.SetForbidden(false, false);
								Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, item);
								job.count = item.stackCount;
								job.checkEncumbrance = true;
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
						Action<int> <>9__24;
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpSome".Translate(item.LabelNoCount, item), delegate()
						{
							int num = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(pawn, item), item.stackCount);
							string text5 = "PickUpCount".Translate(item.LabelNoCount, item);
							int from = 1;
							int to = num;
							Action<int> confirmAction;
							if ((confirmAction = <>9__24) == null)
							{
								confirmAction = (<>9__24 = delegate(int count)
								{
									item.SetForbidden(false, false);
									Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, item);
									job.count = count;
									job.checkEncumbrance = true;
									pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								});
							}
							Dialog_Slider window = new Dialog_Slider(text5, from, to, confirmAction, int.MinValue);
							Find.WindowStack.Add(window);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
					}
				}
			}
			if (!pawn.Map.IsPlayerHome && !pawn.IsFormingCaravan())
			{
				Thing item = c.GetFirstItem(pawn.Map);
				if (item != null && item.def.EverHaulable)
				{
					Pawn bestPackAnimal = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(pawn);
					if (bestPackAnimal != null)
					{
						if (!pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotGiveToPackAnimal".Translate(item.Label, item) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, item, 1))
						{
							opts.Add(new FloatMenuOption("CannotGiveToPackAnimal".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else if (item.stackCount == 1)
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimal".Translate(item.Label, item), delegate()
							{
								item.SetForbidden(false, false);
								Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, item);
								job.count = 1;
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
						else
						{
							if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, item, item.stackCount))
							{
								opts.Add(new FloatMenuOption("CannotGiveToPackAnimalAll".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
							}
							else
							{
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalAll".Translate(item.Label, item), delegate()
								{
									item.SetForbidden(false, false);
									Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, item);
									job.count = item.stackCount;
									pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
							}
							Action<int> <>9__28;
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalSome".Translate(item.LabelNoCount, item), delegate()
							{
								int num = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(bestPackAnimal, item), item.stackCount);
								string text5 = "GiveToPackAnimalCount".Translate(item.LabelNoCount, item);
								int from = 1;
								int to = num;
								Action<int> confirmAction;
								if ((confirmAction = <>9__28) == null)
								{
									confirmAction = (<>9__28 = delegate(int count)
									{
										item.SetForbidden(false, false);
										Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, item);
										job.count = count;
										pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
									});
								}
								Dialog_Slider window = new Dialog_Slider(text5, from, to, confirmAction, int.MinValue);
								Find.WindowStack.Add(window);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
					}
				}
			}
			if (!pawn.Map.IsPlayerHome && pawn.Map.exitMapGrid.MapUsesExitGrid)
			{
				foreach (LocalTargetInfo target in GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForRescue(pawn), true, null))
				{
					Pawn p = (Pawn)target.Thing;
					if (p.Faction == Faction.OfPlayer || p.IsPrisonerOfColony || CaravanUtility.ShouldAutoCapture(p, Faction.OfPlayer))
					{
						if (!pawn.CanReach(p, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotCarryToExit".Translate(p.Label, p) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else
						{
							IntVec3 exitSpot;
							if (!RCellFinder.TryFindBestExitSpot(pawn, out exitSpot, TraverseMode.ByPawn))
							{
								opts.Add(new FloatMenuOption("CannotCarryToExit".Translate(p.Label, p) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
							}
							else
							{
								TaggedString taggedString2 = (p.Faction == Faction.OfPlayer || p.IsPrisonerOfColony) ? "CarryToExit".Translate(p.Label, p) : "CarryToExitAndCapture".Translate(p.Label, p);
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString2, delegate()
								{
									Job job = JobMaker.MakeJob(JobDefOf.CarryDownedPawnToExit, p, exitSpot);
									job.count = 1;
									job.failIfCantJoinOrCreateCaravan = true;
									pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, target, "ReservedBy"));
							}
						}
					}
				}
			}
			if (pawn.equipment != null && pawn.equipment.Primary != null && GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForSelf(pawn), true, null).Any<LocalTargetInfo>())
			{
				if (pawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanUnequip(pawn.equipment.Primary, pawn))
				{
					opts.Add(new FloatMenuOption("CannotDrop".Translate(pawn.equipment.Primary.Label, pawn.equipment.Primary) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else
				{
					Action action5 = delegate()
					{
						pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DropEquipment, pawn.equipment.Primary), JobTag.Misc);
					};
					opts.Add(new FloatMenuOption("Drop".Translate(pawn.equipment.Primary.Label, pawn.equipment.Primary), action5, MenuOptionPriority.Default, null, pawn, 0f, null, null));
				}
			}
			foreach (LocalTargetInfo dest3 in GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForTrade(), true, null))
			{
				if (!pawn.CanReach(dest3, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					opts.Add(new FloatMenuOption("CannotTrade".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else if (pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
				{
					opts.Add(new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(SkillDefOf.Social.LabelCap), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else if (!pawn.CanTradeWith(((Pawn)dest3.Thing).Faction, ((Pawn)dest3.Thing).TraderKind))
				{
					opts.Add(new FloatMenuOption("CannotTradeMissingTitleAbility".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else
				{
					Pawn pTarg = (Pawn)dest3.Thing;
					Action action6 = delegate()
					{
						Job job = JobMaker.MakeJob(JobDefOf.TradeWithPawn, pTarg);
						job.playerForced = true;
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InteractingWithTraders, KnowledgeAmount.Total);
					};
					string t3 = "";
					if (pTarg.Faction != null)
					{
						t3 = " (" + pTarg.Faction.Name + ")";
					}
					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TradeWith".Translate(pTarg.LabelShort + ", " + pTarg.TraderKind.label) + t3, action6, MenuOptionPriority.InitiateSocial, null, dest3.Thing, 0f, null, null), pawn, pTarg, "ReservedBy"));
				}
			}
			using (IEnumerator<LocalTargetInfo> enumerator3 = GenUI.TargetsAt_NewTemp(clickPos, TargetingParameters.ForOpen(pawn), true, null).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					LocalTargetInfo casket = enumerator3.Current;
					if (!pawn.CanReach(casket, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotOpen".Translate(casket.Thing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						opts.Add(new FloatMenuOption("CannotOpen".Translate(casket.Thing) + ": " + "Incapable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else if (casket.Thing.Map.designationManager.DesignationOn(casket.Thing, DesignationDefOf.Open) == null)
					{
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Open".Translate(casket.Thing), delegate()
						{
							Job job = JobMaker.MakeJob(JobDefOf.Open, casket.Thing);
							job.ignoreDesignations = true;
							pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, casket.Thing, "ReservedBy"));
					}
				}
			}
			foreach (Thing thing2 in pawn.Map.thingGrid.ThingsAt(c))
			{
				foreach (FloatMenuOption item6 in thing2.GetFloatMenuOptions(pawn))
				{
					opts.Add(item6);
				}
			}
		}

		// Token: 0x060096FA RID: 38650 RVA: 0x002C4218 File Offset: 0x002C2418
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
				if (pawn.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					flag = true;
					break;
				}
			}
			if (flag2 && !flag)
			{
				return;
			}
			FloatMenuMakerMap.AddJobGiverWorkOrders_NewTmp(clickPos, pawn, opts, false);
		}

		// Token: 0x060096FB RID: 38651 RVA: 0x002C42C4 File Offset: 0x002C24C4
		private static void AddJobGiverWorkOrders_NewTmp(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts, bool drafted)
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
			}))
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
													goto IL_703;
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
											else if (!pawn.CanReach(thing, workGiver_Scanner.PathEndMode, Danger.Deadly, false, TraverseMode.ByPawn))
											{
												text = (thing.Label + ": " + "NoPath".Translate().CapitalizeFirst()).CapitalizeFirst();
											}
											else
											{
												text = "PrioritizeGeneric".Translate(workGiver_Scanner.PostProcessedGerund(job), thing.Label);
												Job localJob = job;
												WorkGiver_Scanner localScanner = workGiver_Scanner;
												job.workGiverDef = workGiver_Scanner.def;
												action = delegate()
												{
													if (pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, clickCell) && workGiver.forceMote != null)
													{
														MoteMaker.MakeStaticMote(clickCell, pawn.Map, workGiver.forceMote, 1f);
													}
												};
											}
										}
									}
									if (DebugViewSettings.showFloatMenuWorkGivers)
									{
										text += string.Format(" (from {0})", workGiver.defName);
									}
									FloatMenuOption menuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, thing, "ReservedBy");
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
						IL_703:;
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
												goto IL_D51;
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
										else if (!pawn.CanReach(clickCell, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
										{
											label = "AreaLower".Translate().CapitalizeFirst() + ": " + "NoPath".Translate().CapitalizeFirst();
										}
										else
										{
											label = "PrioritizeGeneric".Translate(workGiver_Scanner2.PostProcessedGerund(job2), "AreaLower".Translate());
											Job localJob = job2;
											WorkGiver_Scanner localScanner = workGiver_Scanner2;
											job2.workGiverDef = workGiver_Scanner2.def;
											action2 = delegate()
											{
												if (pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, clickCell) && workGiver.forceMote != null)
												{
													MoteMaker.MakeStaticMote(clickCell, pawn.Map, workGiver.forceMote, 1f);
												}
											};
										}
									}
								}
								if (!opts.Any((FloatMenuOption op) => op.Label == label.TrimEnd(Array.Empty<char>())))
								{
									FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action2, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, clickCell, "ReservedBy");
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
					IL_D51:;
				}
			}
		}

		// Token: 0x060096FC RID: 38652 RVA: 0x00064CC8 File Offset: 0x00062EC8
		[Obsolete]
		private static void AddJobGiverWorkOrders(IntVec3 clickCell, Pawn pawn, List<FloatMenuOption> opts, bool drafted)
		{
			FloatMenuMakerMap.AddJobGiverWorkOrders_NewTmp(clickCell.ToVector3(), pawn, opts, drafted);
		}

		// Token: 0x060096FD RID: 38653 RVA: 0x002C50A4 File Offset: 0x002C32A4
		private static FloatMenuOption GotoLocationOption(IntVec3 clickCell, Pawn pawn)
		{
			int num = GenRadial.NumCellsInRadius(2.9f);
			int i = 0;
			IntVec3 curLoc;
			Action <>9__0;
			while (i < num)
			{
				curLoc = GenRadial.RadialPattern[i] + clickCell;
				if (curLoc.Standable(pawn.Map))
				{
					if (!(curLoc != pawn.Position))
					{
						return null;
					}
					if (!pawn.CanReach(curLoc, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						return new FloatMenuOption("CannotGoNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					Action action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate()
						{
							IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(curLoc, pawn);
							Job job = JobMaker.MakeJob(JobDefOf.Goto, intVec);
							if (pawn.Map.exitMapGrid.IsExitCell(UI.MouseCell()))
							{
								job.exitMapOnArrival = true;
							}
							else if (!pawn.Map.IsPlayerHome && !pawn.Map.exitMapGrid.MapUsesExitGrid && CellRect.WholeMap(pawn.Map).IsOnEdge(UI.MouseCell(), 3) && pawn.Map.Parent.GetComponent<FormCaravanComp>() != null && MessagesRepeatAvoider.MessageShowAllowed("MessagePlayerTriedToLeaveMapViaExitGrid-" + pawn.Map.uniqueID, 60f))
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
							if (pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc))
							{
								MoteMaker.MakeStaticMote(intVec, pawn.Map, ThingDefOf.Mote_FeedbackGoto, 1f);
							}
						});
					}
					Action action2 = action;
					return new FloatMenuOption("GoHere".Translate(), action2, MenuOptionPriority.GoHere, null, null, 0f, null, null)
					{
						autoTakeable = true,
						autoTakeablePriority = 10f
					};
				}
				else
				{
					i++;
				}
			}
			return null;
		}

		// Token: 0x04006051 RID: 24657
		public static Pawn makingFor;

		// Token: 0x04006052 RID: 24658
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04006053 RID: 24659
		private static FloatMenuOption[] equivalenceGroupTempStorage;
	}
}
