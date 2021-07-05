using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCD RID: 3021
	public static class QuestTuning
	{
		// Token: 0x04002B36 RID: 11062
		public static readonly SimpleCurve IncreasesPopQuestChanceByPopIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 0.3f),
				true
			},
			{
				new CurvePoint(3f, 0.45f),
				true
			}
		};

		// Token: 0x04002B37 RID: 11063
		public const float RecentQuestSelectionWeightFactor0 = 0.01f;

		// Token: 0x04002B38 RID: 11064
		public const float RecentQuestSelectionWeightFactor1 = 0.3f;

		// Token: 0x04002B39 RID: 11065
		public const float RecentQuestSelectionWeightFactor2 = 0.5f;

		// Token: 0x04002B3A RID: 11066
		public const float RecentQuestSelectionWeightFactor3 = 0.7f;

		// Token: 0x04002B3B RID: 11067
		public const float RecentQuestSelectionWeightFactor4 = 0.9f;

		// Token: 0x04002B3C RID: 11068
		public static readonly SimpleCurve NonFavorQuestSelectionWeightFactorByDaysSinceFavorQuestCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 1f),
				true
			},
			{
				new CurvePoint(25f, 0.01f),
				true
			}
		};

		// Token: 0x04002B3D RID: 11069
		public static readonly SimpleCurve PointsToRewardMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(300f, 800f),
				true
			},
			{
				new CurvePoint(700f, 1500f),
				true
			},
			{
				new CurvePoint(5000f, 4000f),
				true
			}
		};

		// Token: 0x04002B3E RID: 11070
		public const int MinFavorAtOnce = 1;

		// Token: 0x04002B3F RID: 11071
		public const int MaxFavorAtOnce = 12;

		// Token: 0x04002B40 RID: 11072
		public const int MaxGoodwillToAllowGoodwillReward = 92;

		// Token: 0x04002B41 RID: 11073
		public static readonly SimpleCurve PopIncreasingRewardWeightByPopIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(3f, 2f),
				true
			}
		};

		// Token: 0x04002B42 RID: 11074
		public const float FutureResearchProjectTechprintSelectionWeightFactor = 0.02f;

		// Token: 0x04002B43 RID: 11075
		public static readonly SimpleCurve DaysSincePsylinkAvailableToGuaranteedNeuroformerChance = new SimpleCurve
		{
			{
				new CurvePoint(45f, 0f),
				true
			},
			{
				new CurvePoint(60f, 1f),
				true
			}
		};

		// Token: 0x04002B44 RID: 11076
		public const float MinDaysBetweenRaidSourceRaids = 1.5f;

		// Token: 0x04002B45 RID: 11077
		public const float RaidSourceRaidThreatPointsFactor = 0.6f;

		// Token: 0x04002B46 RID: 11078
		public static readonly SimpleCurve PointsToRaidSourceRaidsMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 25f),
				true
			},
			{
				new CurvePoint(1500f, 10f),
				true
			},
			{
				new CurvePoint(5000f, 5f),
				true
			}
		};
	}
}
