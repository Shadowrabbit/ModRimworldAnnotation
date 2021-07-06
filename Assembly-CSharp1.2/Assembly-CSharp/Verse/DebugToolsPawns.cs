using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005BB RID: 1467
	public static class DebugToolsPawns
	{
		// Token: 0x060024C3 RID: 9411 RVA: 0x001138B4 File Offset: 0x00111AB4
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Resurrect()
		{
			foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap).ToList<Thing>())
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					ResurrectionUtility.Resurrect(corpse.InnerPawn);
				}
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x0001E8FE File Offset: 0x0001CAFE
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageUntilDown(Pawn p)
		{
			HealthUtility.DamageUntilDowned(p, true);
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x0001E907 File Offset: 0x0001CB07
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageLegs(Pawn p)
		{
			HealthUtility.DamageLegsUntilIncapableOfMoving(p, true);
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x0001E910 File Offset: 0x0001CB10
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p);
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x0001E918 File Offset: 0x0001CB18
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CarriedDamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p.carryTracker.CarriedThing as Pawn);
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x00113924 File Offset: 0x00111B24
		[DebugAction("Pawns", "10 damage until dead", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Do10DamageUntilDead()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				for (int i = 0; i < 1000; i++)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
					dinfo.SetIgnoreInstantKillProtection(true);
					thing.TakeDamage(dinfo);
					if (thing.Destroyed)
					{
						string str = "Took " + (i + 1) + " hits";
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							if (pawn.health.ShouldBeDeadFromLethalDamageThreshold())
							{
								str = str + " (reached lethal damage threshold of " + pawn.health.LethalDamageThreshold.ToString("0.#") + ")";
							}
							else if (PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, pawn.RaceProps.body.corePart, false, null) <= 0.0001f)
							{
								str += " (core part hp reached 0)";
							}
							else
							{
								PawnCapacityDef pawnCapacityDef = pawn.health.ShouldBeDeadFromRequiredCapacity();
								if (pawnCapacityDef != null)
								{
									str = str + " (incapable of " + pawnCapacityDef.defName + ")";
								}
							}
						}
						Log.Message(str + ".", false);
						break;
					}
				}
			}
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00113AC8 File Offset: 0x00111CC8
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageHeldPawnToDeath()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.CarriedThing is Pawn)
				{
					HealthUtility.DamageUntilDead((Pawn)pawn.carryTracker.CarriedThing);
				}
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00113B64 File Offset: 0x00111D64
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SurgeryFailMinor(Pawn p)
		{
			BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where !x.def.conceptual
			select x).RandomElement<BodyPartRecord>();
			Log.Message("part is " + bodyPartRecord, false);
			HealthUtility.GiveInjuriesOperationFailureMinor(p, bodyPartRecord);
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00113BC8 File Offset: 0x00111DC8
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SurgeryFailCatastrophic(Pawn p)
		{
			BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where !x.def.conceptual
			select x).RandomElement<BodyPartRecord>();
			Log.Message("part is " + bodyPartRecord, false);
			HealthUtility.GiveInjuriesOperationFailureCatastrophic(p, bodyPartRecord);
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x0001E92F File Offset: 0x0001CB2F
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SurgeryFailRidiculous(Pawn p)
		{
			HealthUtility.GiveInjuriesOperationFailureRidiculous(p);
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x0001E937 File Offset: 0x0001CB37
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RestoreBodyPart(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RestorePart(p)));
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x0001E94E File Offset: 0x0001CB4E
		[DebugAction("Pawns", "Apply damage...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ApplyDamage()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_ApplyDamage()));
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x0001E964 File Offset: 0x0001CB64
		[DebugAction("Pawns", "Add Hediff...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddHediff()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_AddHediff()));
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x0001E97A File Offset: 0x0001CB7A
		[DebugAction("Pawns", "Remove Hediff...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveHediff(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RemoveHediff(p)));
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x00113C2C File Offset: 0x00111E2C
		[DebugAction("Pawns", "Heal random injury (10)", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void HealRandomInjury10(Pawn p)
		{
			Hediff_Injury hediff_Injury;
			if ((from x in p.health.hediffSet.GetHediffs<Hediff_Injury>()
			where x.CanHealNaturally() || x.CanHealFromTending()
			select x).TryRandomElement(out hediff_Injury))
			{
				hediff_Injury.Heal(10f);
			}
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x00113C84 File Offset: 0x00111E84
		[DebugAction("Pawns", "Activate HediffGiver", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ActivateHediffGiver(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (p.RaceProps.hediffGiverSets != null)
			{
				foreach (HediffGiver localHdg2 in p.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef set) => set.hediffGivers))
				{
					HediffGiver localHdg = localHdg2;
					list.Add(new FloatMenuOption(localHdg.hediff.defName, delegate()
					{
						if (localHdg.TryApply(p, null))
						{
							Messages.Message(localHdg.hediff.defName + " applied to " + p.Label, MessageTypeDefOf.NeutralEvent, false);
							return;
						}
						Messages.Message("failed to apply " + localHdg.hediff.defName + " to " + p.Label, MessageTypeDefOf.NegativeEvent, false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list));
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x00113D90 File Offset: 0x00111F90
		[DebugAction("Pawns", "Activate HediffGiver World Pawn", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ActivateHediffGiverWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawnLocal2 in from p in Find.WorldPawns.AllPawnsAlive
			where p.RaceProps.Humanlike
			select p)
			{
				Pawn pawnLocal = pawnLocal2;
				list.Add(new DebugMenuOption(pawnLocal.Label, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (HediffGiver hediffGiverLocal2 in pawnLocal.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef s) => s.hediffGivers))
					{
						HediffGiver hediffGiverLocal = hediffGiverLocal2;
						list2.Add(new DebugMenuOption(hediffGiverLocal.hediff.defName, DebugMenuOptionMode.Action, delegate()
						{
							if (hediffGiverLocal.TryApply(pawnLocal, null))
							{
								Messages.Message(hediffGiverLocal.hediff.defName + " applied to " + pawnLocal.Label, MessageTypeDefOf.NeutralEvent, false);
								return;
							}
							Messages.Message("failed to apply " + hediffGiverLocal.hediff.defName + " to " + pawnLocal.Label, MessageTypeDefOf.NegativeEvent, false);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x00113E44 File Offset: 0x00112044
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DiscoverHediffs(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				if (!hediff.Visible)
				{
					hediff.Severity = Mathf.Max(hediff.Severity, hediff.def.stages.First((HediffStage s) => s.becomeVisible).minSeverity);
				}
			}
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x00113EE8 File Offset: 0x001120E8
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrantImmunities(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(hediff.def);
				if (immunityRecord != null)
				{
					immunityRecord.immunity = 1f;
				}
			}
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x0001E991 File Offset: 0x0001CB91
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveBirth(Pawn p)
		{
			Hediff_Pregnant.DoBirthSpawn(p, null);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x00113F64 File Offset: 0x00112164
		[DebugAction("Pawns", "Resistance -1", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ResistanceMinus1(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 1f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x00113FB8 File Offset: 0x001121B8
		[DebugAction("Pawns", "Resistance -10", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ResistanceMinus10(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 10f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x0011400C File Offset: 0x0011220C
		[DebugAction("Pawns", "+20 neural heat", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddPsychicEntropy()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				if (thing is Pawn)
				{
					((Pawn)thing).psychicEntropy.TryAddEntropy(20f, null, true, false);
				}
			}
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x0011408C File Offset: 0x0011228C
		[DebugAction("Pawns", "-20 neural heat", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ReducePsychicEntropy()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				if (thing is Pawn)
				{
					((Pawn)thing).psychicEntropy.TryAddEntropy(-20f, null, true, false);
				}
			}
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x0011410C File Offset: 0x0011230C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ListMeleeVerbs(Pawn p)
		{
			List<Verb> allMeleeVerbs = (from x in p.meleeVerbs.GetUpdatedAvailableVerbsList(false)
			select x.verb).ToList<Verb>();
			float highestWeight = 0f;
			foreach (Verb v2 in allMeleeVerbs)
			{
				float num = VerbUtility.InitialVerbWeight(v2, p);
				if (num > highestWeight)
				{
					highestWeight = num;
				}
			}
			float totalSelectionWeight = 0f;
			foreach (Verb verb in allMeleeVerbs)
			{
				totalSelectionWeight += VerbUtility.FinalSelectionWeight(verb, p, allMeleeVerbs, highestWeight);
			}
			allMeleeVerbs.SortBy((Verb x) => -VerbUtility.InitialVerbWeight(x, p));
			List<TableDataGetter<Verb>> list = new List<TableDataGetter<Verb>>();
			list.Add(new TableDataGetter<Verb>("verb", (Verb v) => v.ToString().Split(new char[]
			{
				'/'
			})[1].TrimEnd(new char[]
			{
				')'
			})));
			list.Add(new TableDataGetter<Verb>("source", delegate(Verb v)
			{
				if (v.HediffSource != null)
				{
					return v.HediffSource.Label;
				}
				if (v.tool != null)
				{
					return v.tool.label;
				}
				return "";
			}));
			list.Add(new TableDataGetter<Verb>("damage", (Verb v) => v.verbProps.AdjustedMeleeDamageAmount(v, p)));
			list.Add(new TableDataGetter<Verb>("cooldown", (Verb v) => v.verbProps.AdjustedCooldown(v, p) + "s"));
			list.Add(new TableDataGetter<Verb>("dmg/sec", (Verb v) => VerbUtility.DPS(v, p)));
			list.Add(new TableDataGetter<Verb>("armor pen", (Verb v) => v.verbProps.AdjustedArmorPenetration(v, p)));
			list.Add(new TableDataGetter<Verb>("hediff", delegate(Verb v)
			{
				string text = "";
				if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty<DamageDefAdditionalHediff>())
				{
					foreach (DamageDefAdditionalHediff damageDefAdditionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
					{
						text = text + damageDefAdditionalHediff.hediff.label + " ";
					}
				}
				return text;
			}));
			list.Add(new TableDataGetter<Verb>("weight", (Verb v) => VerbUtility.InitialVerbWeight(v, p)));
			list.Add(new TableDataGetter<Verb>("category", delegate(Verb v)
			{
				VerbSelectionCategory selectionCategory = v.GetSelectionCategory(p, highestWeight);
				if (selectionCategory == VerbSelectionCategory.Best)
				{
					return "Best".Colorize(Color.green);
				}
				if (selectionCategory != VerbSelectionCategory.Worst)
				{
					return "Mid";
				}
				return "Worst".Colorize(Color.grey);
			}));
			list.Add(new TableDataGetter<Verb>("sel %", (Verb v) => base.<ListMeleeVerbs>g__GetSelectionPercent|1(v).ToStringPercent("F2")));
			List<TableDataGetter<Verb>> list2 = list;
			DebugTables.MakeTablesDialog<Verb>(allMeleeVerbs, list2.ToArray());
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x001143B8 File Offset: 0x001125B8
		[DebugAction("Pawns", "Add/remove pawn relation", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddRemovePawnRelation(Pawn p)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return;
			}
			Func<Pawn, bool> <>9__5;
			Action<bool> act = delegate(bool add)
			{
				if (add)
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (PawnRelationDef pawnRelationDef in DefDatabase<PawnRelationDef>.AllDefs)
					{
						if (!pawnRelationDef.implied)
						{
							PawnRelationDef defLocal = pawnRelationDef;
							list2.Add(new DebugMenuOption(defLocal.defName, DebugMenuOptionMode.Action, delegate()
							{
								List<DebugMenuOption> list4 = new List<DebugMenuOption>();
								IEnumerable<Pawn> source = from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
								where x.RaceProps.IsFlesh
								select x;
								Func<Pawn, bool> keySelector;
								if ((keySelector = <>9__5) == null)
								{
									keySelector = (<>9__5 = ((Pawn x) => x.def == p.def));
								}
								foreach (Pawn pawn in source.OrderByDescending(keySelector).ThenBy((Pawn x) => x.IsWorldPawn()))
								{
									if (p != pawn && (!defLocal.familyByBloodRelation || pawn.def == p.def) && !p.relations.DirectRelationExists(defLocal, pawn))
									{
										Pawn otherLocal = pawn;
										list4.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
										{
											p.relations.AddDirectRelation(defLocal, otherLocal);
										}));
									}
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					return;
				}
				List<DebugMenuOption> list3 = new List<DebugMenuOption>();
				List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					DirectPawnRelation rel = directRelations[i];
					list3.Add(new DebugMenuOption(rel.def.defName + " - " + rel.otherPawn.LabelShort, DebugMenuOptionMode.Action, delegate()
					{
						p.relations.RemoveDirectRelation(rel);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Add", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			list.Add(new DebugMenuOption("Remove", DebugMenuOptionMode.Action, delegate()
			{
				act(false);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x00114448 File Offset: 0x00112648
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddOpinionTalksAbout(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			Action<bool> act = delegate(bool good)
			{
				Func<ThoughtDef, bool> <>9__4;
				foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Humanlike
				select x)
				{
					if (p != pawn)
					{
						IEnumerable<ThoughtDef> allDefs = DefDatabase<ThoughtDef>.AllDefs;
						Func<ThoughtDef, bool> predicate;
						if ((predicate = <>9__4) == null)
						{
							predicate = (<>9__4 = ((ThoughtDef x) => typeof(Thought_MemorySocial).IsAssignableFrom(x.thoughtClass) && ((good && x.stages[0].baseOpinionOffset > 0f) || (!good && x.stages[0].baseOpinionOffset < 0f))));
						}
						IEnumerable<ThoughtDef> source = allDefs.Where(predicate);
						if (source.Any<ThoughtDef>())
						{
							int num = Rand.Range(2, 5);
							for (int i = 0; i < num; i++)
							{
								ThoughtDef def = source.RandomElement<ThoughtDef>();
								pawn.needs.mood.thoughts.memories.TryGainMemory(def, p);
							}
						}
					}
				}
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Good", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			list.Add(new DebugMenuOption("Bad", DebugMenuOptionMode.Action, delegate()
			{
				act(false);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x001144D8 File Offset: 0x001126D8
		[DebugAction("Pawns", "Force vomit...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceVomit(Pawn p)
		{
			p.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false);
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x0001E9A0 File Offset: 0x0001CBA0
		[DebugAction("Pawns", "Psyfocus +20%", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetPsyfocusPositive20(Pawn p)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = p.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(0.2f);
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x0001E9B7 File Offset: 0x0001CBB7
		[DebugAction("Pawns", "Psyfocus -20%", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetPsyfocusNegative20(Pawn p)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = p.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(-0.2f);
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x0001E9CE File Offset: 0x0001CBCE
		[DebugAction("Pawns", "Food -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetFoodNegative20()
		{
			DebugToolsPawns.OffsetNeed(NeedDefOf.Food, -0.2f);
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x0001E9DF File Offset: 0x0001CBDF
		[DebugAction("Pawns", "Rest -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetRestNegative20()
		{
			DebugToolsPawns.OffsetNeed(NeedDefOf.Rest, -0.2f);
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x0001E9F0 File Offset: 0x0001CBF0
		[DebugAction("Pawns", "Joy -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetJoyNegative20()
		{
			DebugToolsPawns.OffsetNeed(NeedDefOf.Joy, -0.2f);
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x0011450C File Offset: 0x0011270C
		[DebugAction("Pawns", "Chemical -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetChemicalNegative20()
		{
			List<NeedDef> allDefsListForReading = DefDatabase<NeedDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (typeof(Need_Chemical).IsAssignableFrom(allDefsListForReading[i].needClass))
				{
					DebugToolsPawns.OffsetNeed(allDefsListForReading[i], -0.2f);
				}
			}
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x00114560 File Offset: 0x00112760
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetSkill()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					for (int i = 0; i <= 20; i++)
					{
						int level = i;
						list2.Add(new DebugMenuOption(level.ToString(), DebugMenuOptionMode.Tool, delegate()
						{
							Pawn pawn = (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
							where t is Pawn
							select t).Cast<Pawn>().FirstOrDefault<Pawn>();
							if (pawn == null)
							{
								return;
							}
							SkillRecord skill = pawn.skills.GetSkill(localDef);
							skill.Level = level;
							skill.xpSinceLastLevel = skill.XpRequiredForLevelUp / 2f;
							DebugActionsUtility.DustPuffFrom(pawn);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x001145EC File Offset: 0x001127EC
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MaxSkill()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<SkillDef> enumerator = DefDatabase<SkillDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SkillDef def = enumerator.Current;
					SkillDef def2 = def;
					list.Add(new DebugMenuOption(def2.defName, DebugMenuOptionMode.Tool, delegate()
					{
						Pawn pawn = (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>().FirstOrDefault<Pawn>();
						if (pawn == null || pawn.skills == null)
						{
							return;
						}
						pawn.skills.Learn(def, 100000000f, false);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x00114678 File Offset: 0x00112878
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MaxAllSkills(Pawn p)
		{
			if (p.skills != null)
			{
				foreach (SkillDef sDef in DefDatabase<SkillDef>.AllDefs)
				{
					p.skills.Learn(sDef, 100000000f, false);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
			if (p.training != null)
			{
				foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
				{
					Pawn trainer = p.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
					bool flag;
					if (p.training.CanAssignToTrain(td, out flag).Accepted)
					{
						p.training.Train(td, trainer, false);
					}
				}
			}
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x00114758 File Offset: 0x00112958
		[DebugAction("Pawns", "Mental break...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MentalBreak()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(log possibles)", DebugMenuOptionMode.Tool, delegate()
			{
				foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Pawn
				select t).Cast<Pawn>())
				{
					pawn.mindState.mentalBreaker.LogPossibleMentalBreaks();
					DebugActionsUtility.DustPuffFrom(pawn);
				}
			}));
			list.Add(new DebugMenuOption("(natural mood break)", DebugMenuOptionMode.Tool, delegate()
			{
				foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Pawn
				select t).Cast<Pawn>())
				{
					pawn.mindState.mentalBreaker.TryDoRandomMoodCausedMentalBreak();
					DebugActionsUtility.DustPuffFrom(pawn);
				}
			}));
			foreach (MentalBreakDef locBrDef2 in from x in DefDatabase<MentalBreakDef>.AllDefs
			orderby x.intensity descending
			select x)
			{
				MentalBreakDef locBrDef = locBrDef2;
				string text = locBrDef.defName;
				if (!Find.CurrentMap.mapPawns.FreeColonists.Any((Pawn x) => locBrDef.Worker.BreakCanOccur(x)))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).Where((Thing t) => t is Pawn).Cast<Pawn>())
					{
						locBrDef.Worker.TryStart(pawn, null, false);
						DebugActionsUtility.DustPuffFrom(pawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x0011489C File Offset: 0x00112A9C
		[DebugAction("Pawns", "Mental state...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MentalState()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (MentalStateDef locBrDef2 in DefDatabase<MentalStateDef>.AllDefs)
			{
				MentalStateDef locBrDef = locBrDef2;
				string text = locBrDef.defName;
				if (!Find.CurrentMap.mapPawns.FreeColonists.Any((Pawn x) => locBrDef.Worker.StateCanOccur(x)))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn locP2 in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						Pawn locP = locP2;
						if (locBrDef != MentalStateDefOf.SocialFighting)
						{
							locP.mindState.mentalStateHandler.TryStartMentalState(locBrDef, null, true, false, null, false);
							DebugActionsUtility.DustPuffFrom(locP);
						}
						else
						{
							DebugTools.curTool = new DebugTool("...with", delegate()
							{
								Pawn pawn = (Pawn)(from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
								where t is Pawn
								select t).FirstOrDefault<Thing>();
								if (pawn != null)
								{
									locP.interactions.StartSocialFight(pawn);
									DebugTools.curTool = null;
								}
							}, null);
						}
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x0001EA01 File Offset: 0x0001CC01
		[DebugAction("Pawns", "Stop mental state", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StopMentalState(Pawn p)
		{
			if (p.InMentalState)
			{
				p.MentalState.RecoverFromState();
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x0011495C File Offset: 0x00112B5C
		[DebugAction("Pawns", "Inspiration...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Inspiration()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (InspirationDef localDef2 in DefDatabase<InspirationDef>.AllDefs)
			{
				InspirationDef localDef = localDef2;
				string text = localDef.defName;
				if (!Find.CurrentMap.mapPawns.FreeColonists.Any((Pawn x) => localDef.Worker.InspirationCanOccur(x)))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>())
					{
						pawn.mindState.inspirationHandler.TryStartInspiration_NewTemp(localDef, "Debug gain");
						DebugActionsUtility.DustPuffFrom(pawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x00114A1C File Offset: 0x00112C1C
		[DebugAction("Pawns", "Give trait...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveTrait()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (TraitDef traitDef in DefDatabase<TraitDef>.AllDefs)
			{
				TraitDef trDef = traitDef;
				for (int j = 0; j < traitDef.degreeDatas.Count; j++)
				{
					int i = j;
					list.Add(new DebugMenuOption(string.Concat(new object[]
					{
						trDef.degreeDatas[i].label,
						" (",
						trDef.degreeDatas[j].degree,
						")"
					}), DebugMenuOptionMode.Tool, delegate()
					{
						foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							if (pawn.story != null)
							{
								Trait trait = new Trait(trDef, trDef.degreeDatas[i].degree, false);
								pawn.story.traits.GainTrait(trait);
								DebugActionsUtility.DustPuffFrom(pawn);
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x00114B40 File Offset: 0x00112D40
		[DebugAction("Pawns", "Set backstory...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetBackstory()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("adulthood", DebugMenuOptionMode.Action, delegate()
			{
				DebugToolsPawns.<SetBackstory>g__AddBackstoryOption|42_0(BackstorySlot.Adulthood);
			}));
			list.Add(new DebugMenuOption("childhood", DebugMenuOptionMode.Action, delegate()
			{
				DebugToolsPawns.<SetBackstory>g__AddBackstoryOption|42_0(BackstorySlot.Childhood);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x00114BC4 File Offset: 0x00112DC4
		[DebugAction("Pawns", "Give ability...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveAbility()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*All", DebugMenuOptionMode.Tool, delegate()
			{
				foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Pawn
				select t).Cast<Pawn>())
				{
					foreach (AbilityDef def in DefDatabase<AbilityDef>.AllDefs)
					{
						pawn.abilities.GainAbility(def);
					}
				}
			}));
			foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefs)
			{
				AbilityDef localAb = abilityDef;
				list.Add(new DebugMenuOption(abilityDef.label, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						pawn.abilities.GainAbility(localAb);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x00114C7C File Offset: 0x00112E7C
		[DebugAction("Pawns", "Give Psylink...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GivePsylink()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			for (int i = 1; i <= 6; i++)
			{
				int level = i;
				list.Add(new DebugMenuOption("Level".Translate() + ": " + i, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						Hediff_ImplantWithLevel hediff_ImplantWithLevel = pawn.GetMainPsylinkSource();
						if (hediff_ImplantWithLevel == null)
						{
							hediff_ImplantWithLevel = (HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, pawn, pawn.health.hediffSet.GetBrain()) as Hediff_ImplantWithLevel);
							pawn.health.AddHediff(hediff_ImplantWithLevel, null, null, null);
						}
						hediff_ImplantWithLevel.ChangeLevel(level - hediff_ImplantWithLevel.level);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x0001EA24 File Offset: 0x0001CC24
		[DebugAction("Pawns", "Remove neural heat", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemovePsychicEntropy(Pawn p)
		{
			if (p.psychicEntropy != null)
			{
				p.psychicEntropy.TryAddEntropy(-1000f, null, true, false);
			}
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x0001EA42 File Offset: 0x0001CC42
		[DebugAction("Pawns", "Give good thought...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveGoodThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugGood, null);
			}
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x0001EA71 File Offset: 0x0001CC71
		[DebugAction("Pawns", "Give bad thought...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveBadThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugBad, null);
			}
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x00114CF4 File Offset: 0x00112EF4
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearBoundUnfinishedThings()
		{
			foreach (Building_WorkTable building_WorkTable in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Building_WorkTable
			select t).Cast<Building_WorkTable>())
			{
				foreach (Bill bill in building_WorkTable.BillStack)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = bill as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null)
					{
						bill_ProductionWithUft.ClearBoundUft();
					}
				}
			}
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x0001EAA0 File Offset: 0x0001CCA0
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceBirthday(Pawn p)
		{
			p.ageTracker.AgeBiologicalTicks = (long)((p.ageTracker.AgeBiologicalYears + 1) * 3600000 + 1);
			p.ageTracker.DebugForceBirthdayBiological();
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x00114DB4 File Offset: 0x00112FB4
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Recruit(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p, 1f, true);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x00114E04 File Offset: 0x00113004
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageApparel(Pawn p)
		{
			if (p.apparel != null && p.apparel.WornApparelCount > 0)
			{
				p.apparel.WornApparel.RandomElement<Apparel>().TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 30f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x00114E64 File Offset: 0x00113064
		[DebugAction("Pawns", "Wear apparel (selected)...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void WearApparel_ToSelected()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Remove all apparel", DebugMenuOptionMode.Action, delegate()
			{
				using (List<object>.Enumerator enumerator2 = Find.Selector.SelectedObjectsListForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Pawn pawn;
						if ((pawn = (enumerator2.Current as Pawn)) != null)
						{
							pawn.apparel.DestroyAll(DestroyMode.Vanish);
						}
					}
				}
			}));
			using (IEnumerator<ThingDef> enumerator = (from def in DefDatabase<ThingDef>.AllDefs
			where def.IsApparel
			select def into d
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						foreach (object obj in Find.Selector.SelectedObjectsListForReading)
						{
							ThingDef stuff = GenStuff.RandomStuffFor(def);
							Apparel newApparel = (Apparel)ThingMaker.MakeThing(def, stuff);
							Pawn pawn;
							if ((pawn = (obj as Pawn)) != null)
							{
								pawn.apparel.Wear(newApparel, false, false);
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x00114F68 File Offset: 0x00113168
		[DebugAction("Pawns", "Equip primary (selected)...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void EquipPrimary_ToSelected()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Remove primary", DebugMenuOptionMode.Action, delegate()
			{
				using (List<object>.Enumerator enumerator2 = Find.Selector.SelectedObjectsListForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Pawn pawn;
						if ((pawn = (enumerator2.Current as Pawn)) != null && pawn.equipment != null && pawn.equipment.Primary != null)
						{
							pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
						}
					}
				}
			}));
			using (IEnumerator<ThingDef> enumerator = (from def in DefDatabase<ThingDef>.AllDefs
			where def.equipmentType == EquipmentType.Primary
			select def into d
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						using (List<object>.Enumerator enumerator2 = Find.Selector.SelectedObjectsListForReading.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Pawn pawn;
								if ((pawn = (enumerator2.Current as Pawn)) != null && pawn.equipment != null)
								{
									if (pawn.equipment.Primary != null)
									{
										pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
									}
									ThingDef stuff = GenStuff.RandomStuffFor(def);
									ThingWithComps newEq = (ThingWithComps)ThingMaker.MakeThing(def, stuff);
									pawn.equipment.AddEquipment(newEq);
								}
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x0001EACE File Offset: 0x0001CCCE
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TameAnimal(Pawn p)
		{
			if (p.AnimalOrWildMan() && p.Faction != Faction.OfPlayer)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.FirstOrDefault<Pawn>(), p, 1f, true);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x0011506C File Offset: 0x0011326C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TrainAnimal(Pawn p)
		{
			if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				DebugActionsUtility.DustPuffFrom(p);
				bool flag = false;
				foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
				{
					if (p.training.GetWanted(td))
					{
						p.training.Train(td, null, true);
						flag = true;
					}
				}
				if (!flag)
				{
					foreach (TrainableDef td2 in DefDatabase<TrainableDef>.AllDefs)
					{
						if (p.training.CanAssignToTrain(td2).Accepted)
						{
							p.training.Train(td2, null, true);
						}
					}
				}
			}
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x00115160 File Offset: 0x00113360
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryDevelopBoundRelation(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			if (p.RaceProps.Humanlike)
			{
				IEnumerable<Pawn> source = from x in p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Animal && x.Faction == p.Faction
				select x;
				if (source.Any<Pawn>())
				{
					RelationsUtility.TryDevelopBondRelation(p, source.RandomElement<Pawn>(), 999999f);
					return;
				}
			}
			else if (p.RaceProps.Animal)
			{
				IEnumerable<Pawn> source2 = from x in p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Humanlike && x.Faction == p.Faction
				select x;
				if (source2.Any<Pawn>())
				{
					RelationsUtility.TryDevelopBondRelation(source2.RandomElement<Pawn>(), p, 999999f);
				}
			}
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x0001EB0C File Offset: 0x0001CD0C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void QueueTrainingDecay(Pawn p)
		{
			if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				p.training.Debug_MakeDegradeHappenSoon();
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x0011523C File Offset: 0x0011343C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartMarriageCeremony(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike
			select x)
			{
				if (p != pawn)
				{
					Pawn otherLocal = pawn;
					list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
					{
						if (!p.relations.DirectRelationExists(PawnRelationDefOf.Fiance, otherLocal))
						{
							p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, otherLocal);
							p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Spouse, otherLocal);
							p.relations.AddDirectRelation(PawnRelationDefOf.Fiance, otherLocal);
							Messages.Message("Dev: Auto added fiance relation.", p, MessageTypeDefOf.TaskCompletion, false);
						}
						if (!p.Map.lordsStarter.TryStartMarriageCeremony(p, otherLocal))
						{
							Messages.Message("Could not find any valid marriage site.", MessageTypeDefOf.RejectInput, false);
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x00115354 File Offset: 0x00113554
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceInteraction(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction))
			{
				if (pawn != p)
				{
					Pawn otherLocal = pawn;
					list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (InteractionDef interactionLocal2 in DefDatabase<InteractionDef>.AllDefsListForReading)
						{
							InteractionDef interactionLocal = interactionLocal2;
							list2.Add(new DebugMenuOption(interactionLocal.label, DebugMenuOptionMode.Action, delegate()
							{
								p.interactions.TryInteractWith(otherLocal, interactionLocal);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x0001EB41 File Offset: 0x0001CD41
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartRandomGathering()
		{
			if (!Find.CurrentMap.lordsStarter.TryStartRandomGathering(true))
			{
				Messages.Message("Could not find any valid gathering spot or organizer.", MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x00115454 File Offset: 0x00113654
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartGathering()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<GatheringDef>.Enumerator enumerator = DefDatabase<GatheringDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GatheringDef gatheringDef = enumerator.Current;
					list.Add(new DebugMenuOption(gatheringDef.LabelCap + " (" + (gatheringDef.Worker.CanExecute(Find.CurrentMap, null) ? "Yes" : "No") + ")", DebugMenuOptionMode.Action, delegate()
					{
						gatheringDef.Worker.TryExecute(Find.CurrentMap, null);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x0001EB65 File Offset: 0x0001CD65
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartPrisonBreak(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return;
			}
			PrisonBreakUtility.StartPrisonBreak(p);
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x0001EB76 File Offset: 0x0001CD76
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PassToWorld(Pawn p)
		{
			p.DeSpawn(DestroyMode.Vanish);
			Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.KeepForever);
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x0001EB8B File Offset: 0x0001CD8B
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Make1YearOlder(Pawn p)
		{
			p.ageTracker.DebugMake1YearOlder();
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x00115528 File Offset: 0x00113728
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryJobGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localType2 in typeof(ThinkNode_JobGiver).AllSubclasses())
			{
				Type localType = localType2;
				list.Add(new DebugMenuOption(localType.Name, DebugMenuOptionMode.Action, delegate()
				{
					ThinkNode_JobGiver thinkNode_JobGiver = (ThinkNode_JobGiver)Activator.CreateInstance(localType);
					thinkNode_JobGiver.ResolveReferences();
					ThinkResult thinkResult = thinkNode_JobGiver.TryIssueJobPackage(p, default(JobIssueParams));
					if (thinkResult.Job != null)
					{
						p.jobs.StartJob(thinkResult.Job, JobCondition.None, null, false, true, null, null, false, false);
						return;
					}
					Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x001155D8 File Offset: 0x001137D8
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryJoyGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<JoyGiverDef>.Enumerator enumerator = DefDatabase<JoyGiverDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JoyGiverDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.Worker.CanBeGivenTo(p) ? def.defName : (def.defName + " [NO]"), DebugMenuOptionMode.Action, delegate()
					{
						Job job = def.Worker.TryGiveJob(p);
						if (job != null)
						{
							p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false);
							return;
						}
						Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x0001EB98 File Offset: 0x0001CD98
		[DebugAction("Pawns", "EndCurrentJob(InterruptForced)", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void EndCurrentJobInterruptForced(Pawn p)
		{
			p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x0001EBAE File Offset: 0x0001CDAE
		[DebugAction("Pawns", "CheckForJobOverride", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckForJobOverride(Pawn p)
		{
			p.jobs.CheckForJobOverride();
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x001156B0 File Offset: 0x001138B0
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleJobLogging(Pawn p)
		{
			p.jobs.debugLog = !p.jobs.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
			MoteMaker.ThrowText(p.DrawPos, p.Map, p.LabelShort + "\n" + (p.jobs.debugLog ? "ON" : "OFF"), -1f);
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x0001EBC1 File Offset: 0x0001CDC1
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleStanceLogging(Pawn p)
		{
			p.stances.debugLog = !p.stances.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x0011571C File Offset: 0x0011391C
		private static void OffsetNeed(NeedDef nd, float offsetPct)
		{
			foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Pawn
			select t).Cast<Pawn>())
			{
				Need need = pawn.needs.TryGetNeed(nd);
				if (need != null)
				{
					need.CurLevel += offsetPct * need.MaxLevel;
					DebugActionsUtility.DustPuffFrom(pawn);
				}
			}
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x001157C4 File Offset: 0x001139C4
		[DebugAction("Pawns", "Kidnap colonist", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Kidnap(Pawn p)
		{
			if (p.IsColonist)
			{
				Faction faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
				if (faction != null)
				{
					faction.kidnapped.Kidnap(p, faction.leader);
				}
			}
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x00115800 File Offset: 0x00113A00
		[DebugAction("Pawns", "Face cell (selected)...", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Selected_SetFacing()
		{
			using (List<object>.Enumerator enumerator = Find.Selector.SelectedObjectsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn;
					if ((pawn = (enumerator.Current as Pawn)) != null)
					{
						pawn.rotationTracker.FaceTarget(UI.MouseCell());
					}
				}
			}
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x00115870 File Offset: 0x00113A70
		[CompilerGenerated]
		internal static void <SetBackstory>g__AddBackstoryOption|42_0(BackstorySlot slot)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (Dictionary<string, Backstory>.Enumerator enumerator = BackstoryDatabase.allBackstories.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, Backstory> outerBackstory = enumerator.Current;
					if (outerBackstory.Value.slot == slot)
					{
						list.Add(new DebugMenuOption(outerBackstory.Key, DebugMenuOptionMode.Tool, delegate()
						{
							foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
							where t is Pawn
							select t).Cast<Pawn>())
							{
								if (pawn.story != null)
								{
									if (slot == BackstorySlot.Adulthood)
									{
										pawn.story.adulthood = outerBackstory.Value;
									}
									else
									{
										pawn.story.childhood = outerBackstory.Value;
									}
									MeditationFocusTypeAvailabilityCache.ClearFor(pawn);
									DebugActionsUtility.DustPuffFrom(pawn);
								}
							}
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}
	}
}
