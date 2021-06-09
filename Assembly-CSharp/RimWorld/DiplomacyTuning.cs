using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001586 RID: 5510
	public static class DiplomacyTuning
	{
		// Token: 0x04004EA1 RID: 20129
		public const int MaxGoodwill = 100;

		// Token: 0x04004EA2 RID: 20130
		public const int MinGoodwill = -100;

		// Token: 0x04004EA3 RID: 20131
		public const int BecomeHostileThreshold = -75;

		// Token: 0x04004EA4 RID: 20132
		public const int BecomeNeutralThreshold = 0;

		// Token: 0x04004EA5 RID: 20133
		public const int BecomeAllyThreshold = 75;

		// Token: 0x04004EA6 RID: 20134
		public const int InitialHostileThreshold = -10;

		// Token: 0x04004EA7 RID: 20135
		public const int InitialAllyThreshold = 75;

		// Token: 0x04004EA8 RID: 20136
		public static readonly IntRange ForcedStartingEnemyGoodwillRange = new IntRange(-100, -40);

		// Token: 0x04004EA9 RID: 20137
		public const int MinGoodwillToRequestAICoreQuest = 40;

		// Token: 0x04004EAA RID: 20138
		public const int RequestAICoreQuestSilverCost = 1500;

		// Token: 0x04004EAB RID: 20139
		public static readonly FloatRange RansomFeeMarketValueFactorRange = new FloatRange(1.2f, 2.2f);

		// Token: 0x04004EAC RID: 20140
		public const int Goodwill_NaturalChangeStep = 10;

		// Token: 0x04004EAD RID: 20141
		public const float Goodwill_PerDirectDamageToPawn = -1.3f;

		// Token: 0x04004EAE RID: 20142
		public const float Goodwill_PerDirectDamageToBuilding = -1f;

		// Token: 0x04004EAF RID: 20143
		public const int Goodwill_MemberCrushed_Humanlike = -25;

		// Token: 0x04004EB0 RID: 20144
		public const int Goodwill_MemberCrushed_Animal = -15;

		// Token: 0x04004EB1 RID: 20145
		public const int Goodwill_MemberNeutrallyDied_Humanlike = -5;

		// Token: 0x04004EB2 RID: 20146
		public const int Goodwill_MemberNeutrallyDied_Animal = -3;

		// Token: 0x04004EB3 RID: 20147
		public const int Goodwill_BodyPartRemovalViolation = -70;

		// Token: 0x04004EB4 RID: 20148
		public const int Goodwill_MemberEuthanized = -100;

		// Token: 0x04004EB5 RID: 20149
		public const int Goodwill_AttackedSettlement = -50;

		// Token: 0x04004EB6 RID: 20150
		public const int Goodwill_MilitaryAidRequested = -25;

		// Token: 0x04004EB7 RID: 20151
		public const int Goodwill_TraderRequested = -15;

		// Token: 0x04004EB8 RID: 20152
		public const int Goodwill_MemberStripped = -40;

		// Token: 0x04004EB9 RID: 20153
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

		// Token: 0x04004EBA RID: 20154
		public const float Goodwill_BaseGiftSilverForOneGoodwill = 40f;

		// Token: 0x04004EBB RID: 20155
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

		// Token: 0x04004EBC RID: 20156
		public const float Goodwill_GiftPrisonerOfTheirFactionValueFactor = 2f;

		// Token: 0x04004EBD RID: 20157
		public const float Goodwill_TradedMarketValueforOneGoodwill = 600f;

		// Token: 0x04004EBE RID: 20158
		public const int Goodwill_DestroyedMutualEnemyBase = 20;

		// Token: 0x04004EBF RID: 20159
		public const int Goodwill_MemberExitedMapHealthy = 12;

		// Token: 0x04004EC0 RID: 20160
		public const int Goodwill_MemberExitedMapHealthy_LeaderBonus = 40;

		// Token: 0x04004EC1 RID: 20161
		public const float Goodwill_PerTend = 1f;

		// Token: 0x04004EC2 RID: 20162
		public const int Goodwill_MaxTimesTendedTo = 10;

		// Token: 0x04004EC3 RID: 20163
		public const int Goodwill_QuestTradeRequestCompleted = 12;

		// Token: 0x04004EC4 RID: 20164
		public static readonly IntRange Goodwill_PeaceTalksDisasterRange = new IntRange(-50, -40);

		// Token: 0x04004EC5 RID: 20165
		public static readonly IntRange Goodwill_PeaceTalksBackfireRange = new IntRange(-20, -10);

		// Token: 0x04004EC6 RID: 20166
		public static readonly IntRange Goodwill_PeaceTalksSuccessRange = new IntRange(60, 70);

		// Token: 0x04004EC7 RID: 20167
		public static readonly IntRange Goodwill_PeaceTalksTriumphRange = new IntRange(100, 110);

		// Token: 0x04004EC8 RID: 20168
		public static readonly IntRange RoyalFavor_PeaceTalksSuccessRange = new IntRange(1, 4);

		// Token: 0x04004EC9 RID: 20169
		public const float VisitorGiftChanceBase = 0.25f;

		// Token: 0x04004ECA RID: 20170
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

		// Token: 0x04004ECB RID: 20171
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

		// Token: 0x04004ECC RID: 20172
		public static readonly FloatRange VisitorGiftTotalMarketValueRangeBase = new FloatRange(100f, 500f);

		// Token: 0x04004ECD RID: 20173
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

		// Token: 0x04004ECE RID: 20174
		public static readonly FloatRange RequestedMilitaryAidPointsRange = new FloatRange(800f, 1000f);
	}
}
