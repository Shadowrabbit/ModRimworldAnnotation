using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200168C RID: 5772
	public class QuestNode_GetRandomNegativeGameCondition : QuestNode
	{
		// Token: 0x0600863C RID: 34364 RVA: 0x00302278 File Offset: 0x00300478
		public static void ResetStaticData()
		{
			QuestNode_GetRandomNegativeGameCondition.options = new List<QuestNode_GetRandomNegativeGameCondition.Option>
			{
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.VolcanicWinter, new FloatRange(10f, 20f), 0.4f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.WeatherController, new FloatRange(5f, 20f), 0.4f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.HeatWave, new FloatRange(4f, 8f), 1f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.ColdSnap, new FloatRange(4f, 8f), 1f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.ToxicFallout, new FloatRange(5f, 20f), 0.8f, 2),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.PsychicSuppression, new FloatRange(4f, 8f), 1.5f, 2),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.EMIField, new FloatRange(4f, 8f), 1.8f, 3),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.PsychicDrone, new FloatRange(4f, 8f), 2f, 3)
			};
		}

		// Token: 0x0600863D RID: 34365 RVA: 0x003023B7 File Offset: 0x003005B7
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null && this.DoWork(slate);
		}

		// Token: 0x0600863E RID: 34366 RVA: 0x003023D1 File Offset: 0x003005D1
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600863F RID: 34367 RVA: 0x003023E0 File Offset: 0x003005E0
		private bool DoWork(Slate slate)
		{
			QuestNode_GetRandomNegativeGameCondition.Option option;
			if (QuestGen.Working)
			{
				if (!(from x in QuestNode_GetRandomNegativeGameCondition.options
				where x.challengeRating == QuestGen.quest.challengeRating && this.PossibleNow(x.gameCondition, slate)
				select x).TryRandomElement(out option) && !(from x in QuestNode_GetRandomNegativeGameCondition.options
				where x.challengeRating < QuestGen.quest.challengeRating && this.PossibleNow(x.gameCondition, slate)
				select x).TryRandomElement(out option) && !(from x in QuestNode_GetRandomNegativeGameCondition.options
				where this.PossibleNow(x.gameCondition, slate)
				select x).TryRandomElement(out option))
				{
					return false;
				}
			}
			else if (!(from x in QuestNode_GetRandomNegativeGameCondition.options
			where this.PossibleNow(x.gameCondition, slate)
			select x).TryRandomElement(out option))
			{
				return false;
			}
			int var = (int)(option.durationDaysRange.RandomInRange * 60000f);
			slate.Set<GameConditionDef>(this.storeGameConditionAs.GetValue(slate), option.gameCondition, false);
			slate.Set<int>(this.storeGameConditionDurationAs.GetValue(slate), var, false);
			slate.Set<float>(this.storeGameConditionDifficultyAs.GetValue(slate), option.difficulty, false);
			return true;
		}

		// Token: 0x06008640 RID: 34368 RVA: 0x00302504 File Offset: 0x00300704
		private bool PossibleNow(GameConditionDef def, Slate slate)
		{
			if (def == null)
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			if (map.gameConditionManager.ConditionIsActive(def))
			{
				return false;
			}
			IncidentDef incidentDef = null;
			List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].Worker is IncidentWorker_MakeGameCondition && allDefsListForReading[i].gameCondition == def)
				{
					incidentDef = allDefsListForReading[i];
					break;
				}
			}
			if (incidentDef != null)
			{
				if (!Find.Storyteller.difficulty.AllowedBy(incidentDef.disabledWhen))
				{
					return false;
				}
				if (GenDate.DaysPassed < incidentDef.earliestDay)
				{
					return false;
				}
				if (incidentDef.Worker.FiredTooRecently(map))
				{
					return false;
				}
			}
			return (def != GameConditionDefOf.ColdSnap || IncidentWorker_ColdSnap.IsTemperatureAppropriate(map)) && (def != GameConditionDefOf.HeatWave || IncidentWorker_HeatWave.IsTemperatureAppropriate(map));
		}

		// Token: 0x0400540A RID: 21514
		[NoTranslate]
		public SlateRef<string> storeGameConditionAs;

		// Token: 0x0400540B RID: 21515
		[NoTranslate]
		public SlateRef<string> storeGameConditionDurationAs;

		// Token: 0x0400540C RID: 21516
		[NoTranslate]
		public SlateRef<string> storeGameConditionDifficultyAs;

		// Token: 0x0400540D RID: 21517
		private static List<QuestNode_GetRandomNegativeGameCondition.Option> options;

		// Token: 0x02002933 RID: 10547
		private struct Option
		{
			// Token: 0x0600E0AC RID: 57516 RVA: 0x00421D3A File Offset: 0x0041FF3A
			public Option(GameConditionDef gameCondition, FloatRange durationDaysRange, float difficulty, int challengeRating)
			{
				this.gameCondition = gameCondition;
				this.durationDaysRange = durationDaysRange;
				this.difficulty = difficulty;
				this.challengeRating = challengeRating;
			}

			// Token: 0x04009B1F RID: 39711
			public GameConditionDef gameCondition;

			// Token: 0x04009B20 RID: 39712
			public FloatRange durationDaysRange;

			// Token: 0x04009B21 RID: 39713
			public float difficulty;

			// Token: 0x04009B22 RID: 39714
			public int challengeRating;
		}
	}
}
