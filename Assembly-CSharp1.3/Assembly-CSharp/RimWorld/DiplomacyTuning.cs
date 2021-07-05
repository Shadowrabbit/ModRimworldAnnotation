using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB6 RID: 3766
	public static class DiplomacyTuning
	{
		// Token: 0x040033EE RID: 13294
		public const int MaxGoodwill = 100;

		// Token: 0x040033EF RID: 13295
		public const int MinGoodwill = -100;

		// Token: 0x040033F0 RID: 13296
		public const int NaturalEnemyNaturalGoodwill = -130;

		// Token: 0x040033F1 RID: 13297
		public const int NaturalEnemyInitialGoodwill = -80;

		// Token: 0x040033F2 RID: 13298
		public const int NaturalGoodwillRange = 50;

		// Token: 0x040033F3 RID: 13299
		public const float NaturalGoodwillDailyChange = 0.2f;

		// Token: 0x040033F4 RID: 13300
		public const float GoodwillChangeTowardsNaturalGoodwillFactor = 1.25f;

		// Token: 0x040033F5 RID: 13301
		public const int BecomeHostileThreshold = -75;

		// Token: 0x040033F6 RID: 13302
		public const int BecomeNeutralThreshold = 0;

		// Token: 0x040033F7 RID: 13303
		public const int BecomeAllyThreshold = 75;

		// Token: 0x040033F8 RID: 13304
		public const int InitialHostileThreshold = -10;

		// Token: 0x040033F9 RID: 13305
		public const int InitialAllyThreshold = 75;

		// Token: 0x040033FA RID: 13306
		public static readonly IntRange ForcedStartingEnemyGoodwillRange = new IntRange(-100, -40);

		// Token: 0x040033FB RID: 13307
		public const int MinGoodwillToRequestAICoreQuest = 40;

		// Token: 0x040033FC RID: 13308
		public const int RequestAICoreQuestSilverCost = 1500;

		// Token: 0x040033FD RID: 13309
		public static readonly FloatRange RansomFeeMarketValueFactorRange = new FloatRange(1.2f, 2.2f);

		// Token: 0x040033FE RID: 13310
		public const int Goodwill_NaturalChangeStep = 10;

		// Token: 0x040033FF RID: 13311
		public const float Goodwill_PerDirectDamageToPawn = -1.3f;

		// Token: 0x04003400 RID: 13312
		public const float Goodwill_PerDirectDamageToBuilding = -1f;

		// Token: 0x04003401 RID: 13313
		public const int Goodwill_MemberCrushed_Humanlike = -25;

		// Token: 0x04003402 RID: 13314
		public const int Goodwill_MemberCrushed_Animal = -15;

		// Token: 0x04003403 RID: 13315
		public const int Goodwill_MemberNeutrallyDied_Humanlike = -5;

		// Token: 0x04003404 RID: 13316
		public const int Goodwill_MemberNeutrallyDied_Animal = -3;

		// Token: 0x04003405 RID: 13317
		public const int Goodwill_BodyPartRemovalViolation = -70;

		// Token: 0x04003406 RID: 13318
		public const int Goodwill_MemberEuthanized = -100;

		// Token: 0x04003407 RID: 13319
		public const int Goodwill_AttackedSettlement = -50;

		// Token: 0x04003408 RID: 13320
		public const int Goodwill_MilitaryAidRequested = -25;

		// Token: 0x04003409 RID: 13321
		public const int Goodwill_TraderRequested = -15;

		// Token: 0x0400340A RID: 13322
		public const int Goodwill_MemberStripped = -40;

		// Token: 0x0400340B RID: 13323
		public static readonly SimpleCurve Goodwill_PerQuadrumFromSettlementProximity = new SimpleCurve
		{
			{
				new CurvePoint(2f, -30f),
				true
			},
			{
				new CurvePoint(3f, -20f),
				true
			},
			{
				new CurvePoint(4f, -10f),
				true
			},
			{
				new CurvePoint(5f, 0f),
				true
			}
		};

		// Token: 0x0400340C RID: 13324
		public const float Goodwill_BaseGiftSilverForOneGoodwill = 40f;

		// Token: 0x0400340D RID: 13325
		public static readonly SimpleCurve GiftGoodwillFactorRelationsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(75f, 0.25f),
				true
			}
		};

		// Token: 0x0400340E RID: 13326
		public const float Goodwill_GiftPrisonerOfTheirFactionValueFactor = 2f;

		// Token: 0x0400340F RID: 13327
		public const float Goodwill_TradedMarketValueforOneGoodwill = 600f;

		// Token: 0x04003410 RID: 13328
		public const int Goodwill_DestroyedMutualEnemyBase = 20;

		// Token: 0x04003411 RID: 13329
		public const int Goodwill_MemberExitedMapHealthy = 12;

		// Token: 0x04003412 RID: 13330
		public const int Goodwill_MemberExitedMapHealthy_LeaderBonus = 40;

		// Token: 0x04003413 RID: 13331
		public const float Goodwill_PerTend = 1f;

		// Token: 0x04003414 RID: 13332
		public const int Goodwill_MaxTimesTendedTo = 10;

		// Token: 0x04003415 RID: 13333
		public const int Goodwill_QuestTradeRequestCompleted = 12;

		// Token: 0x04003416 RID: 13334
		public static readonly IntRange Goodwill_PeaceTalksDisasterRange = new IntRange(-50, -40);

		// Token: 0x04003417 RID: 13335
		public static readonly IntRange Goodwill_PeaceTalksBackfireRange = new IntRange(-20, -10);

		// Token: 0x04003418 RID: 13336
		public static readonly IntRange Goodwill_PeaceTalksSuccessRange = new IntRange(60, 70);

		// Token: 0x04003419 RID: 13337
		public static readonly IntRange Goodwill_PeaceTalksTriumphRange = new IntRange(100, 110);

		// Token: 0x0400341A RID: 13338
		public static readonly IntRange RoyalFavor_PeaceTalksSuccessRange = new IntRange(1, 4);

		// Token: 0x0400341B RID: 13339
		public const float VisitorGiftChanceBase = 0.25f;

		// Token: 0x0400341C RID: 13340
		public static readonly SimpleCurve VisitorGiftChanceFactorFromPlayerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(30000f, 1f),
				true
			},
			{
				new CurvePoint(80000f, 0.1f),
				true
			},
			{
				new CurvePoint(300000f, 0f),
				true
			}
		};

		// Token: 0x0400341D RID: 13341
		public static readonly SimpleCurve VisitorGiftChanceFactorFromGoodwillCurve = new SimpleCurve
		{
			{
				new CurvePoint(-30f, 0f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			}
		};

		// Token: 0x0400341E RID: 13342
		public static readonly FloatRange VisitorGiftTotalMarketValueRangeBase = new FloatRange(100f, 500f);

		// Token: 0x0400341F RID: 13343
		public static readonly SimpleCurve VisitorGiftTotalMarketValueFactorFromPlayerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(10000f, 0.25f),
				true
			},
			{
				new CurvePoint(100000f, 1f),
				true
			}
		};

		// Token: 0x04003420 RID: 13344
		public static readonly FloatRange RequestedMilitaryAidPointsRange = new FloatRange(800f, 1000f);
	}
}
