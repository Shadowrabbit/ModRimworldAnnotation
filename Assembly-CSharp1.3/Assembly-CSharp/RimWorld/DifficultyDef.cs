using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A5D RID: 2653
	public sealed class DifficultyDef : Def
	{
		// Token: 0x04002392 RID: 9106
		[Obsolete]
		public Color drawColor = Color.white;

		// Token: 0x04002393 RID: 9107
		[Obsolete]
		public bool isExtreme;

		// Token: 0x04002394 RID: 9108
		public bool isCustom;

		// Token: 0x04002395 RID: 9109
		[Obsolete]
		public int difficulty = -1;

		// Token: 0x04002396 RID: 9110
		public float threatScale = 1f;

		// Token: 0x04002397 RID: 9111
		public bool allowBigThreats = true;

		// Token: 0x04002398 RID: 9112
		public bool allowIntroThreats = true;

		// Token: 0x04002399 RID: 9113
		public bool allowCaveHives = true;

		// Token: 0x0400239A RID: 9114
		public bool peacefulTemples;

		// Token: 0x0400239B RID: 9115
		public bool allowViolentQuests = true;

		// Token: 0x0400239C RID: 9116
		public bool predatorsHuntHumanlikes = true;

		// Token: 0x0400239D RID: 9117
		public float scariaRotChance;

		// Token: 0x0400239E RID: 9118
		public float colonistMoodOffset;

		// Token: 0x0400239F RID: 9119
		public float tradePriceFactorLoss;

		// Token: 0x040023A0 RID: 9120
		public float cropYieldFactor = 1f;

		// Token: 0x040023A1 RID: 9121
		public float mineYieldFactor = 1f;

		// Token: 0x040023A2 RID: 9122
		public float butcherYieldFactor = 1f;

		// Token: 0x040023A3 RID: 9123
		public float researchSpeedFactor = 1f;

		// Token: 0x040023A4 RID: 9124
		public float diseaseIntervalFactor = 1f;

		// Token: 0x040023A5 RID: 9125
		public float enemyReproductionRateFactor = 1f;

		// Token: 0x040023A6 RID: 9126
		public float playerPawnInfectionChanceFactor = 1f;

		// Token: 0x040023A7 RID: 9127
		public float manhunterChanceOnDamageFactor = 1f;

		// Token: 0x040023A8 RID: 9128
		public float deepDrillInfestationChanceFactor = 1f;

		// Token: 0x040023A9 RID: 9129
		public float foodPoisonChanceFactor = 1f;

		// Token: 0x040023AA RID: 9130
		[Obsolete]
		public float threatsGeneratorThreatCountFactor = 1f;

		// Token: 0x040023AB RID: 9131
		public float maintenanceCostFactor = 1f;

		// Token: 0x040023AC RID: 9132
		public float enemyDeathOnDownedChanceFactor = 1f;

		// Token: 0x040023AD RID: 9133
		public float adaptationGrowthRateFactorOverZero = 1f;

		// Token: 0x040023AE RID: 9134
		public float adaptationEffectFactor = 1f;

		// Token: 0x040023AF RID: 9135
		public float questRewardValueFactor = 1f;

		// Token: 0x040023B0 RID: 9136
		public float raidLootPointsFactor = 1f;

		// Token: 0x040023B1 RID: 9137
		public bool allowTraps = true;

		// Token: 0x040023B2 RID: 9138
		public bool allowTurrets = true;

		// Token: 0x040023B3 RID: 9139
		public bool allowMortars = true;

		// Token: 0x040023B4 RID: 9140
		public bool classicMortars;

		// Token: 0x040023B5 RID: 9141
		public bool allowExtremeWeatherIncidents = true;

		// Token: 0x040023B6 RID: 9142
		public bool fixedWealthMode;
	}
}
