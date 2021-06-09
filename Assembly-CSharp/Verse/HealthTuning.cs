using System;

namespace Verse
{
	// Token: 0x020003A0 RID: 928
	public class HealthTuning
	{
		// Token: 0x040011B1 RID: 4529
		public const int StandardInterval = 60;

		// Token: 0x040011B2 RID: 4530
		public const float SmallPawnFragmentedDamageHealthScaleThreshold = 0.5f;

		// Token: 0x040011B3 RID: 4531
		public const int SmallPawnFragmentedDamageMinimumDamageAmount = 10;

		// Token: 0x040011B4 RID: 4532
		public static float ChanceToAdditionallyDamageInnerSolidPart = 0.2f;

		// Token: 0x040011B5 RID: 4533
		public const float MinBleedingRateToBleed = 0.1f;

		// Token: 0x040011B6 RID: 4534
		public const float BleedSeverityRecoveryPerInterval = 0.00033333333f;

		// Token: 0x040011B7 RID: 4535
		public const float BloodFilthDropChanceFactorStanding = 0.004f;

		// Token: 0x040011B8 RID: 4536
		public const float BloodFilthDropChanceFactorLaying = 0.0004f;

		// Token: 0x040011B9 RID: 4537
		public const int BaseTicksAfterInjuryToStopBleeding = 90000;

		// Token: 0x040011BA RID: 4538
		public const int TicksAfterMissingBodyPartToStopBeingFresh = 90000;

		// Token: 0x040011BB RID: 4539
		public const float DefaultPainShockThreshold = 0.8f;

		// Token: 0x040011BC RID: 4540
		public const int InjuryHealInterval = 600;

		// Token: 0x040011BD RID: 4541
		public const float InjuryHealPerDay_Base = 8f;

		// Token: 0x040011BE RID: 4542
		public const float InjuryHealPerDayOffset_Laying = 4f;

		// Token: 0x040011BF RID: 4543
		public const float InjuryHealPerDayOffset_Tended = 8f;

		// Token: 0x040011C0 RID: 4544
		public const int InjurySeverityTendedPerMedicine = 20;

		// Token: 0x040011C1 RID: 4545
		public const float BaseTotalDamageLethalThreshold = 150f;

		// Token: 0x040011C2 RID: 4546
		public const float BecomePermanentBaseChance = 0.02f;

		// Token: 0x040011C3 RID: 4547
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

		// Token: 0x040011C4 RID: 4548
		public static readonly HealthTuning.PainCategoryWeighted[] InjuryPainCategories = new HealthTuning.PainCategoryWeighted[]
		{
			new HealthTuning.PainCategoryWeighted(PainCategory.Painless, 0.5f),
			new HealthTuning.PainCategoryWeighted(PainCategory.LowPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.MediumPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.HighPain, 0.1f)
		};

		// Token: 0x040011C5 RID: 4549
		public const float MinDamagePartPctForInfection = 0.2f;

		// Token: 0x040011C6 RID: 4550
		public static readonly IntRange InfectionDelayRange = new IntRange(15000, 45000);

		// Token: 0x040011C7 RID: 4551
		public const float AnimalsInfectionChanceFactor = 0.1f;

		// Token: 0x040011C8 RID: 4552
		public const float HypothermiaGrowthPerDegreeUnder = 6.45E-05f;

		// Token: 0x040011C9 RID: 4553
		public const float HeatstrokeGrowthPerDegreeOver = 6.45E-05f;

		// Token: 0x040011CA RID: 4554
		public const float MinHeatstrokeProgressPerInterval = 0.000375f;

		// Token: 0x040011CB RID: 4555
		public const float MinHypothermiaProgress = 0.00075f;

		// Token: 0x040011CC RID: 4556
		public const float HarmfulTemperatureOffset = 10f;

		// Token: 0x040011CD RID: 4557
		public const float MinTempOverComfyMaxForBurn = 150f;

		// Token: 0x040011CE RID: 4558
		public const float BurnDamagePerTempOverage = 0.06f;

		// Token: 0x040011CF RID: 4559
		public const int MinBurnDamage = 3;

		// Token: 0x040011D0 RID: 4560
		public const float ImmunityGainRandomFactorMin = 0.8f;

		// Token: 0x040011D1 RID: 4561
		public const float ImmunityGainRandomFactorMax = 1.2f;

		// Token: 0x040011D2 RID: 4562
		public const float ImpossibleToFallSickIfAboveThisImmunityLevel = 0.6f;

		// Token: 0x040011D3 RID: 4563
		public const int HediffGiverUpdateInterval = 60;

		// Token: 0x040011D4 RID: 4564
		public const int VomitCheckInterval = 600;

		// Token: 0x040011D5 RID: 4565
		public const int DeathCheckInterval = 200;

		// Token: 0x040011D6 RID: 4566
		public const int ForgetRandomMemoryThoughtCheckInterval = 400;

		// Token: 0x040011D7 RID: 4567
		public const float PawnBaseHealthForSummary = 75f;

		// Token: 0x040011D8 RID: 4568
		public const float DeathOnDownedChance_NonColonyAnimal = 0.5f;

		// Token: 0x040011D9 RID: 4569
		public const float DeathOnDownedChance_NonColonyMechanoid = 1f;

		// Token: 0x040011DA RID: 4570
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

		// Token: 0x040011DB RID: 4571
		public const float TendPriority_LifeThreateningDisease = 1f;

		// Token: 0x040011DC RID: 4572
		public const float TendPriority_PerBleedRate = 1.5f;

		// Token: 0x040011DD RID: 4573
		public const float TendPriority_DiseaseSeverityDecreasesWhenTended = 0.025f;

		// Token: 0x020003A1 RID: 929
		public struct PainCategoryWeighted
		{
			// Token: 0x0600170F RID: 5903 RVA: 0x000163FC File Offset: 0x000145FC
			public PainCategoryWeighted(PainCategory category, float weight)
			{
				this.category = category;
				this.weight = weight;
			}

			// Token: 0x040011DE RID: 4574
			public PainCategory category;

			// Token: 0x040011DF RID: 4575
			public float weight;
		}
	}
}
