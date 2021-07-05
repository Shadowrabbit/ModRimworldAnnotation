using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.QuestGen;

namespace Verse
{
	// Token: 0x0200039F RID: 927
	public static class DebugActionsRoyalty
	{
		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001B97 RID: 7063 RVA: 0x000A0F6D File Offset: 0x0009F16D
		private static IEnumerable<Faction> FactionsWithRoyalTitles
		{
			get
			{
				return from f in Find.FactionManager.AllFactions
				where f.def.RoyalTitlesAwardableInSeniorityOrderForReading.Count > 0
				select f;
			}
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000A0F9D File Offset: 0x0009F19D
		private static bool CheckAnyFactionWithRoyalTitles()
		{
			if (!DebugActionsRoyalty.FactionsWithRoyalTitles.Any<Faction>())
			{
				Messages.Message("No factions with royal titles found.", MessageTypeDefOf.RejectInput, false);
				return false;
			}
			return true;
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x000A0FC0 File Offset: 0x0009F1C0
		[DebugAction("General", "Award 4 honor", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Award4RoyalFavor()
		{
			if (!DebugActionsRoyalty.CheckAnyFactionWithRoyalTitles())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction localFaction2 in DebugActionsRoyalty.FactionsWithRoyalTitles)
			{
				Faction localFaction = localFaction2;
				list.Add(new DebugMenuOption(localFaction.Name, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
					if (firstPawn != null)
					{
						firstPawn.royalty.GainFavor(localFaction, 4);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000A1054 File Offset: 0x0009F254
		[DebugAction("General", "Award 10 honor", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Award10RoyalFavor()
		{
			if (!DebugActionsRoyalty.CheckAnyFactionWithRoyalTitles())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction localFaction2 in DebugActionsRoyalty.FactionsWithRoyalTitles)
			{
				Faction localFaction = localFaction2;
				list.Add(new DebugMenuOption(localFaction.Name, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
					if (firstPawn != null)
					{
						firstPawn.royalty.GainFavor(localFaction, 10);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000A10E8 File Offset: 0x0009F2E8
		[DebugAction("General", "Remove 4 honor", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Remove4RoyalFavor()
		{
			if (!DebugActionsRoyalty.CheckAnyFactionWithRoyalTitles())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction localFaction2 in DebugActionsRoyalty.FactionsWithRoyalTitles)
			{
				Faction localFaction = localFaction2;
				list.Add(new DebugMenuOption(localFaction.Name, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
					if (firstPawn != null)
					{
						firstPawn.royalty.TryRemoveFavor(localFaction, 4);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x000A117C File Offset: 0x0009F37C
		[DebugAction("General", "Reduce royal title", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ReduceRoyalTitle()
		{
			if (!DebugActionsRoyalty.CheckAnyFactionWithRoyalTitles())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction localFaction2 in DebugActionsRoyalty.FactionsWithRoyalTitles)
			{
				Faction localFaction = localFaction2;
				list.Add(new DebugMenuOption(localFaction.Name, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
					if (firstPawn != null)
					{
						firstPawn.royalty.ReduceTitle(localFaction);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000A1210 File Offset: 0x0009F410
		[DebugAction("General", "Set royal title", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetTitleForced()
		{
			if (!DebugActionsRoyalty.CheckAnyFactionWithRoyalTitles())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction localFaction2 in DebugActionsRoyalty.FactionsWithRoyalTitles)
			{
				Faction localFaction = localFaction2;
				list.Add(new DebugMenuOption(localFaction.Name, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (RoyalTitleDef localTitleDef2 in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
					{
						RoyalTitleDef localTitleDef = localTitleDef2;
						list2.Add(new DebugMenuOption(localTitleDef.defName, DebugMenuOptionMode.Tool, delegate()
						{
							Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
							if (firstPawn != null)
							{
								firstPawn.royalty.SetTitle(localFaction, localTitleDef, true, false, true);
							}
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x000A12A4 File Offset: 0x0009F4A4
		[DebugOutput]
		private static void RoyalTitles()
		{
			IEnumerable<RoyalTitleDef> allDefsListForReading = DefDatabase<RoyalTitleDef>.AllDefsListForReading;
			TableDataGetter<RoyalTitleDef>[] array = new TableDataGetter<RoyalTitleDef>[10];
			array[0] = new TableDataGetter<RoyalTitleDef>("defName", (RoyalTitleDef title) => title.defName);
			array[1] = new TableDataGetter<RoyalTitleDef>("seniority", (RoyalTitleDef title) => title.seniority);
			array[2] = new TableDataGetter<RoyalTitleDef>("favorCost", (RoyalTitleDef title) => title.favorCost);
			array[3] = new TableDataGetter<RoyalTitleDef>("Awardable", (RoyalTitleDef title) => title.Awardable);
			array[4] = new TableDataGetter<RoyalTitleDef>("minimumExpectationLock", delegate(RoyalTitleDef title)
			{
				if (title.minExpectation != null)
				{
					return title.minExpectation.defName;
				}
				return "NULL";
			});
			array[5] = new TableDataGetter<RoyalTitleDef>("requiredMinimumApparelQuality", delegate(RoyalTitleDef title)
			{
				if (title.requiredMinimumApparelQuality != QualityCategory.Awful)
				{
					return title.requiredMinimumApparelQuality.ToString();
				}
				return "None";
			});
			array[6] = new TableDataGetter<RoyalTitleDef>("requireApparel", delegate(RoyalTitleDef title)
			{
				if (title.requiredApparel != null)
				{
					return string.Join(",\r\n", (from a in title.requiredApparel
					select a.ToString()).ToArray<string>());
				}
				return "NULL";
			});
			array[7] = new TableDataGetter<RoyalTitleDef>("awardThought", delegate(RoyalTitleDef title)
			{
				if (title.awardThought != null)
				{
					return title.awardThought.defName;
				}
				return "NULL";
			});
			array[8] = new TableDataGetter<RoyalTitleDef>("lostThought", delegate(RoyalTitleDef title)
			{
				if (title.lostThought != null)
				{
					return title.lostThought.defName;
				}
				return "NULL";
			});
			array[9] = new TableDataGetter<RoyalTitleDef>("factions", (RoyalTitleDef title) => string.Join(",", (from f in DefDatabase<FactionDef>.AllDefsListForReading
			where f.RoyalTitlesAwardableInSeniorityOrderForReading.Contains(title)
			select f.defName).ToArray<string>()));
			DebugTables.MakeTablesDialog<RoyalTitleDef>(allDefsListForReading, array);
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x000A147C File Offset: 0x0009F67C
		[DebugOutput(name = "Honor Availability (slow)")]
		private static void RoyalFavorAvailability()
		{
			StorytellerCompProperties_OnOffCycle storytellerCompProperties_OnOffCycle = (StorytellerCompProperties_OnOffCycle)StorytellerDefOf.Cassandra.comps.Find(delegate(StorytellerCompProperties x)
			{
				StorytellerCompProperties_OnOffCycle storytellerCompProperties_OnOffCycle2 = x as StorytellerCompProperties_OnOffCycle;
				if (storytellerCompProperties_OnOffCycle2 == null)
				{
					return false;
				}
				if (storytellerCompProperties_OnOffCycle2.IncidentCategory != IncidentCategoryDefOf.GiveQuest)
				{
					return false;
				}
				if (storytellerCompProperties_OnOffCycle2.enableIfAnyModActive != null)
				{
					if (storytellerCompProperties_OnOffCycle2.enableIfAnyModActive.Any((string m) => m.ToLower() == ModContentPack.RoyaltyModPackageId))
					{
						return true;
					}
				}
				return false;
			});
			float onDays = storytellerCompProperties_OnOffCycle.onDays;
			float average = storytellerCompProperties_OnOffCycle.numIncidentsRange.Average;
			float num = average / onDays;
			SimpleCurve simpleCurve = new SimpleCurve
			{
				{
					new CurvePoint(0f, 35f),
					true
				},
				{
					new CurvePoint(15f, 150f),
					true
				},
				{
					new CurvePoint(150f, 5000f),
					true
				}
			};
			int num2 = 0;
			List<RoyalTitleDef> royalTitlesAwardableInSeniorityOrderForReading = FactionDefOf.Empire.RoyalTitlesAwardableInSeniorityOrderForReading;
			for (int i = 0; i < royalTitlesAwardableInSeniorityOrderForReading.Count; i++)
			{
				num2 += royalTitlesAwardableInSeniorityOrderForReading[i].favorCost;
				if (royalTitlesAwardableInSeniorityOrderForReading[i] == RoyalTitleDefOf.Count)
				{
					break;
				}
			}
			float num3 = 0f;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = -1;
			int num9 = -1;
			int num10 = -1;
			int ticksGame = Find.TickManager.TicksGame;
			StoryState storyState = new StoryState(Find.World);
			for (int j = 0; j < 200; j++)
			{
				Find.TickManager.DebugSetTicksGame(j * 60000);
				num3 += num * storytellerCompProperties_OnOffCycle.acceptFractionByDaysPassedCurve.Evaluate((float)j);
				while (num3 >= 1f)
				{
					num3 -= 1f;
					num4++;
					float points = simpleCurve.Evaluate((float)j);
					Slate slate = new Slate();
					slate.Set<float>("points", points, false);
					QuestScriptDef questScriptDef = (from x in DefDatabase<QuestScriptDef>.AllDefsListForReading
					where x.IsRootRandomSelected && x.CanRun(slate)
					select x).RandomElementByWeight((QuestScriptDef x) => NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(x, points, storyState));
					Quest quest = QuestGen.Generate(questScriptDef, slate);
					if (quest.InvolvedFactions.Contains(Faction.OfEmpire))
					{
						num7++;
					}
					QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = (QuestPart_GiveRoyalFavor)quest.PartsListForReading.Find((QuestPart x) => x is QuestPart_GiveRoyalFavor);
					if (questPart_GiveRoyalFavor != null)
					{
						num5 += questPart_GiveRoyalFavor.amount;
						num6++;
						if (num5 >= num2 && num8 < 0)
						{
							num8 = j;
						}
						if (num9 < 0 || questPart_GiveRoyalFavor.amount < num9)
						{
							num9 = questPart_GiveRoyalFavor.amount;
						}
						if (num10 < 0 || questPart_GiveRoyalFavor.amount > num10)
						{
							num10 = questPart_GiveRoyalFavor.amount;
						}
					}
					storyState.RecordRandomQuestFired(questScriptDef);
					quest.CleanupQuestParts();
				}
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Results for: Days=",
				200,
				", intervalDays=",
				onDays,
				", questsPerInterval=",
				average,
				":"
			}));
			stringBuilder.AppendLine("Quests: " + num4);
			stringBuilder.AppendLine("Quests with honor: " + num6);
			stringBuilder.AppendLine("Quests from Empire: " + num7);
			stringBuilder.AppendLine("Min honor reward: " + num9);
			stringBuilder.AppendLine("Max honor reward: " + num10);
			stringBuilder.AppendLine("Total honor: " + num5);
			stringBuilder.AppendLine("Honor required for Count: " + num2);
			stringBuilder.AppendLine("Count title possible on day: " + num8);
			Log.Message(stringBuilder.ToString());
		}
	}
}
