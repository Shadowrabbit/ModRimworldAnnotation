using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F82 RID: 3970
	public sealed class DifficultyDef : Def
	{
		// Token: 0x04003873 RID: 14451
		[Obsolete]
		public Color drawColor = Color.white;

		// Token: 0x04003874 RID: 14452
		[Obsolete]
		public bool isExtreme;

		// Token: 0x04003875 RID: 14453
		public bool isCustom;

		// Token: 0x04003876 RID: 14454
		[Obsolete]
		public int difficulty = -1;

		// Token: 0x04003877 RID: 14455
		public float threatScale = 1f;

		// Token: 0x04003878 RID: 14456
		public bool allowBigThreats = true;

		// Token: 0x04003879 RID: 14457
		public bool allowIntroThreats = true;

		// Token: 0x0400387A RID: 14458
		public bool allowCaveHives = true;

		// Token: 0x0400387B RID: 14459
		public bool peacefulTemples;

		// Token: 0x0400387C RID: 14460
		public bool allowViolentQuests = true;

		// Token: 0x0400387D RID: 14461
		public bool predatorsHuntHumanlikes = true;

		// Token: 0x0400387E RID: 14462
		public float scariaRotChance;

		// Token: 0x0400387F RID: 14463
		public float colonistMoodOffset;

		// Token: 0x04003880 RID: 14464
		public float tradePriceFactorLoss;

		// Token: 0x04003881 RID: 14465
		public float cropYieldFactor = 1f;

		// Token: 0x04003882 RID: 14466
		public float mineYieldFactor = 1f;

		// Token: 0x04003883 RID: 14467
		public float butcherYieldFactor = 1f;

		// Token: 0x04003884 RID: 14468
		public float researchSpeedFactor = 1f;

		// Token: 0x04003885 RID: 14469
		public float diseaseIntervalFactor = 1f;

		// Token: 0x04003886 RID: 14470
		public float enemyReproductionRateFactor = 1f;

		// Token: 0x04003887 RID: 14471
		public float playerPawnInfectionChanceFactor = 1f;

		// Token: 0x04003888 RID: 14472
		public float manhunterChanceOnDamageFactor = 1f;

		// Token: 0x04003889 RID: 14473
		public float deepDrillInfestationChanceFactor = 1f;

		// Token: 0x0400388A RID: 14474
		public float foodPoisonChanceFactor = 1f;

		// Token: 0x0400388B RID: 14475
		[Obsolete]
		public float threatsGeneratorThreatCountFactor = 1f;

		// Token: 0x0400388C RID: 14476
		public float maintenanceCostFactor = 1f;

		// Token: 0x0400388D RID: 14477
		public float enemyDeathOnDownedChanceFactor = 1f;

		// Token: 0x0400388E RID: 14478
		public float adaptationGrowthRateFactorOverZero = 1f;

		// Token: 0x0400388F RID: 14479
		public float adaptationEffectFactor = 1f;

		// Token: 0x04003890 RID: 14480
		public float questRewardValueFactor = 1f;

		// Token: 0x04003891 RID: 14481
		public float raidLootPointsFactor = 1f;

		// Token: 0x04003892 RID: 14482
		public bool allowTraps = true;

		// Token: 0x04003893 RID: 14483
		public bool allowTurrets = true;

		// Token: 0x04003894 RID: 14484
		public bool allowMortars = true;

		// Token: 0x04003895 RID: 14485
		public bool allowExtremeWeatherIncidents = true;

		// Token: 0x04003896 RID: 14486
		public bool fixedWealthMode;
	}
}
