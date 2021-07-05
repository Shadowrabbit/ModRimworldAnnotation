using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000399 RID: 921
	public static class DebugActionsIdeo
	{
		// Token: 0x06001B27 RID: 6951 RVA: 0x0009D4F8 File Offset: 0x0009B6F8
		[DebugOutput]
		public static void PreceptDefs()
		{
			IEnumerable<PreceptDef> dataSources = from x in DefDatabase<PreceptDef>.AllDefs
			orderby x.defName
			select x;
			TableDataGetter<PreceptDef>[] array = new TableDataGetter<PreceptDef>[8];
			array[0] = new TableDataGetter<PreceptDef>("defName", (PreceptDef x) => x.defName);
			array[1] = new TableDataGetter<PreceptDef>("impact", (PreceptDef x) => x.impact);
			array[2] = new TableDataGetter<PreceptDef>("duplicates", (PreceptDef x) => x.allowDuplicates.ToStringCheckBlank());
			array[3] = new TableDataGetter<PreceptDef>("defaultSelectionWeight", (PreceptDef x) => x.defaultSelectionWeight.ToString());
			array[4] = new TableDataGetter<PreceptDef>("noExpansion", (PreceptDef x) => x.classic.ToStringCheckBlank());
			array[5] = new TableDataGetter<PreceptDef>("exclusionTags", (PreceptDef x) => x.exclusionTags.ToCommaList(false, true));
			array[6] = new TableDataGetter<PreceptDef>("effects", (PreceptDef x) => string.Join("\n", x.comps.SelectMany((PreceptComp y) => y.GetDescriptions()).Distinct<string>()));
			array[7] = new TableDataGetter<PreceptDef>("stats", delegate(PreceptDef x)
			{
				IEnumerable<string> first;
				if (x.statOffsets == null)
				{
					first = Enumerable.Empty<string>();
				}
				else
				{
					first = from y in x.statOffsets
					select y.stat.defName + ": " + y.ValueToStringAsOffset;
				}
				IEnumerable<string> second;
				if (x.statFactors == null)
				{
					second = Enumerable.Empty<string>();
				}
				else
				{
					second = from y in x.statFactors
					select y.stat.defName + ": " + y.ToStringAsFactor;
				}
				return first.Concat(second).ToCommaList(false, true);
			});
			DebugTables.MakeTablesDialog<PreceptDef>(dataSources, array);
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0009D69C File Offset: 0x0009B89C
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		public static void SpawnRelic()
		{
			IntVec3 cell = UI.MouseCell();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Precept> preceptsListForReading = Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				Precept precept = preceptsListForReading[i];
				Precept_Relic relicPrecept;
				if ((relicPrecept = (precept as Precept_Relic)) != null)
				{
					list.Add(new DebugMenuOption(precept.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						GenSpawn.Spawn(relicPrecept.GenerateRelic(), cell, Find.CurrentMap, WipeMode.Vanish);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x0009D744 File Offset: 0x0009B944
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		public static void SetSourcePrecept()
		{
			List<Thing> things = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (things.NullOrEmpty<Thing>())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Precept> preceptsListForReading = Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				Precept precept = preceptsListForReading[i];
				Precept_ThingStyle stylePrecept;
				if ((stylePrecept = (precept as Precept_ThingStyle)) != null)
				{
					list.Add(new DebugMenuOption(precept.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						foreach (Thing thing in things)
						{
							thing.StyleSourcePrecept = stylePrecept;
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0009D810 File Offset: 0x0009BA10
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetIdeo()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Ideo> ideosListForReading = Find.IdeoManager.IdeosListForReading;
			for (int i = 0; i < ideosListForReading.Count; i++)
			{
				Ideo ideo = ideosListForReading[i];
				list.Add(new DebugMenuOption(ideo.name, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in UI.MouseCell().GetThingList(Find.CurrentMap).OfType<Pawn>().ToList<Pawn>())
					{
						if (!pawn.RaceProps.Humanlike)
						{
							break;
						}
						pawn.ideo.SetIdeo(ideo);
						DebugActionsUtility.DustPuffFrom(pawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0009D885 File Offset: 0x0009BA85
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddSlave()
		{
			DebugActionsMisc.AddGuest(GuestStatus.Slave);
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x0009D890 File Offset: 0x0009BA90
		[DebugAction("Pawns", "Suppression +10%", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SuppressionPlus10(Pawn p)
		{
			if (p.guest != null && p.IsSlave)
			{
				Need_Suppression need_Suppression = p.needs.TryGetNeed<Need_Suppression>();
				need_Suppression.CurLevel += need_Suppression.MaxLevel * 0.1f;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x0009D8D8 File Offset: 0x0009BAD8
		[DebugAction("Pawns", "Suppression -10%", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SuppressionMinus10(Pawn p)
		{
			if (p.guest != null && p.IsSlave)
			{
				Need_Suppression need_Suppression = p.needs.TryGetNeed<Need_Suppression>();
				need_Suppression.CurLevel -= need_Suppression.MaxLevel * 0.1f;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0009D920 File Offset: 0x0009BB20
		[DebugAction("Pawns", "Clear suppression schedule", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ResetSuppresionSchedule(Pawn p)
		{
			if (p.guest != null && p.IsSlave)
			{
				p.mindState.lastSlaveSuppressedTick = -99999;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0009D948 File Offset: 0x0009BB48
		[DebugAction("Pawns", "Will +1", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void WillPlus1(Pawn p)
		{
			if (p.guest != null && p.IsPrisoner)
			{
				Pawn_GuestTracker guest = p.guest;
				Pawn_GuestTracker guest2 = p.guest;
				float num = guest2.will + 1f;
				guest2.will = num;
				guest.will = Mathf.Max(num, 0f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0009D99C File Offset: 0x0009BB9C
		[DebugAction("Pawns", "Will -1", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void WillMinus1(Pawn p)
		{
			if (p.guest != null && p.IsPrisoner)
			{
				Pawn_GuestTracker guest = p.guest;
				Pawn_GuestTracker guest2 = p.guest;
				float num = guest2.will - 1f;
				guest2.will = num;
				guest.will = Mathf.Max(num, 0f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0009D9EE File Offset: 0x0009BBEE
		[DebugAction("Pawns", "Start slave rebellion (random)", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartSlaveRebellion(Pawn p)
		{
			if (SlaveRebellionUtility.CanParticipateInSlaveRebellion(p) && SlaveRebellionUtility.StartSlaveRebellion(p, false))
			{
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x0009DA07 File Offset: 0x0009BC07
		[DebugAction("Pawns", "Start slave rebellion (aggressive)", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartSlaveRebellionAggressive(Pawn p)
		{
			if (SlaveRebellionUtility.CanParticipateInSlaveRebellion(p) && SlaveRebellionUtility.StartSlaveRebellion(p, true))
			{
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x0009DA20 File Offset: 0x0009BC20
		[DebugAction("Pawns", "Change style", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ChangeStyle(Pawn p)
		{
			if (p.RaceProps.Humanlike && p.story != null)
			{
				Find.WindowStack.Add(new Dialog_StylingStation(p, null));
			}
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x0009DA48 File Offset: 0x0009BC48
		[DebugAction("Pawns", "Request style change", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RequestStyleChange(Pawn p)
		{
			if (p.style != null && p.style.CanDesireLookChange)
			{
				p.style.RequestLookChange();
			}
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x0009DA6C File Offset: 0x0009BC6C
		[DebugAction("General", "Hack 100%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CompleteHack()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				CompHackable compHackable = thing.TryGetComp<CompHackable>();
				if (compHackable != null && !compHackable.IsHacked)
				{
					compHackable.Hack(compHackable.defence, null);
					DebugActionsUtility.DustPuffFrom(thing);
				}
			}
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x0009DAF0 File Offset: 0x0009BCF0
		[DebugAction("General", "Hack +10%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Hack10()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				CompHackable compHackable = thing.TryGetComp<CompHackable>();
				if (compHackable != null && !compHackable.IsHacked)
				{
					compHackable.Hack(compHackable.defence * 0.1f, null);
					DebugActionsUtility.DustPuffFrom(thing);
				}
			}
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x0009DB7C File Offset: 0x0009BD7C
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DarklightAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (intVec.InBounds(currentMap))
				{
					currentMap.debugDrawer.FlashCell(intVec, DarklightUtility.IsDarklightAt(intVec, currentMap) ? 0.5f : 0f, null, 100);
				}
			}
		}
	}
}
