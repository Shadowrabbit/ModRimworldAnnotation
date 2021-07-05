using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F52 RID: 8018
	public class QuestNode_GetRandomNegativeGameCondition : QuestNode
	{
		// Token: 0x0600AB1B RID: 43803 RVA: 0x0031E210 File Offset: 0x0031C410
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

		// Token: 0x0600AB1C RID: 43804 RVA: 0x0006FF2D File Offset: 0x0006E12D
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null && this.DoWork(slate);
		}

		// Token: 0x0600AB1D RID: 43805 RVA: 0x0006FF47 File Offset: 0x0006E147
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600AB1E RID: 43806 RVA: 0x0031E350 File Offset: 0x0031C550
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

		// Token: 0x0600AB1F RID: 43807 RVA: 0x0031E474 File Offset: 0x0031C674
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
				if (!Find.Storyteller.difficultyValues.AllowedBy(incidentDef.disabledWhen))
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

		// Token: 0x04007471 RID: 29809
		[NoTranslate]
		public SlateRef<string> storeGameConditionAs;

		// Token: 0x04007472 RID: 29810
		[NoTranslate]
		public SlateRef<string> storeGameConditionDurationAs;

		// Token: 0x04007473 RID: 29811
		[NoTranslate]
		public SlateRef<string> storeGameConditionDifficultyAs;

		// Token: 0x04007474 RID: 29812
		private static List<QuestNode_GetRandomNegativeGameCondition.Option> options;

		// Token: 0x02001F53 RID: 8019
		private struct Option
		{
			// Token: 0x0600AB21 RID: 43809 RVA: 0x0006FF55 File Offset: 0x0006E155
			public Option(GameConditionDef gameCondition, FloatRange durationDaysRange, float difficulty, int challengeRating)
			{
				this.gameCondition = gameCondition;
				this.durationDaysRange = durationDaysRange;
				this.difficulty = difficulty;
				this.challengeRating = challengeRating;
			}

			// Token: 0x04007475 RID: 29813
			public GameConditionDef gameCondition;

			// Token: 0x04007476 RID: 29814
			public FloatRange durationDaysRange;

			// Token: 0x04007477 RID: 29815
			public float difficulty;

			// Token: 0x04007478 RID: 29816
			public int challengeRating;
		}
	}
}
