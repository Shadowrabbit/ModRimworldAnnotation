using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001039 RID: 4153
	public static class NaturalRandomQuestChooser
	{
		// Token: 0x06005A72 RID: 23154 RVA: 0x0003EACF File Offset: 0x0003CCCF
		public static float PopulationIncreasingQuestChance()
		{
			return QuestTuning.IncreasesPopQuestChanceByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntentForQuest);
		}

		// Token: 0x06005A73 RID: 23155 RVA: 0x001D5614 File Offset: 0x001D3814
		public static QuestScriptDef ChooseNaturalRandomQuest(float points, IIncidentTarget target)
		{
			NaturalRandomQuestChooser.<>c__DisplayClass1_0 CS$<>8__locals1 = new NaturalRandomQuestChooser.<>c__DisplayClass1_0();
			CS$<>8__locals1.points = points;
			CS$<>8__locals1.target = target;
			bool flag = Rand.Chance(NaturalRandomQuestChooser.PopulationIncreasingQuestChance());
			QuestScriptDef result;
			if (CS$<>8__locals1.<ChooseNaturalRandomQuest>g__TryGetQuest|0(flag, out result))
			{
				return result;
			}
			QuestScriptDef result2;
			if (flag && CS$<>8__locals1.<ChooseNaturalRandomQuest>g__TryGetQuest|0(false, out result2))
			{
				return result2;
			}
			Log.Error("Couldn't find any random quest. points=" + CS$<>8__locals1.points, false);
			return null;
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x001D567C File Offset: 0x001D387C
		public static float GetNaturalRandomSelectionWeight(QuestScriptDef quest, float points, StoryState storyState)
		{
			if (quest.rootSelectionWeight <= 0f || points < quest.rootMinPoints || StorytellerUtility.GetProgressScore(storyState.Target) < quest.rootMinProgressScore)
			{
				return 0f;
			}
			float num = quest.rootSelectionWeight;
			if (quest.rootSelectionWeightFactorFromPointsCurve != null)
			{
				num *= quest.rootSelectionWeightFactorFromPointsCurve.Evaluate(points);
			}
			for (int i = 0; i < storyState.RecentRandomQuests.Count; i++)
			{
				if (storyState.RecentRandomQuests[i] == quest)
				{
					switch (i)
					{
					case 0:
						num *= 0.01f;
						break;
					case 1:
						num *= 0.3f;
						break;
					case 2:
						num *= 0.5f;
						break;
					case 3:
						num *= 0.7f;
						break;
					case 4:
						num *= 0.9f;
						break;
					}
				}
			}
			if (!quest.canGiveRoyalFavor && NaturalRandomQuestChooser.<GetNaturalRandomSelectionWeight>g__PlayerWantsRoyalFavorFromAnyFaction|2_0())
			{
				int num2 = (storyState.LastRoyalFavorQuestTick != -1) ? storyState.LastRoyalFavorQuestTick : 0;
				float x = (float)(Find.TickManager.TicksGame - num2) / 60000f;
				num *= QuestTuning.NonFavorQuestSelectionWeightFactorByDaysSinceFavorQuestCurve.Evaluate(x);
			}
			return num;
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x001D578C File Offset: 0x001D398C
		public static float GetNaturalDecreeSelectionWeight(QuestScriptDef quest, StoryState storyState)
		{
			if (quest.decreeSelectionWeight <= 0f)
			{
				return 0f;
			}
			float num = quest.decreeSelectionWeight;
			for (int i = 0; i < storyState.RecentRandomDecrees.Count; i++)
			{
				if (storyState.RecentRandomDecrees[i] == quest)
				{
					switch (i)
					{
					case 0:
						num *= 0.01f;
						break;
					case 1:
						num *= 0.3f;
						break;
					case 2:
						num *= 0.5f;
						break;
					case 3:
						num *= 0.7f;
						break;
					case 4:
						num *= 0.9f;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x001D5828 File Offset: 0x001D3A28
		public static float DebugTotalNaturalRandomSelectionWeight(QuestScriptDef quest, float points, IIncidentTarget target)
		{
			if (!quest.IsRootRandomSelected)
			{
				return 0f;
			}
			if (!quest.CanRun(points))
			{
				return 0f;
			}
			float naturalRandomSelectionWeight = NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(quest, points, target.StoryState);
			float num = QuestTuning.IncreasesPopQuestChanceByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntentForQuest);
			return num * (quest.rootIncreasesPopulation ? naturalRandomSelectionWeight : 0f) + (1f - num) * ((!quest.rootIncreasesPopulation) ? naturalRandomSelectionWeight : 0f);
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x001D589C File Offset: 0x001D3A9C
		[CompilerGenerated]
		internal static bool <GetNaturalRandomSelectionWeight>g__PlayerWantsRoyalFavorFromAnyFaction|2_0()
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].allowRoyalFavorRewards && allFactionsListForReading[i] != Faction.OfPlayer && allFactionsListForReading[i].def.HasRoyalTitles && !allFactionsListForReading[i].temporary)
				{
					return true;
				}
			}
			return false;
		}
	}
}
