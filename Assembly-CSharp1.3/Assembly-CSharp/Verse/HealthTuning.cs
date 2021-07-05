using System;

namespace Verse
{
	// Token: 0x02000276 RID: 630
	public class HealthTuning
	{
		// Token: 0x04000D78 RID: 3448
		public const int StandardInterval = 60;

		// Token: 0x04000D79 RID: 3449
		public const float SmallPawnFragmentedDamageHealthScaleThreshold = 0.5f;

		// Token: 0x04000D7A RID: 3450
		public const int SmallPawnFragmentedDamageMinimumDamageAmount = 10;

		// Token: 0x04000D7B RID: 3451
		public static float ChanceToAdditionallyDamageInnerSolidPart = 0.2f;

		// Token: 0x04000D7C RID: 3452
		public const float MinBleedingRateToBleed = 0.1f;

		// Token: 0x04000D7D RID: 3453
		public const float BleedSeverityRecoveryPerInterval = 0.00033333333f;

		// Token: 0x04000D7E RID: 3454
		public const float BloodFilthDropChanceFactorStanding = 0.004f;

		// Token: 0x04000D7F RID: 3455
		public const float BloodFilthDropChanceFactorLaying = 0.0004f;

		// Token: 0x04000D80 RID: 3456
		public const int BaseTicksAfterInjuryToStopBleeding = 90000;

		// Token: 0x04000D81 RID: 3457
		public const int TicksAfterMissingBodyPartToStopBeingFresh = 90000;

		// Token: 0x04000D82 RID: 3458
		public const float DefaultPainShockThreshold = 0.8f;

		// Token: 0x04000D83 RID: 3459
		public const int InjuryHealInterval = 600;

		// Token: 0x04000D84 RID: 3460
		public const float InjuryHealPerDay_Base = 8f;

		// Token: 0x04000D85 RID: 3461
		public const float InjuryHealPerDayOffset_Laying = 4f;

		// Token: 0x04000D86 RID: 3462
		public const float InjuryHealPerDayOffset_Tended = 8f;

		// Token: 0x04000D87 RID: 3463
		public const int InjurySeverityTendedPerMedicine = 20;

		// Token: 0x04000D88 RID: 3464
		public const float BaseTotalDamageLethalThreshold = 150f;

		// Token: 0x04000D89 RID: 3465
		public const float BecomePermanentBaseChance = 0.02f;

		// Token: 0x04000D8A RID: 3466
		public static readonly SimpleCurve BecomePermanentChanceFactorBySeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(4f, 0f),
				true
			},
			{
				new CurvePoint(14f, 1f),
				true
			}
		};

		// Token: 0x04000D8B RID: 3467
		public static readonly HealthTuning.PainCategoryWeighted[] InjuryPainCategories = new HealthTuning.PainCategoryWeighted[]
		{
			new HealthTuning.PainCategoryWeighted(PainCategory.Painless, 0.5f),
			new HealthTuning.PainCategoryWeighted(PainCategory.LowPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.MediumPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.HighPain, 0.1f)
		};

		// Token: 0x04000D8C RID: 3468
		public const float MinDamagePartPctForInfection = 0.2f;

		// Token: 0x04000D8D RID: 3469
		public static readonly IntRange InfectionDelayRange = new IntRange(15000, 45000);

		// Token: 0x04000D8E RID: 3470
		public const float AnimalsInfectionChanceFactor = 0.1f;

		// Token: 0x04000D8F RID: 3471
		public const float HypothermiaGrowthPerDegreeUnder = 6.45E-05f;

		// Token: 0x04000D90 RID: 3472
		public const float HeatstrokeGrowthPerDegreeOver = 6.45E-05f;

		// Token: 0x04000D91 RID: 3473
		public const float MinHeatstrokeProgressPerInterval = 0.000375f;

		// Token: 0x04000D92 RID: 3474
		public const float MinHypothermiaProgress = 0.00075f;

		// Token: 0x04000D93 RID: 3475
		public const float HarmfulTemperatureOffset = 10f;

		// Token: 0x04000D94 RID: 3476
		public const float MinTempOverComfyMaxForBurn = 150f;

		// Token: 0x04000D95 RID: 3477
		public const float BurnDamagePerTempOverage = 0.06f;

		// Token: 0x04000D96 RID: 3478
		public const int MinBurnDamage = 3;

		// Token: 0x04000D97 RID: 3479
		public const float ImmunityGainRandomFactorMin = 0.8f;

		// Token: 0x04000D98 RID: 3480
		public const float ImmunityGainRandomFactorMax = 1.2f;

		// Token: 0x04000D99 RID: 3481
		public const float ImpossibleToFallSickIfAboveThisImmunityLevel = 0.6f;

		// Token: 0x04000D9A RID: 3482
		public const int HediffGiverUpdateInterval = 60;

		// Token: 0x04000D9B RID: 3483
		public const int VomitCheckInterval = 600;

		// Token: 0x04000D9C RID: 3484
		public const int DeathCheckInterval = 200;

		// Token: 0x04000D9D RID: 3485
		public const int ForgetRandomMemoryThoughtCheckInterval = 400;

		// Token: 0x04000D9E RID: 3486
		public const float PawnBaseHealthForSummary = 75f;

		// Token: 0x04000D9F RID: 3487
		public const float DeathOnDownedChance_NonColonyAnimal = 0.5f;

		// Token: 0x04000DA0 RID: 3488
		public const float DeathOnDownedChance_NonColonyMechanoid = 1f;

		// Token: 0x04000DA1 RID: 3489
		public static readonly SimpleCurve DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.92f),
				true
			},
			{
				new CurvePoint(0f, 0.85f),
				true
			},
			{
				new CurvePoint(1f, 0.62f),
				true
			},
			{
				new CurvePoint(2f, 0.55f),
				true
			},
			{
				new CurvePoint(8f, 0.3f),
				true
			}
		};

		// Token: 0x04000DA2 RID: 3490
		public const float TendPriority_LifeThreateningDisease = 1f;

		// Token: 0x04000DA3 RID: 3491
		public const float TendPriority_PerBleedRate = 1.5f;

		// Token: 0x04000DA4 RID: 3492
		public const float TendPriority_DiseaseSeverityDecreasesWhenTended = 0.025f;

		// Token: 0x020019DD RID: 6621
		public struct PainCategoryWeighted
		{
			// Token: 0x06009A61 RID: 39521 RVA: 0x00363CA8 File Offset: 0x00361EA8
			public PainCategoryWeighted(PainCategory category, float weight)
			{
				this.category = category;
				this.weight = weight;
			}

			// Token: 0x04006326 RID: 25382
			public PainCategory category;

			// Token: 0x04006327 RID: 25383
			public float weight;
		}
	}
}
