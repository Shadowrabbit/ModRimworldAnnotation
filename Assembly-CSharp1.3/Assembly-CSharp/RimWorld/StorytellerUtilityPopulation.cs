using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C5A RID: 3162
	public static class StorytellerUtilityPopulation
	{
		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x060049DD RID: 18909 RVA: 0x00183EF3 File Offset: 0x001820F3
		private static StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x060049DE RID: 18910 RVA: 0x00186B75 File Offset: 0x00184D75
		public static float PopulationIntent
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulation, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x060049DF RID: 18911 RVA: 0x00186B95 File Offset: 0x00184D95
		public static float PopulationIntentForQuest
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulationIncludingQuests, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x060049E0 RID: 18912 RVA: 0x00186BB5 File Offset: 0x00184DB5
		public static float AdjustedPopulation
		{
			get
			{
				return 0f + (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>() + (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>() * StorytellerUtilityPopulation.PopulationValue_Prisoner + (float)QuestUtility.TotalBorrowedColonistCount();
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x060049E1 RID: 18913 RVA: 0x00186BE4 File Offset: 0x00184DE4
		public static float AdjustedPopulationIncludingQuests
		{
			get
			{
				float num = StorytellerUtilityPopulation.AdjustedPopulation;
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (!questsListForReading[i].Historical && questsListForReading[i].IncreasesPopulation)
					{
						num += 1f;
					}
				}
				return num;
			}
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x00186C38 File Offset: 0x00184E38
		private static float CalculatePopulationIntent(StorytellerDef def, float curPop, float popAdaptation)
		{
			float num = def.populationIntentFactorFromPopCurve.Evaluate(curPop);
			if (num > 0f)
			{
				num *= def.populationIntentFactorFromPopAdaptDaysCurve.Evaluate(popAdaptation);
			}
			return num;
		}

		// Token: 0x060049E3 RID: 18915 RVA: 0x00186C6C File Offset: 0x00184E6C
		public static string DebugReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Population intent: ".PadRight(40) + StorytellerUtilityPopulation.PopulationIntent.ToString("F2"));
			stringBuilder.AppendLine("Population intent for quest: ".PadRight(40) + StorytellerUtilityPopulation.PopulationIntentForQuest.ToString("F2"));
			stringBuilder.AppendLine("Chance random quest increases population: ".PadRight(40) + NaturalRandomQuestChooser.PopulationIncreasingQuestChance().ToStringPercent());
			stringBuilder.AppendLine("Adjusted population: ".PadRight(40) + StorytellerUtilityPopulation.AdjustedPopulation.ToString("F1"));
			stringBuilder.AppendLine("Adjusted population including quests: ".PadRight(40) + StorytellerUtilityPopulation.AdjustedPopulation.ToString("F1"));
			stringBuilder.AppendLine("Pop adaptation days: ".PadRight(40) + Find.StoryWatcher.watcherPopAdaptation.AdaptDays.ToString("F2"));
			return stringBuilder.ToString();
		}

		// Token: 0x060049E4 RID: 18916 RVA: 0x00186D84 File Offset: 0x00184F84
		[DebugOutput]
		public static void PopulationIntents()
		{
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add((float)i);
			}
			List<float> list2 = new List<float>();
			for (int j = 0; j < 40; j += 2)
			{
				list2.Add((float)j);
			}
			DebugTables.MakeTablesDialog<float, float>(list2, (float ds) => "d-" + ds.ToString("F0"), list, (float rv) => rv.ToString("F2"), (float ds, float p) => StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, p, (float)((int)ds)).ToString("F2"), "pop");
		}

		// Token: 0x04002CE9 RID: 11497
		private static float PopulationValue_Prisoner = 0.5f;
	}
}
