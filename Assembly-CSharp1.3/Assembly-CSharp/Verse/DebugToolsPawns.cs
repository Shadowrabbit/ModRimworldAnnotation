using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020003A5 RID: 933
	public static class DebugToolsPawns
	{
		// Token: 0x06001BE8 RID: 7144 RVA: 0x000A3904 File Offset: 0x000A1B04
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

		// Token: 0x06001BE9 RID: 7145 RVA: 0x000A3974 File Offset: 0x000A1B74
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageUntilDown(Pawn p)
		{
			HealthUtility.DamageUntilDowned(p, true);
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000A397D File Offset: 0x000A1B7D
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageLegs(Pawn p)
		{
			HealthUtility.DamageLegsUntilIncapableOfMoving(p, true);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x000A3986 File Offset: 0x000A1B86
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p);
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x000A398E File Offset: 0x000A1B8E
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CarriedDamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p.carryTracker.CarriedThing as Pawn);
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x000A39A8 File Offset: 0x000A1BA8
		[DebugAction("Pawns", "10 damage until dead", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Do10DamageUntilDead()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				for (int i = 0; i < 1000; i++)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
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
						Log.Message(str + ".");
						break;
					}
				}
			}
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x000A3B4C File Offset: 0x000A1D4C
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

		// Token: 0x06001BEF RID: 7151 RVA: 0x000A3BE8 File Offset: 0x000A1DE8
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SurgeryFailMinor(Pawn p)
		{
			BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where !x.def.conceptual
			select x).RandomElement<BodyPartRecord>();
			Log.Message("part is " + bodyPartRecord);
			HealthUtility.GiveInjuriesOperationFailureMinor(p, bodyPartRecord);
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x000A3C4C File Offset: 0x000A1E4C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SurgeryFailCatastrophic(Pawn p)
		{
			BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where !x.def.conceptual
			select x).RandomElement<BodyPartRecord>();
			Log.Message("part is " + bodyPartRecord);
			HealthUtility.GiveInjuriesOperationFailureCatastrophic(p, bodyPartRecord);
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000A3CAE File Offset: 0x000A1EAE
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SurgeryFailRidiculous(Pawn p)
		{
			HealthUtility.GiveInjuriesOperationFailureRidiculous(p);
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000A3CB6 File Offset: 0x000A1EB6
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RestoreBodyPart(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RestorePart(p)));
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000A3CCD File Offset: 0x000A1ECD
		[DebugAction("Pawns", "Apply damage...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ApplyDamage()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_ApplyDamage()));
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000A3CE3 File Offset: 0x000A1EE3
		[DebugAction("Pawns", "Add Hediff...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddHediff()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_AddHediff()));
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000A3CF9 File Offset: 0x000A1EF9
		[DebugAction("Pawns", "Remove Hediff...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveHediff(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RemoveHediff(p)));
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000A3D10 File Offset: 0x000A1F10
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

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000A3D68 File Offset: 0x000A1F68
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
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list));
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000A3E74 File Offset: 0x000A2074
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

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000A3F28 File Offset: 0x000A2128
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

		// Token: 0x06001BFA RID: 7162 RVA: 0x000A3FCC File Offset: 0x000A21CC
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

		// Token: 0x06001BFB RID: 7163 RVA: 0x000A4048 File Offset: 0x000A2248
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveBirth(Pawn p)
		{
			Hediff_Pregnant.DoBirthSpawn(p, null);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000A4058 File Offset: 0x000A2258
		[DebugAction("Pawns", "Resistance -1", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ResistanceMinus1(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 1f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000A40AC File Offset: 0x000A22AC
		[DebugAction("Pawns", "Resistance -10", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ResistanceMinus10(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 10f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000A4100 File Offset: 0x000A2300
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

		// Token: 0x06001BFF RID: 7167 RVA: 0x000A4180 File Offset: 0x000A2380
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

		// Token: 0x06001C00 RID: 7168 RVA: 0x000A4200 File Offset: 0x000A2400
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

		// Token: 0x06001C01 RID: 7169 RVA: 0x000A44AC File Offset: 0x000A26AC
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
											if (defLocal == PawnRelationDefOf.Fiance)
											{
												otherLocal.relations.nextMarriageNameChange = (p.relations.nextMarriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage(p));
											}
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
			if (!p.relations.DirectRelations.NullOrEmpty<DirectPawnRelation>())
			{
				list.Add(new DebugMenuOption("Remove", DebugMenuOptionMode.Action, delegate()
				{
					act(false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x000A4554 File Offset: 0x000A2754
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
								pawn.needs.mood.thoughts.memories.TryGainMemory(def, p, null);
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

		// Token: 0x06001C03 RID: 7171 RVA: 0x000A45E4 File Offset: 0x000A27E4
		[DebugAction("Pawns", "Force vomit...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceVomit(Pawn p)
		{
			p.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false);
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x000A4616 File Offset: 0x000A2816
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

		// Token: 0x06001C05 RID: 7173 RVA: 0x000A462D File Offset: 0x000A282D
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

		// Token: 0x06001C06 RID: 7174 RVA: 0x000A4644 File Offset: 0x000A2844
		[DebugAction("Pawns", "Food -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetFoodNegative20()
		{
			DebugToolsPawns.OffsetNeed(NeedDefOf.Food, -0.2f);
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000A4655 File Offset: 0x000A2855
		[DebugAction("Pawns", "Rest -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetRestNegative20()
		{
			DebugToolsPawns.OffsetNeed(NeedDefOf.Rest, -0.2f);
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x000A4666 File Offset: 0x000A2866
		[DebugAction("Pawns", "Joy -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetJoyNegative20()
		{
			DebugToolsPawns.OffsetNeed(NeedDefOf.Joy, -0.2f);
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x000A4678 File Offset: 0x000A2878
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

		// Token: 0x06001C0A RID: 7178 RVA: 0x000A46CA File Offset: 0x000A28CA
		[DebugAction("Pawns", "Indoors -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetIndoorsNegative20()
		{
			DebugToolsPawns.OffsetNeed(NeedDefOf.Indoors, -0.2f);
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x000A46DC File Offset: 0x000A28DC
		[DebugAction("Pawns", "Needs misc -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetNeedsMiscNegative20()
		{
			foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Pawn
			select t).Cast<Pawn>())
			{
				Pawn_NeedsTracker needs = pawn.needs;
				foreach (Need need in from x in pawn.needs.AllNeeds
				where x != needs.food && x != needs.joy && x != needs.mood && x != needs.outdoors && x != needs.indoors && x != needs.rest && x != needs.roomsize && x != needs.authority && x != needs.beauty && x != needs.comfort && x != needs.drugsDesire && !(x is Need_Chemical)
				select x)
				{
					Need need2 = pawn.needs.TryGetNeed(need.def);
					if (need2 != null)
					{
						need2.CurLevel += -0.2f * need2.MaxLevel;
					}
				}
				DebugActionsUtility.DustPuffFrom(pawn);
			}
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x000A47F8 File Offset: 0x000A29F8
		[DebugAction("Pawns", "Certainty - 20%", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetCertaintyNegative20(Pawn p)
		{
			p.ideo.Debug_ReduceCertainty(0.2f);
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x000A480C File Offset: 0x000A2A0C
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

		// Token: 0x06001C0E RID: 7182 RVA: 0x000A4898 File Offset: 0x000A2A98
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

		// Token: 0x06001C0F RID: 7183 RVA: 0x000A4924 File Offset: 0x000A2B24
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

		// Token: 0x06001C10 RID: 7184 RVA: 0x000A4A04 File Offset: 0x000A2C04
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

		// Token: 0x06001C11 RID: 7185 RVA: 0x000A4B48 File Offset: 0x000A2D48
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DoVoiceCall(Pawn p)
		{
			Pawn_CallTracker caller = p.caller;
			if (caller == null)
			{
				return;
			}
			caller.DoCall();
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000A4B5C File Offset: 0x000A2D5C
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
							locP.mindState.mentalStateHandler.TryStartMentalState(locBrDef, null, true, false, null, false, false, false);
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
									locP.interactions.StartSocialFight(pawn, "MessageSocialFight");
									DebugTools.curTool = null;
								}
							}, null);
						}
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x000A4C1C File Offset: 0x000A2E1C
		[DebugAction("Pawns", "Stop mental state", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StopMentalState(Pawn p)
		{
			if (p.InMentalState)
			{
				p.MentalState.RecoverFromState();
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x000A4C40 File Offset: 0x000A2E40
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
						pawn.mindState.inspirationHandler.TryStartInspiration(localDef, "Debug gain", true);
						DebugActionsUtility.DustPuffFrom(pawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x000A4D00 File Offset: 0x000A2F00
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

		// Token: 0x06001C16 RID: 7190 RVA: 0x000A4E24 File Offset: 0x000A3024
		[DebugAction("Pawns", "Remove all traits", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveAllTraits(Pawn p)
		{
			if (p.story != null)
			{
				for (int i = p.story.traits.allTraits.Count - 1; i >= 0; i--)
				{
					p.story.traits.RemoveTrait(p.story.traits.allTraits[i]);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x000A4E88 File Offset: 0x000A3088
		[DebugAction("Pawns", "Set backstory...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetBackstory()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("adulthood", DebugMenuOptionMode.Action, delegate()
			{
				DebugToolsPawns.<SetBackstory>g__AddBackstoryOption|47_0(BackstorySlot.Adulthood);
			}));
			list.Add(new DebugMenuOption("childhood", DebugMenuOptionMode.Action, delegate()
			{
				DebugToolsPawns.<SetBackstory>g__AddBackstoryOption|47_0(BackstorySlot.Childhood);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x000A4F0C File Offset: 0x000A310C
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

		// Token: 0x06001C19 RID: 7193 RVA: 0x000A4FC4 File Offset: 0x000A31C4
		[DebugAction("Pawns", "Give Psylink...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GivePsylink()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			for (int i = 1; i <= 6; i++)
			{
				int level = i;
				list.Add(new DebugMenuOption("Level".Translate().CapitalizeFirst() + ": " + i, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						Hediff_Level hediff_Level = pawn.GetMainPsylinkSource();
						if (hediff_Level == null)
						{
							hediff_Level = (HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, pawn, pawn.health.hediffSet.GetBrain()) as Hediff_Level);
							pawn.health.AddHediff(hediff_Level, null, null, null);
						}
						hediff_Level.ChangeLevel(level - hediff_Level.level);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x000A5044 File Offset: 0x000A3244
		[DebugAction("Pawns", "Remove neural heat", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemovePsychicEntropy(Pawn p)
		{
			if (p.psychicEntropy != null)
			{
				p.psychicEntropy.TryAddEntropy(-1000f, null, true, false);
			}
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000A5062 File Offset: 0x000A3262
		[DebugAction("Pawns", "Give good thought...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveGoodThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugGood, null, null);
			}
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x000A5092 File Offset: 0x000A3292
		[DebugAction("Pawns", "Give bad thought...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveBadThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugBad, null, null);
			}
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000A50C4 File Offset: 0x000A32C4
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

		// Token: 0x06001C1E RID: 7198 RVA: 0x000A5184 File Offset: 0x000A3384
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceBirthday(Pawn p)
		{
			p.ageTracker.AgeBiologicalTicks = (long)((p.ageTracker.AgeBiologicalYears + 1) * 3600000 + 1);
			p.ageTracker.DebugForceBirthdayBiological();
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x000A51B2 File Offset: 0x000A33B2
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Recruit(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p, true);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000A51F0 File Offset: 0x000A33F0
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Enslave(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
			{
				GenGuest.EnslavePrisoner(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x000A5230 File Offset: 0x000A3430
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageApparel(Pawn p)
		{
			if (p.apparel != null && p.apparel.WornApparelCount > 0)
			{
				p.apparel.WornApparel.RandomElement<Apparel>().TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 30f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000A5290 File Offset: 0x000A3490
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

		// Token: 0x06001C23 RID: 7203 RVA: 0x000A5394 File Offset: 0x000A3594
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

		// Token: 0x06001C24 RID: 7204 RVA: 0x000A5498 File Offset: 0x000A3698
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TameAnimal(Pawn p)
		{
			if (p.AnimalOrWildMan() && p.Faction != Faction.OfPlayer)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.FirstOrDefault<Pawn>(), p, true);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x000A54D4 File Offset: 0x000A36D4
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

		// Token: 0x06001C26 RID: 7206 RVA: 0x000A55C8 File Offset: 0x000A37C8
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

		// Token: 0x06001C27 RID: 7207 RVA: 0x000A56A1 File Offset: 0x000A38A1
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void QueueTrainingDecay(Pawn p)
		{
			if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				p.training.Debug_MakeDegradeHappenSoon();
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x000A56D8 File Offset: 0x000A38D8
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DisplayRelationsInfo(Pawn pawn)
		{
			List<TableDataGetter<Pawn>> list = new List<TableDataGetter<Pawn>>();
			list.Add(new TableDataGetter<Pawn>("name", (Pawn p) => p.LabelCap));
			list.Add(new TableDataGetter<Pawn>("kind label", (Pawn p) => p.KindLabel));
			list.Add(new TableDataGetter<Pawn>("gender", (Pawn p) => p.gender.GetLabel(false)));
			list.Add(new TableDataGetter<Pawn>("age", (Pawn p) => p.ageTracker.AgeBiologicalYears));
			list.Add(new TableDataGetter<Pawn>("my compat", (Pawn p) => pawn.relations.CompatibilityWith(p).ToString("F2")));
			list.Add(new TableDataGetter<Pawn>("their compat", (Pawn p) => p.relations.CompatibilityWith(pawn).ToString("F2")));
			list.Add(new TableDataGetter<Pawn>("my 2nd\nrom chance", (Pawn p) => pawn.relations.SecondaryRomanceChanceFactor(p).ToStringPercent("F0")));
			list.Add(new TableDataGetter<Pawn>("their 2nd\nrom chance", (Pawn p) => p.relations.SecondaryRomanceChanceFactor(pawn).ToStringPercent("F0")));
			list.Add(new TableDataGetter<Pawn>("lovin mtb", (Pawn p) => LovePartnerRelationUtility.GetLovinMtbHours(pawn, p).ToString("F1") + " h"));
			List<TableDataGetter<Pawn>> list2 = list;
			DebugTables.MakeTablesDialog<Pawn>(from x in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
			where x != pawn && x.RaceProps.Humanlike
			select x, list2.ToArray());
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x000A587C File Offset: 0x000A3A7C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DisplayInteractionsInfo(Pawn pawn)
		{
			DebugToolsPawns.<>c__DisplayClass65_0 CS$<>8__locals1 = new DebugToolsPawns.<>c__DisplayClass65_0();
			CS$<>8__locals1.pawn = pawn;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<Pawn> source = CS$<>8__locals1.pawn.Map.mapPawns.SpawnedPawnsInFaction(CS$<>8__locals1.pawn.Faction);
			Func<Pawn, bool> predicate;
			if ((predicate = CS$<>8__locals1.<>9__0) == null)
			{
				predicate = (CS$<>8__locals1.<>9__0 = ((Pawn x) => x != CS$<>8__locals1.pawn && x.RaceProps.Humanlike));
			}
			using (IEnumerator<Pawn> enumerator = source.Where(predicate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugToolsPawns.<>c__DisplayClass65_1 CS$<>8__locals2 = new DebugToolsPawns.<>c__DisplayClass65_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.p = enumerator.Current;
					float totalWeight = DefDatabase<InteractionDef>.AllDefs.Sum((InteractionDef x) => x.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p));
					Func<InteractionDef, string> <>9__5;
					list.Add(new DebugMenuOption(CS$<>8__locals2.p.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						List<TableDataGetter<InteractionDef>> list2 = new List<TableDataGetter<InteractionDef>>();
						list2.Add(new TableDataGetter<InteractionDef>("defName", (InteractionDef i) => i.defName));
						string label = "sel weight";
						Func<InteractionDef, float> getter;
						if ((getter = CS$<>8__locals2.<>9__4) == null)
						{
							getter = (CS$<>8__locals2.<>9__4 = ((InteractionDef i) => i.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p)));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label, getter));
						string label2 = "sel chance";
						Func<InteractionDef, string> getter2;
						if ((getter2 = <>9__5) == null)
						{
							getter2 = (<>9__5 = ((InteractionDef i) => (i.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p) / totalWeight).ToStringPercent()));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label2, getter2));
						string label3 = "fight\nchance";
						Func<InteractionDef, string> getter3;
						if ((getter3 = CS$<>8__locals2.<>9__6) == null)
						{
							getter3 = (CS$<>8__locals2.<>9__6 = ((InteractionDef i) => CS$<>8__locals2.p.interactions.SocialFightChance(i, CS$<>8__locals2.CS$<>8__locals1.pawn).ToStringPercent()));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label3, getter3));
						string label4 = "success\nchance";
						Func<InteractionDef, string> getter4;
						if ((getter4 = CS$<>8__locals2.<>9__7) == null)
						{
							getter4 = (CS$<>8__locals2.<>9__7 = delegate(InteractionDef i)
							{
								if (i == InteractionDefOf.RomanceAttempt)
								{
									return ((InteractionWorker_RomanceAttempt)i.Worker).SuccessChance(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p).ToStringPercent();
								}
								if (i == InteractionDefOf.MarriageProposal)
								{
									return ((InteractionWorker_MarriageProposal)i.Worker).AcceptanceChance(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p).ToStringPercent();
								}
								return "";
							});
						}
						list2.Add(new TableDataGetter<InteractionDef>(label4, getter4));
						List<TableDataGetter<InteractionDef>> list3 = list2;
						DebugTables.MakeTablesDialog<InteractionDef>(DefDatabase<InteractionDef>.AllDefs, list3.ToArray());
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x000A5998 File Offset: 0x000A3B98
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

		// Token: 0x06001C2B RID: 7211 RVA: 0x000A5AB0 File Offset: 0x000A3CB0
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

		// Token: 0x06001C2C RID: 7212 RVA: 0x000A5BB0 File Offset: 0x000A3DB0
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartRandomGathering()
		{
			if (!Find.CurrentMap.lordsStarter.TryStartRandomGathering(true))
			{
				Messages.Message("Could not find any valid gathering spot or organizer.", MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000A5BD4 File Offset: 0x000A3DD4
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

		// Token: 0x06001C2E RID: 7214 RVA: 0x000A5CA8 File Offset: 0x000A3EA8
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartPrisonBreak(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return;
			}
			PrisonBreakUtility.StartPrisonBreak(p);
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000A5CB9 File Offset: 0x000A3EB9
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PassToWorld(Pawn p)
		{
			p.DeSpawn(DestroyMode.Vanish);
			Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.KeepForever);
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x000A5CCE File Offset: 0x000A3ECE
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Make1YearOlder(Pawn p)
		{
			p.ageTracker.DebugMakeOlder(3600000L);
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x000A5CE1 File Offset: 0x000A3EE1
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Make1DayOlder(Pawn p)
		{
			p.ageTracker.DebugMakeOlder(60000L);
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x000A5CF4 File Offset: 0x000A3EF4
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

		// Token: 0x06001C33 RID: 7219 RVA: 0x000A5DA8 File Offset: 0x000A3FA8
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

		// Token: 0x06001C34 RID: 7220 RVA: 0x000A5E80 File Offset: 0x000A4080
		[DebugAction("Pawns", "EndCurrentJob(InterruptForced)", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void EndCurrentJobInterruptForced(Pawn p)
		{
			p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000A5E96 File Offset: 0x000A4096
		[DebugAction("Pawns", "CheckForJobOverride", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckForJobOverride(Pawn p)
		{
			p.jobs.CheckForJobOverride();
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000A5EAC File Offset: 0x000A40AC
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleJobLogging(Pawn p)
		{
			p.jobs.debugLog = !p.jobs.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
			MoteMaker.ThrowText(p.DrawPos, p.Map, p.LabelShort + "\n" + (p.jobs.debugLog ? "ON" : "OFF"), -1f);
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000A5F17 File Offset: 0x000A4117
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleStanceLogging(Pawn p)
		{
			p.stances.debugLog = !p.stances.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000A5F38 File Offset: 0x000A4138
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

		// Token: 0x06001C39 RID: 7225 RVA: 0x000A5FE0 File Offset: 0x000A41E0
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

		// Token: 0x06001C3A RID: 7226 RVA: 0x000A601C File Offset: 0x000A421C
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

		// Token: 0x06001C3B RID: 7227 RVA: 0x000A608C File Offset: 0x000A428C
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddPrecept()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<PreceptDef> enumerator = DefDatabase<PreceptDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PreceptDef preceptDef = enumerator.Current;
					list.Add(new DebugMenuOption(preceptDef.issue.LabelCap + ": " + preceptDef.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						Faction.OfPlayer.ideos.PrimaryIdeo.AddPrecept(PreceptMaker.MakePrecept(preceptDef), true, null, null);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x000A613C File Offset: 0x000A433C
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemovePrecept()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<Precept>.Enumerator enumerator = Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept precept = enumerator.Current;
					list.Add(new DebugMenuOption(precept.def.issue.LabelCap + ": " + precept.def.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						Faction.OfPlayer.ideos.PrimaryIdeo.RemovePrecept(precept, false);
					}));
				}
			}
			using (List<Ideo>.Enumerator enumerator2 = Faction.OfPlayer.ideos.IdeosMinorListForReading.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Ideo ideo = enumerator2.Current;
					using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Precept precept = enumerator.Current;
							list.Add(new DebugMenuOption(precept.def.issue.LabelCap + ": " + precept.def.LabelCap, DebugMenuOptionMode.Action, delegate()
							{
								ideo.RemovePrecept(precept, false);
							}));
						}
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x000A630C File Offset: 0x000A450C
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TriggerDateRitual()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Ritual ritual;
					if ((ritual = (precept as Precept_Ritual)) != null && ritual.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>() != null)
					{
						string text = ritual.LabelCap;
						if (!ideo.ObligationsActive)
						{
							text += "[NO]";
						}
						list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
						{
							ritual.AddObligation(new RitualObligation(ritual, true));
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000A642C File Offset: 0x000A462C
		[DebugAction("Pawns", "Add 5 days to obligation timer", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Add5DaysToObligationTimer()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				using (List<Precept>.Enumerator enumerator2 = ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Precept_Ritual precept_Ritual;
						if ((precept_Ritual = (enumerator2.Current as Precept_Ritual)) != null && !precept_Ritual.activeObligations.NullOrEmpty<RitualObligation>())
						{
							using (List<RitualObligation>.Enumerator enumerator3 = precept_Ritual.activeObligations.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									RitualObligation obligation = enumerator3.Current;
									string text = precept_Ritual.LabelCap;
									string text2 = precept_Ritual.obligationTargetFilter.LabelExtraPart(obligation);
									if (text2.NullOrEmpty())
									{
										text = text + " " + text2;
									}
									list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
									{
										obligation.DebugOffsetTriggeredTick(-300000);
									}));
								}
							}
						}
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000A658C File Offset: 0x000A478C
		[DebugAction("Pawns", "Remove ritual obligation", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveRitualObligation()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Ritual ritual;
					if ((ritual = (precept as Precept_Ritual)) != null && !ritual.activeObligations.NullOrEmpty<RitualObligation>())
					{
						using (List<RitualObligation>.Enumerator enumerator3 = ritual.activeObligations.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								RitualObligation obligation = enumerator3.Current;
								string text = ritual.LabelCap;
								string text2 = ritual.obligationTargetFilter.LabelExtraPart(obligation);
								if (text2.NullOrEmpty())
								{
									text = text + " " + text2;
								}
								list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
								{
									ritual.activeObligations.Remove(obligation);
								}));
							}
						}
					}
				}
			}
			if (list.Any<DebugMenuOption>())
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				return;
			}
			Messages.Message("No obligations to remove.", LookTargets.Invalid, MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000A6770 File Offset: 0x000A4970
		[DebugAction("Pawns", "Generate 200 ritual names", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Generate200RitualNames()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Ritual ritual;
					if ((ritual = (precept as Precept_Ritual)) != null)
					{
						list.Add(new DebugMenuOption(ritual.def.issue.LabelCap + ": " + ritual.LabelCap, DebugMenuOptionMode.Action, delegate()
						{
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i < 200; i++)
							{
								stringBuilder.AppendLine(ritual.GenerateNameRaw());
							}
							Log.Message(stringBuilder.ToString());
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x000A6880 File Offset: 0x000A4A80
		[DebugAction("Pawns", "Set ideo role...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetIdeoRole(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			if (p.Ideo != null)
			{
				using (List<Precept_Role>.Enumerator enumerator = p.Ideo.cachedPossibleRoles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Precept_Role role = enumerator.Current;
						list.Add(new DebugMenuOption(role.LabelCap, DebugMenuOptionMode.Action, delegate()
						{
							role.Assign(p, true);
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x000A693C File Offset: 0x000A4B3C
		[DebugAction("Pawns", "Make guilty", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeGuilty(Pawn p)
		{
			Pawn_GuiltTracker guilt = p.guilt;
			if (guilt != null)
			{
				guilt.Notify_Guilty(60000);
			}
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000A695C File Offset: 0x000A4B5C
		[CompilerGenerated]
		internal static void <SetBackstory>g__AddBackstoryOption|47_0(BackstorySlot slot)
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
