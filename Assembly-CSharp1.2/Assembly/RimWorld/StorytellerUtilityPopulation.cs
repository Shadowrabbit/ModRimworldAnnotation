using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200124E RID: 4686
	public static class StorytellerUtilityPopulation
	{
		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06006637 RID: 26167 RVA: 0x00045D32 File Offset: 0x00043F32
		private static StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06006638 RID: 26168 RVA: 0x00045D3E File Offset: 0x00043F3E
		public static float PopulationIntent
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulation, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x06006639 RID: 26169 RVA: 0x00045D5E File Offset: 0x00043F5E
		public static float PopulationIntentForQuest
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulationIncludingQuests, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x0600663A RID: 26170 RVA: 0x00045D7E File Offset: 0x00043F7E
		public static float AdjustedPopulation
		{
			get
			{
				return 0f + (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>() + (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>() * StorytellerUtilityPopulation.PopulationValue_Prisoner + (float)QuestUtility.TotalBorrowedColonistCount();
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x0600663B RID: 26171 RVA: 0x001F8CB8 File Offset: 0x001F6EB8
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

		// Token: 0x0600663C RID: 26172 RVA: 0x001F8D0C File Offset: 0x001F6F0C
		private static float CalculatePopulationIntent(StorytellerDef def, float curPop, float popAdaptation)
		{
			float num = def.populationIntentFactorFromPopCurve.Evaluate(curPop);
			if (num > 0f)
			{
				num *= def.populationIntentFactorFromPopAdaptDaysCurve.Evaluate(popAdaptation);
			}
			return num;
		}

		// Token: 0x0600663D RID: 26173 RVA: 0x001F8D40 File Offset: 0x001F6F40
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

		// Token: 0x0600663E RID: 26174 RVA: 0x001F8E58 File Offset: 0x001F7058
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

		// Token: 0x04004418 RID: 17432
		private static float PopulationValue_Prisoner = 0.5f;
	}
}
