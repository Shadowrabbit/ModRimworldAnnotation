using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000EDA RID: 3802
	public static class IdeoUtility
	{
		// Token: 0x06005A4D RID: 23117 RVA: 0x001F37B4 File Offset: 0x001F19B4
		public static void Notify_HistoryEvent(HistoryEvent ev, bool canApplySelfTookThoughts = true)
		{
			IdeoUtility.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.ev = ev;
			if (CS$<>8__locals1.ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out CS$<>8__locals1.pawn))
			{
				if (CS$<>8__locals1.pawn.Ideo != null)
				{
					CS$<>8__locals1.pawn.Ideo.Notify_MemberTookAction(CS$<>8__locals1.ev, canApplySelfTookThoughts);
				}
				if (CS$<>8__locals1.pawn.IsCaravanMember())
				{
					Caravan caravan = CS$<>8__locals1.pawn.GetCaravan();
					for (int i = 0; i < caravan.pawns.Count; i++)
					{
						IdeoUtility.<Notify_HistoryEvent>g__CheckKnows|0_0(caravan.pawns[i], ref CS$<>8__locals1);
					}
					return;
				}
				if (CS$<>8__locals1.pawn.Spawned)
				{
					List<Pawn> allPawnsSpawned = CS$<>8__locals1.pawn.Map.mapPawns.AllPawnsSpawned;
					for (int j = 0; j < allPawnsSpawned.Count; j++)
					{
						IdeoUtility.<Notify_HistoryEvent>g__CheckKnows|0_0(allPawnsSpawned[j], ref CS$<>8__locals1);
					}
					return;
				}
			}
			else
			{
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
				for (int k = 0; k < allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count; k++)
				{
					Pawn pawn = allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[k];
					if (pawn.Ideo != null)
					{
						pawn.Ideo.Notify_MemberKnows(CS$<>8__locals1.ev, pawn);
					}
				}
			}
		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x001F38E4 File Offset: 0x001F1AE4
		public static bool DoerWillingToDo(this HistoryEvent ev)
		{
			Pawn arg = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
			return arg == null || arg.Ideo == null || arg.Ideo.MemberWillingToDo(ev);
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x001F391C File Offset: 0x001F1B1C
		public static bool DoerWillingToDo(HistoryEventDef def, Pawn pawn)
		{
			return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo();
		}

		// Token: 0x06005A50 RID: 23120 RVA: 0x001F3934 File Offset: 0x001F1B34
		public static bool Notify_PawnAboutToDo(this HistoryEvent ev, string messageKey = "MessagePawnUnwillingToDoDueToIdeo")
		{
			if (!ev.DoerWillingToDo())
			{
				Pawn arg = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
				Messages.Message(messageKey.Translate(arg), arg, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			return true;
		}

		// Token: 0x06005A51 RID: 23121 RVA: 0x001F3980 File Offset: 0x001F1B80
		public static bool Notify_PawnAboutToDo(this HistoryEvent ev, out FloatMenuOption opt, string baseText)
		{
			if (!ev.DoerWillingToDo())
			{
				opt = new FloatMenuOption(baseText + ": " + "IdeoligionForbids".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return false;
			}
			opt = null;
			return true;
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x001F39CE File Offset: 0x001F1BCE
		public static bool Notify_PawnAboutToDo_Job(this HistoryEvent ev)
		{
			if (!ev.DoerWillingToDo())
			{
				JobFailReason.Is("IdeoligionForbids".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x001F39F0 File Offset: 0x001F1BF0
		public static List<MemeDef> GenerateRandomMemes(IdeoGenerationParms parms)
		{
			return IdeoUtility.GenerateRandomMemes(IdeoFoundation.MemeCountRangeNPCInitial.RandomInRange, parms);
		}

		// Token: 0x06005A54 RID: 23124 RVA: 0x001F3A10 File Offset: 0x001F1C10
		private static bool CanAdd(MemeDef meme, List<MemeDef> memes, FactionDef forFaction = null)
		{
			if (memes.Contains(meme))
			{
				return false;
			}
			if (forFaction != null && !IdeoUtility.IsMemeAllowedFor(meme, forFaction))
			{
				return false;
			}
			for (int i = 0; i < memes.Count; i++)
			{
				for (int j = 0; j < meme.exclusionTags.Count; j++)
				{
					if (memes[i].exclusionTags.Contains(meme.exclusionTags[j]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005A55 RID: 23125 RVA: 0x001F3A80 File Offset: 0x001F1C80
		public static List<MemeDef> GenerateRandomMemes(int count, IdeoGenerationParms parms)
		{
			FactionDef forFaction = parms.forFaction;
			bool forPlayerFaction = forFaction != null && forFaction.isPlayer;
			List<MemeDef> memes = new List<MemeDef>();
			bool flag = false;
			if (forFaction != null && forFaction.requiredMemes != null)
			{
				for (int i = 0; i < forFaction.requiredMemes.Count; i++)
				{
					memes.Add(forFaction.requiredMemes[i]);
					if (forFaction.requiredMemes[i].category == MemeCategory.Normal)
					{
						count--;
					}
					else if (forFaction.requiredMemes[i].category == MemeCategory.Structure)
					{
						flag = true;
					}
				}
			}
			if (forFaction != null && forFaction.structureMemeWeights != null && !flag)
			{
				MemeWeight memeWeight;
				MemeWeight memeWeight2;
				if ((from x in forFaction.structureMemeWeights
				where IdeoUtility.CanAdd(x.meme, memes, forFaction) && (!IdeoUtility.AnyIdeoHas(x.meme) | forPlayerFaction)
				select x).TryRandomElementByWeight((MemeWeight x) => x.selectionWeight * x.meme.randomizationSelectionWeightFactor, out memeWeight))
				{
					memes.Add(memeWeight.meme);
					flag = true;
				}
				else if ((from x in forFaction.structureMemeWeights
				where IdeoUtility.CanAdd(x.meme, memes, forFaction)
				select x).TryRandomElementByWeight((MemeWeight x) => x.selectionWeight * x.meme.randomizationSelectionWeightFactor, out memeWeight2))
				{
					memes.Add(memeWeight2.meme);
					flag = true;
				}
			}
			if (!flag)
			{
				MemeDef item;
				MemeDef item2;
				if ((from x in DefDatabase<MemeDef>.AllDefs
				where x.category == MemeCategory.Structure && IdeoUtility.CanAdd(x, memes, forFaction) && (!IdeoUtility.AnyIdeoHas(x) | forPlayerFaction)
				select x).TryRandomElement(out item))
				{
					memes.Add(item);
				}
				else if ((from x in DefDatabase<MemeDef>.AllDefs
				where x.category == MemeCategory.Structure && IdeoUtility.CanAdd(x, memes, forFaction)
				select x).TryRandomElementByWeight((MemeDef x) => x.randomizationSelectionWeightFactor, out item2))
				{
					memes.Add(item2);
				}
			}
			Func<MemeDef, bool> <>9__7;
			for (int j = 0; j < count; j++)
			{
				IEnumerable<MemeDef> allDefs = DefDatabase<MemeDef>.AllDefs;
				Func<MemeDef, bool> predicate;
				if ((predicate = <>9__7) == null)
				{
					predicate = (<>9__7 = ((MemeDef x) => x.category == MemeCategory.Normal && IdeoUtility.CanAdd(x, memes, forFaction) && (parms.disallowedMemes == null || !parms.disallowedMemes.Contains(x))));
				}
				MemeDef item3;
				if (allDefs.Where(predicate).TryRandomElementByWeight((MemeDef x) => x.randomizationSelectionWeightFactor, out item3))
				{
					memes.Add(item3);
				}
			}
			return memes;
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x001F3D20 File Offset: 0x001F1F20
		public static List<MemeDef> RandomizeNormalMemes(int count, List<MemeDef> previousMemes, FactionDef forFaction = null)
		{
			List<MemeDef> memes = new List<MemeDef>();
			foreach (MemeDef memeDef in previousMemes)
			{
				if (memeDef.category != MemeCategory.Normal)
				{
					memes.Add(memeDef);
				}
			}
			if (forFaction != null && forFaction.requiredMemes != null)
			{
				for (int i = 0; i < forFaction.requiredMemes.Count; i++)
				{
					MemeDef memeDef2 = forFaction.requiredMemes[i];
					if (memeDef2.category == MemeCategory.Normal)
					{
						memes.Add(memeDef2);
						count--;
					}
				}
			}
			Func<MemeDef, bool> <>9__0;
			for (int j = 0; j < count; j++)
			{
				IEnumerable<MemeDef> allDefs = DefDatabase<MemeDef>.AllDefs;
				Func<MemeDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((MemeDef x) => x.category == MemeCategory.Normal && IdeoUtility.CanAdd(x, memes, forFaction)));
				}
				MemeDef item;
				if (allDefs.Where(predicate).TryRandomElement(out item))
				{
					memes.Add(item);
				}
			}
			return memes;
		}

		// Token: 0x06005A57 RID: 23127 RVA: 0x001F3E48 File Offset: 0x001F2048
		public static List<MemeDef> RandomizeStructureMeme(List<MemeDef> previousMemes, FactionDef forFaction = null)
		{
			List<MemeDef> memes = new List<MemeDef>();
			foreach (MemeDef memeDef in previousMemes)
			{
				if (memeDef.category != MemeCategory.Structure)
				{
					memes.Add(memeDef);
				}
			}
			bool flag = false;
			if (forFaction != null && forFaction.requiredMemes != null)
			{
				for (int i = 0; i < forFaction.requiredMemes.Count; i++)
				{
					MemeDef memeDef2 = forFaction.requiredMemes[i];
					if (memeDef2.category == MemeCategory.Structure)
					{
						memes.Add(memeDef2);
						flag = true;
					}
				}
			}
			MemeDef item;
			if (!flag && (from x in DefDatabase<MemeDef>.AllDefs
			where x.category == MemeCategory.Structure && IdeoUtility.CanAdd(x, memes, forFaction)
			select x).TryRandomElement(out item))
			{
				memes.Add(item);
			}
			return memes;
		}

		// Token: 0x06005A58 RID: 23128 RVA: 0x001F3F58 File Offset: 0x001F2158
		private static bool AnyIdeoHas(MemeDef meme)
		{
			if (Find.World == null)
			{
				return false;
			}
			List<Ideo> ideosListForReading = Find.IdeoManager.IdeosListForReading;
			for (int i = 0; i < ideosListForReading.Count; i++)
			{
				if (ideosListForReading[i].memes.Contains(meme))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005A59 RID: 23129 RVA: 0x001F3FA4 File Offset: 0x001F21A4
		public static bool CanUseIdeo(FactionDef factionDef, Ideo ideo, List<PreceptDef> disallowedPrecepts)
		{
			if (factionDef.allowedCultures != null && !factionDef.allowedCultures.Contains(ideo.culture))
			{
				return false;
			}
			if (factionDef.requiredMemes != null)
			{
				for (int i = 0; i < factionDef.requiredMemes.Count; i++)
				{
					if (!ideo.memes.Contains(factionDef.requiredMemes[i]))
					{
						return false;
					}
				}
			}
			for (int j = 0; j < ideo.memes.Count; j++)
			{
				if (!IdeoUtility.IsMemeAllowedFor(ideo.memes[j], factionDef))
				{
					return false;
				}
			}
			if (!factionDef.isPlayer)
			{
				List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
				for (int k = 0; k < preceptsListForReading.Count; k++)
				{
					if (!preceptsListForReading[k].def.allowedForNPCFactions)
					{
						return false;
					}
				}
			}
			Predicate<PreceptApparelRequirement> <>9__3;
			return (disallowedPrecepts == null || !ideo.PreceptsListForReading.Any((Precept p) => disallowedPrecepts.Contains(p.def))) && !ideo.PreceptsListForReading.OfType<Precept_Ritual>().Any((Precept_Ritual p) => !RitualPatternDef.CanUseWithTechLevel(factionDef.techLevel, p.minTechLevel, p.maxTechLevel)) && !ideo.PreceptsListForReading.OfType<Precept_Role>().Any(delegate(Precept_Role p)
			{
				if (!p.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
				{
					List<PreceptApparelRequirement> apparelRequirements = p.apparelRequirements;
					Predicate<PreceptApparelRequirement> predicate;
					if ((predicate = <>9__3) == null)
					{
						predicate = (<>9__3 = ((PreceptApparelRequirement req) => !req.Compatible(ideo, factionDef)));
					}
					return apparelRequirements.Any(predicate);
				}
				return false;
			});
		}

		// Token: 0x06005A5A RID: 23130 RVA: 0x001F4138 File Offset: 0x001F2338
		public static bool IsMemeAllowedFor(MemeDef meme, FactionDef faction)
		{
			return (faction.structureMemeWeights != null && meme.category == MemeCategory.Structure && faction.structureMemeWeights.Any((MemeWeight x) => x.meme == meme && x.selectionWeight != 0f)) || ((meme.category != MemeCategory.Normal || meme.allowDuringTutorial || !faction.classicIdeo) && (faction.disallowedMemes == null || !faction.disallowedMemes.Contains(meme)) && ((faction.requiredMemes != null && faction.requiredMemes.Contains(meme)) || faction.allowedMemes == null || faction.allowedMemes.Contains(meme)));
		}

		// Token: 0x06005A5B RID: 23131 RVA: 0x001F4200 File Offset: 0x001F2400
		public static bool PlayerHasPreceptForBuilding(ThingDef buildingDef)
		{
			using (IEnumerator<Ideo> enumerator = Faction.OfPlayer.ideos.AllIdeos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasPreceptForBuilding(buildingDef))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005A5C RID: 23132 RVA: 0x001F4260 File Offset: 0x001F2460
		public static IEnumerable<Pawn> AllColonistsWithCharityPrecept()
		{
			List<Pawn> colonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				List<Precept> precepts = ideo.PreceptsListForReading;
				int num;
				for (int i = 0; i < precepts.Count; i = num + 1)
				{
					if (precepts[i].def.issue == IssueDefOf.Charity)
					{
						for (int j = 0; j < colonists.Count; j = num + 1)
						{
							if (colonists[j].Ideo == ideo)
							{
								yield return colonists[j];
							}
							num = j;
						}
					}
					num = i;
				}
				precepts = null;
				ideo = null;
			}
			IEnumerator<Ideo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005A5D RID: 23133 RVA: 0x001F426C File Offset: 0x001F246C
		public static bool AnyColonistWithRanchingIssue()
		{
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
				for (int i = 0; i < preceptsListForReading.Count; i++)
				{
					if (preceptsListForReading[i].def.issue == IssueDefOf.Ranching)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005A5E RID: 23134 RVA: 0x001F42F0 File Offset: 0x001F24F0
		public static void Notify_PlayerRaidedSomeone(IEnumerable<Pawn> allRaiders)
		{
			if (allRaiders.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			allRaiders = from p in allRaiders
			where !p.Dead
			select p;
			Find.History.Notify_PlayerRaidedSomeone();
			if (ModsConfig.IdeologyActive)
			{
				foreach (Pawn arg in allRaiders)
				{
					HistoryEvent historyEvent = new HistoryEvent(HistoryEventDefOf.Raided, arg.Named(HistoryEventArgsNames.Doer));
					Find.HistoryEventsManager.RecordEvent(historyEvent, true);
				}
			}
		}

		// Token: 0x06005A5F RID: 23135 RVA: 0x001F4398 File Offset: 0x001F2598
		public static void Notify_QuestCleanedUp(Quest quest, QuestState state)
		{
			if (state == QuestState.EndedOfferExpired || state == QuestState.EndedFailed)
			{
				if (quest.root.failedOrExpiredHistoryEvent != null)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(quest.root.failedOrExpiredHistoryEvent), true);
				}
				if (quest.charity && ModsConfig.IdeologyActive && IdeoUtility.AllColonistsWithCharityPrecept().Any<Pawn>())
				{
					Messages.Message(string.Format("{0}: {1}", "MessageCharityEventRefused".Translate(), "MessageCharityQuestEndedFailed".Translate(quest.name)), null, MessageTypeDefOf.NegativeEvent, quest, true);
					return;
				}
			}
			else if (state == QuestState.EndedSuccess)
			{
				if (quest.root.successHistoryEvent != null)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(quest.root.successHistoryEvent), true);
				}
				if (quest.charity && ModsConfig.IdeologyActive && IdeoUtility.AllColonistsWithCharityPrecept().Any<Pawn>())
				{
					Messages.Message(string.Format("{0}: {1}", "MessageCharityEventFulfilled".Translate(), "MessageCharityQuestEndedSuccess".Translate(quest.name)), null, MessageTypeDefOf.PositiveEvent, quest, true);
				}
			}
		}

		// Token: 0x06005A60 RID: 23136 RVA: 0x001F44C8 File Offset: 0x001F26C8
		public static float GetStyleDominanceFromCellsCenteredOn(IntVec3 center, IntVec3 rootCell, Map map, Ideo ideo)
		{
			IdeoUtility.<>c__DisplayClass20_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			bool flag = false;
			float num = 0f;
			IdeoUtility.GetJoinedRooms(rootCell, CS$<>8__locals1.map, IdeoUtility.tmpCheckRooms);
			if (!IdeoUtility.tmpCheckRooms.Any<Room>())
			{
				return num;
			}
			int num2 = GenRadial.NumCellsInRadius(24.9f);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 c = center + GenRadial.RadialPattern[i];
				if (IdeoUtility.<GetStyleDominanceFromCellsCenteredOn>g__CellIsValid|20_0(c, ref CS$<>8__locals1))
				{
					if (flag)
					{
						CS$<>8__locals1.map.debugDrawer.FlashCell(c, 0.1f, "d", 50);
					}
					TerrainDef terrain = c.GetTerrain(CS$<>8__locals1.map);
					if (ideo.cachedPossibleBuildables.Contains(terrain))
					{
						num += terrain.GetStatValueAbstract(StatDefOf.StyleDominance, null);
					}
					else if (!terrain.canGenerateDefaultDesignator)
					{
						num -= terrain.GetStatValueAbstract(StatDefOf.StyleDominance, null);
					}
					foreach (Thing t in c.GetThingList(CS$<>8__locals1.map))
					{
						num += IdeoUtility.GetStyleDominance(t, ideo);
					}
				}
			}
			IdeoUtility.tmpCheckRooms.Clear();
			return num;
		}

		// Token: 0x06005A61 RID: 23137 RVA: 0x001F4610 File Offset: 0x001F2810
		private static void GetJoinedRooms(IntVec3 rootCell, Map map, HashSet<Room> rooms)
		{
			Room room = rootCell.GetRoom(map);
			if (room == null)
			{
				return;
			}
			rooms.Clear();
			rooms.Add(room);
			if (room.IsDoorway)
			{
				foreach (Region region in room.FirstRegion.Neighbors)
				{
					if (!rooms.Contains(region.Room))
					{
						rooms.Add(region.Room);
					}
				}
			}
		}

		// Token: 0x06005A62 RID: 23138 RVA: 0x001F4698 File Offset: 0x001F2898
		public static bool ThingSatisfiesIdeo(Thing thing, Ideo ideo)
		{
			return IdeoUtility.GetStyleDominance(thing, ideo) > 0f;
		}

		// Token: 0x06005A63 RID: 23139 RVA: 0x001F46A8 File Offset: 0x001F28A8
		private static float GetStyleDominance(Thing t, Ideo ideo)
		{
			float statValue = t.GetStatValue(StatDefOf.StyleDominance, true);
			CompRelicContainer compRelicContainer = t.TryGetComp<CompRelicContainer>();
			if (compRelicContainer != null && compRelicContainer.Full)
			{
				CompStyleable compStyleable = compRelicContainer.ContainedThing.TryGetComp<CompStyleable>();
				bool flag;
				if (compStyleable == null)
				{
					flag = (null != null);
				}
				else
				{
					Precept_ThingStyle sourcePrecept = compStyleable.SourcePrecept;
					flag = (((sourcePrecept != null) ? sourcePrecept.ideo : null) != null);
				}
				if (flag)
				{
					if (compStyleable.SourcePrecept.ideo == ideo)
					{
						return statValue;
					}
					return -statValue;
				}
			}
			ThingStyleDef styleDef = t.GetStyleDef();
			if (styleDef != null)
			{
				if (ideo.GetStyleFor(t.def) == styleDef)
				{
					return statValue;
				}
				return -statValue;
			}
			else
			{
				if (t.def.canGenerateDefaultDesignator)
				{
					return 0f;
				}
				if (ideo.cachedPossibleBuildables.Contains(t.def) || (t.StyleSourcePrecept != null && ideo.PreceptsListForReading.Contains(t.StyleSourcePrecept)))
				{
					return statValue;
				}
				return -statValue;
			}
		}

		// Token: 0x06005A64 RID: 23140 RVA: 0x001F4770 File Offset: 0x001F2970
		public static List<TreeSighting> TreeSightingsNearPawn(IntVec3 rootCell, Map map, Ideo ideo)
		{
			IdeoUtility.tmpTreeSightings.Clear();
			int num = GenRadial.NumCellsInRadius(11.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = rootCell + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && !intVec.Fogged(map) && GenSight.LineOfSight(rootCell, intVec, map, false, null, 0, 0))
				{
					foreach (Thing thing in intVec.GetThingList(map))
					{
						Plant plant;
						if ((plant = (thing as Plant)) != null && plant.def.plant.IsTree)
						{
							IdeoUtility.tmpTreeSightings.Add(new TreeSighting(thing, Find.TickManager.TicksGame));
						}
					}
				}
			}
			return IdeoUtility.tmpTreeSightings;
		}

		// Token: 0x06005A65 RID: 23141 RVA: 0x001F485C File Offset: 0x001F2A5C
		public static bool IdeoCausesHumanMeatCravings(this Ideo ideo)
		{
			return ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.NoRecentHumanMeat_Preferred) || ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.NoRecentHumanMeat_RequiredStrong) || ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.NoRecentHumanMeat_RequiredRavenous);
		}

		// Token: 0x06005A66 RID: 23142 RVA: 0x001F4894 File Offset: 0x001F2A94
		public static bool IdeoPrefersNudity(this Ideo ideo)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.prefersNudity)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005A67 RID: 23143 RVA: 0x001F48F4 File Offset: 0x001F2AF4
		public static bool IdeoPrefersNudityForGender(this Ideo ideo, Gender gender)
		{
			foreach (Precept precept in ideo.PreceptsListForReading)
			{
				if (precept.def.prefersNudity && (precept.def.genderPrefersNudity == Gender.None || precept.def.genderPrefersNudity == gender))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005A68 RID: 23144 RVA: 0x001F4970 File Offset: 0x001F2B70
		public static bool IdeoApprovesOfSlavery(this Ideo ideo)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.approvesOfSlavery)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005A69 RID: 23145 RVA: 0x001F49D0 File Offset: 0x001F2BD0
		public static bool IdeoPrefersDarkness(this Ideo ideo)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.prefersDarkness)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005A6A RID: 23146 RVA: 0x001F4A30 File Offset: 0x001F2C30
		public static bool IdeoDisablesCrampedRoomThoughts(this Ideo ideo)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.disableCrampedRoomThoughts)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005A6B RID: 23147 RVA: 0x001F4A90 File Offset: 0x001F2C90
		public static bool IdeoApprovesOfBlindness(this Ideo ideo)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.approvesOfBlindness)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005A6C RID: 23148 RVA: 0x001F4AF0 File Offset: 0x001F2CF0
		public static void Notify_NewColonyStarted()
		{
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				ideo.relicsCollected = false;
			}
			foreach (Ideo ideo2 in Faction.OfPlayer.ideos.AllIdeos)
			{
				using (List<Precept>.Enumerator enumerator2 = ideo2.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Precept_Relic precept_Relic;
						if ((precept_Relic = (enumerator2.Current as Precept_Relic)) != null)
						{
							precept_Relic.Notify_NewColonyStarted();
						}
					}
				}
			}
		}

		// Token: 0x06005A6D RID: 23149 RVA: 0x001F4BCC File Offset: 0x001F2DCC
		public static Pawn FindFirstPawnWithLeaderRole(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				if (pawn.Ideo != null)
				{
					Precept_Role role = pawn.Ideo.GetRole(pawn);
					if (role != null && role.def.leaderRole)
					{
						return pawn;
					}
				}
			}
			return null;
		}

		// Token: 0x06005A6E RID: 23150 RVA: 0x001F4C21 File Offset: 0x001F2E21
		public static Ideo MakeEmptyIdeo()
		{
			Ideo ideo = IdeoGenerator.MakeIdeo(DefDatabase<IdeoFoundationDef>.AllDefs.RandomElement<IdeoFoundationDef>());
			ideo.foundation.RandomizeIcon();
			return ideo;
		}

		// Token: 0x06005A6F RID: 23151 RVA: 0x001F4C40 File Offset: 0x001F2E40
		public static Color? GetIdeoColorForBuilding(ThingDef def, Faction faction)
		{
			if (def.building.useIdeoColor && faction != null && faction.ideos != null)
			{
				Ideo ideo = null;
				Predicate<ThingStyleCategoryWithPriority> <>9__0;
				foreach (Ideo ideo2 in faction.ideos.AllIdeos)
				{
					if (def.dominantStyleCategory != null)
					{
						List<ThingStyleCategoryWithPriority> thingStyleCategories = ideo2.thingStyleCategories;
						Predicate<ThingStyleCategoryWithPriority> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((ThingStyleCategoryWithPriority sc) => sc.category == def.dominantStyleCategory));
						}
						if (thingStyleCategories.Any(predicate))
						{
							ideo = ideo2;
							break;
						}
					}
					using (List<MemeDef>.Enumerator enumerator2 = ideo2.memes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.AllDesignatorBuildables.Contains(def))
							{
								ideo = ideo2;
								break;
							}
						}
					}
					if (ideo != null)
					{
						break;
					}
				}
				ideo = (ideo ?? faction.ideos.PrimaryIdeo);
				if (ideo != null)
				{
					return new Color?(ideo.Color);
				}
			}
			return null;
		}

		// Token: 0x06005A71 RID: 23153 RVA: 0x001F4D9E File Offset: 0x001F2F9E
		[CompilerGenerated]
		internal static void <Notify_HistoryEvent>g__CheckKnows|0_0(Pawn p, ref IdeoUtility.<>c__DisplayClass0_0 A_1)
		{
			if (p != A_1.pawn && p.Ideo != null)
			{
				p.Ideo.Notify_MemberKnows(A_1.ev, p);
			}
		}

		// Token: 0x06005A72 RID: 23154 RVA: 0x001F4DC4 File Offset: 0x001F2FC4
		[CompilerGenerated]
		internal static bool <GetStyleDominanceFromCellsCenteredOn>g__CellIsValid|20_0(IntVec3 c, ref IdeoUtility.<>c__DisplayClass20_0 A_1)
		{
			if (!c.InBounds(A_1.map) || c.Fogged(A_1.map))
			{
				return false;
			}
			Room room = c.GetRoom(A_1.map);
			return room != null && IdeoUtility.tmpCheckRooms.Contains(room);
		}

		// Token: 0x040034F9 RID: 13561
		private static HashSet<Room> tmpCheckRooms = new HashSet<Room>();

		// Token: 0x040034FA RID: 13562
		private static List<TreeSighting> tmpTreeSightings = new List<TreeSighting>();
	}
}
