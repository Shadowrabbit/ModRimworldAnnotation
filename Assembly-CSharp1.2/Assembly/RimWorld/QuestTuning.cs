using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200115A RID: 4442
	public static class QuestTuning
	{
		// Token: 0x04004134 RID: 16692
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

		// Token: 0x04004135 RID: 16693
		public const float RecentQuestSelectionWeightFactor0 = 0.01f;

		// Token: 0x04004136 RID: 16694
		public const float RecentQuestSelectionWeightFactor1 = 0.3f;

		// Token: 0x04004137 RID: 16695
		public const float RecentQuestSelectionWeightFactor2 = 0.5f;

		// Token: 0x04004138 RID: 16696
		public const float RecentQuestSelectionWeightFactor3 = 0.7f;

		// Token: 0x04004139 RID: 16697
		public const float RecentQuestSelectionWeightFactor4 = 0.9f;

		// Token: 0x0400413A RID: 16698
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

		// Token: 0x0400413B RID: 16699
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

		// Token: 0x0400413C RID: 16700
		public const int MinFavorAtOnce = 1;

		// Token: 0x0400413D RID: 16701
		public const int MaxFavorAtOnce = 12;

		// Token: 0x0400413E RID: 16702
		public const int MaxGoodwillToAllowGoodwillReward = 92;

		// Token: 0x0400413F RID: 16703
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

		// Token: 0x04004140 RID: 16704
		public const float FutureResearchProjectTechprintSelectionWeightFactor = 0.02f;

		// Token: 0x04004141 RID: 16705
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

		// Token: 0x04004142 RID: 16706
		public const float MinDaysBetweenRaidSourceRaids = 1.5f;

		// Token: 0x04004143 RID: 16707
		public const float RaidSourceRaidThreatPointsFactor = 0.6f;

		// Token: 0x04004144 RID: 16708
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
