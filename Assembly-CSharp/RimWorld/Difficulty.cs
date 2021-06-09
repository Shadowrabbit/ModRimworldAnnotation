using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200116D RID: 4461
	public class Difficulty : IExposable
	{
		// Token: 0x06006221 RID: 25121 RVA: 0x001EA9E0 File Offset: 0x001E8BE0
		public Difficulty()
		{
		}

		// Token: 0x06006222 RID: 25122 RVA: 0x001EAB10 File Offset: 0x001E8D10
		public Difficulty(DifficultyDef src)
		{
			this.CopyFrom(src);
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06006223 RID: 25123 RVA: 0x00043833 File Offset: 0x00041A33
		public float EffectiveQuestRewardValueFactor
		{
			get
			{
				return 1f / Mathf.Clamp(this.threatScale, 0.3f, 10f) * this.questRewardValueFactor;
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x06006224 RID: 25124 RVA: 0x00043857 File Offset: 0x00041A57
		public float EffectiveRaidLootPointsFactor
		{
			get
			{
				return 1f / Mathf.Clamp(this.threatScale, 0.3f, 10f) * this.raidLootPointsFactor;
			}
		}

		// Token: 0x06006225 RID: 25125 RVA: 0x001EAC48 File Offset: 0x001E8E48
		public bool AllowedToBuild(BuildableDef def)
		{
			ThingDef thingDef;
			if ((thingDef = (def as ThingDef)) != null)
			{
				if (!this.allowTraps && thingDef.building.isTrap)
				{
					return false;
				}
				if ((!this.allowMortars || !this.allowTurrets) && thingDef.building.IsTurret)
				{
					if (!thingDef.building.IsMortar)
					{
						return this.allowTurrets;
					}
					return this.allowMortars;
				}
			}
			return true;
		}

		// Token: 0x06006226 RID: 25126 RVA: 0x001EACB0 File Offset: 0x001E8EB0
		public bool AllowedBy(DifficultyConditionConfig cfg)
		{
			return cfg == null || ((this.allowBigThreats || !cfg.bigThreatsDisabled) && (this.allowTraps || !cfg.trapsDisabled) && (this.allowTurrets || !cfg.turretsDisabled) && (this.allowMortars || !cfg.mortarsDisabled) && (this.allowExtremeWeatherIncidents || !cfg.extremeWeatherIncidentsDisabled));
		}

		// Token: 0x06006227 RID: 25127 RVA: 0x001EAD20 File Offset: 0x001E8F20
		public void CopyFrom(DifficultyDef src)
		{
			this.threatScale = src.threatScale;
			this.allowBigThreats = src.allowBigThreats;
			this.allowIntroThreats = src.allowIntroThreats;
			this.allowCaveHives = src.allowCaveHives;
			this.peacefulTemples = src.peacefulTemples;
			this.allowViolentQuests = src.allowViolentQuests;
			this.predatorsHuntHumanlikes = src.predatorsHuntHumanlikes;
			this.scariaRotChance = src.scariaRotChance;
			this.colonistMoodOffset = src.colonistMoodOffset;
			this.tradePriceFactorLoss = src.tradePriceFactorLoss;
			this.cropYieldFactor = src.cropYieldFactor;
			this.mineYieldFactor = src.mineYieldFactor;
			this.butcherYieldFactor = src.butcherYieldFactor;
			this.researchSpeedFactor = src.researchSpeedFactor;
			this.diseaseIntervalFactor = src.diseaseIntervalFactor;
			this.enemyReproductionRateFactor = src.enemyReproductionRateFactor;
			this.playerPawnInfectionChanceFactor = src.playerPawnInfectionChanceFactor;
			this.manhunterChanceOnDamageFactor = src.manhunterChanceOnDamageFactor;
			this.deepDrillInfestationChanceFactor = src.deepDrillInfestationChanceFactor;
			this.foodPoisonChanceFactor = src.foodPoisonChanceFactor;
			this.maintenanceCostFactor = src.maintenanceCostFactor;
			this.enemyDeathOnDownedChanceFactor = src.enemyDeathOnDownedChanceFactor;
			this.adaptationGrowthRateFactorOverZero = src.adaptationGrowthRateFactorOverZero;
			this.adaptationEffectFactor = src.adaptationEffectFactor;
			this.questRewardValueFactor = src.questRewardValueFactor;
			this.raidLootPointsFactor = src.raidLootPointsFactor;
			this.allowTraps = src.allowTraps;
			this.allowTurrets = src.allowTurrets;
			this.allowMortars = src.allowMortars;
			this.allowExtremeWeatherIncidents = src.allowExtremeWeatherIncidents;
			this.fixedWealthMode = src.fixedWealthMode;
			this.fixedWealthTimeFactor = 1f;
			this.friendlyFireChanceFactor = 0.4f;
			this.allowInstantKillChance = 1f;
		}

		// Token: 0x06006228 RID: 25128 RVA: 0x001EAEC4 File Offset: 0x001E90C4
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.threatScale, "threatScale", 0f, false);
			Scribe_Values.Look<bool>(ref this.allowBigThreats, "allowBigThreats", false, false);
			Scribe_Values.Look<bool>(ref this.allowIntroThreats, "allowIntroThreats", false, false);
			Scribe_Values.Look<bool>(ref this.allowCaveHives, "allowCaveHives", false, false);
			Scribe_Values.Look<bool>(ref this.peacefulTemples, "peacefulTemples", false, false);
			Scribe_Values.Look<bool>(ref this.allowViolentQuests, "allowViolentQuests", false, false);
			Scribe_Values.Look<bool>(ref this.predatorsHuntHumanlikes, "predatorsHuntHumanlikes", false, false);
			Scribe_Values.Look<float>(ref this.scariaRotChance, "scariaRotChance", 0f, false);
			Scribe_Values.Look<float>(ref this.colonistMoodOffset, "colonistMoodOffset", 0f, false);
			Scribe_Values.Look<float>(ref this.tradePriceFactorLoss, "tradePriceFactorLoss", 0f, false);
			Scribe_Values.Look<float>(ref this.cropYieldFactor, "cropYieldFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.mineYieldFactor, "mineYieldFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.butcherYieldFactor, "butcherYieldFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.researchSpeedFactor, "researchSpeedFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.diseaseIntervalFactor, "diseaseIntervalFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.enemyReproductionRateFactor, "enemyReproductionRateFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.playerPawnInfectionChanceFactor, "playerPawnInfectionChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.manhunterChanceOnDamageFactor, "manhunterChanceOnDamageFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.deepDrillInfestationChanceFactor, "deepDrillInfestationChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.foodPoisonChanceFactor, "foodPoisonChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.maintenanceCostFactor, "maintenanceCostFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.enemyDeathOnDownedChanceFactor, "enemyDeathOnDownedChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.adaptationGrowthRateFactorOverZero, "adaptationGrowthRateFactorOverZero", 0f, false);
			Scribe_Values.Look<float>(ref this.adaptationEffectFactor, "adaptationEffectFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.questRewardValueFactor, "questRewardValueFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.raidLootPointsFactor, "raidLootPointsFactor", 1f, false);
			Scribe_Values.Look<bool>(ref this.allowTraps, "allowTraps", true, false);
			Scribe_Values.Look<bool>(ref this.allowTurrets, "allowTurrets", true, false);
			Scribe_Values.Look<bool>(ref this.allowMortars, "allowMortars", true, false);
			Scribe_Values.Look<bool>(ref this.allowExtremeWeatherIncidents, "allowExtremeWeatherIncidents", true, false);
			Scribe_Values.Look<bool>(ref this.fixedWealthMode, "fixedWealthMode", false, false);
			Scribe_Values.Look<float>(ref this.fixedWealthTimeFactor, "fixedWealthTimeFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.friendlyFireChanceFactor, "friendlyFireChanceFactor", 0.4f, false);
			Scribe_Values.Look<float>(ref this.allowInstantKillChance, "allowInstantKillChance", 1f, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.maintenanceCostFactor = Mathf.Max(0.01f, this.maintenanceCostFactor);
			}
		}

		// Token: 0x06006229 RID: 25129 RVA: 0x001EB1B0 File Offset: 0x001E93B0
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("threatScale".PadRight(40)).Append(this.threatScale).AppendLine();
			stringBuilder.Append("allowBigThreats".PadRight(40)).Append(this.allowBigThreats).AppendLine();
			stringBuilder.Append("allowIntroThreats".PadRight(40)).Append(this.allowIntroThreats).AppendLine();
			stringBuilder.Append("allowCaveHives".PadRight(40)).Append(this.allowCaveHives).AppendLine();
			stringBuilder.Append("peacefulTemples".PadRight(40)).Append(this.peacefulTemples).AppendLine();
			stringBuilder.Append("allowViolentQuests".PadRight(40)).Append(this.allowViolentQuests).AppendLine();
			stringBuilder.Append("predatorsHuntHumanlikes".PadRight(40)).Append(this.predatorsHuntHumanlikes).AppendLine();
			stringBuilder.Append("scariaRotChance".PadRight(40)).Append(this.scariaRotChance).AppendLine();
			stringBuilder.Append("colonistMoodOffset".PadRight(40)).Append(this.colonistMoodOffset).AppendLine();
			stringBuilder.Append("tradePriceFactorLoss".PadRight(40)).Append(this.tradePriceFactorLoss).AppendLine();
			stringBuilder.Append("cropYieldFactor".PadRight(40)).Append(this.cropYieldFactor).AppendLine();
			stringBuilder.Append("mineYieldFactor".PadRight(40)).Append(this.mineYieldFactor).AppendLine();
			stringBuilder.Append("butcherYieldFactor".PadRight(40)).Append(this.butcherYieldFactor).AppendLine();
			stringBuilder.Append("researchSpeedFactor".PadRight(40)).Append(this.researchSpeedFactor).AppendLine();
			stringBuilder.Append("diseaseIntervalFactor".PadRight(40)).Append(this.diseaseIntervalFactor).AppendLine();
			stringBuilder.Append("enemyReproductionRateFactor".PadRight(40)).Append(this.enemyReproductionRateFactor).AppendLine();
			stringBuilder.Append("playerPawnInfectionChanceFactor".PadRight(40)).Append(this.playerPawnInfectionChanceFactor).AppendLine();
			stringBuilder.Append("manhunterChanceOnDamageFactor".PadRight(40)).Append(this.manhunterChanceOnDamageFactor).AppendLine();
			stringBuilder.Append("deepDrillInfestationChanceFactor".PadRight(40)).Append(this.deepDrillInfestationChanceFactor).AppendLine();
			stringBuilder.Append("foodPoisonChanceFactor".PadRight(40)).Append(this.foodPoisonChanceFactor).AppendLine();
			stringBuilder.Append("maintenanceCostFactor".PadRight(40)).Append(this.maintenanceCostFactor).AppendLine();
			stringBuilder.Append("enemyDeathOnDownedChanceFactor".PadRight(40)).Append(this.enemyDeathOnDownedChanceFactor).AppendLine();
			stringBuilder.Append("adaptationGrowthRateFactorOverZero".PadRight(40)).Append(this.adaptationGrowthRateFactorOverZero).AppendLine();
			stringBuilder.Append("adaptationEffectFactor".PadRight(40)).Append(this.adaptationEffectFactor).AppendLine();
			stringBuilder.Append("questRewardValueFactor".PadRight(40)).Append(this.questRewardValueFactor).AppendLine();
			stringBuilder.Append("raidLootPointsFactor".PadRight(40)).Append(this.raidLootPointsFactor).AppendLine();
			stringBuilder.Append("allowTraps".PadRight(40)).Append(this.allowTraps).AppendLine();
			stringBuilder.Append("allowTurrets".PadRight(40)).Append(this.allowTurrets).AppendLine();
			stringBuilder.Append("allowMortars".PadRight(40)).Append(this.allowMortars).AppendLine();
			stringBuilder.Append("allowExtremeWeatherIncidents".PadRight(40)).Append(this.allowExtremeWeatherIncidents).AppendLine();
			stringBuilder.Append("fixedWealthMode".PadRight(40)).Append(this.fixedWealthMode).AppendLine();
			stringBuilder.Append("fixedWealthTimeFactor".PadRight(40)).Append(this.fixedWealthTimeFactor).AppendLine();
			stringBuilder.Append("friendlyFireChanceFactor".PadRight(40)).Append(this.friendlyFireChanceFactor).AppendLine();
			stringBuilder.Append("allowInstantKillChance".PadRight(40)).Append(this.allowInstantKillChance).AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x0600622A RID: 25130 RVA: 0x0004387B File Offset: 0x00041A7B
		[DebugOutput]
		public static void DifficultyDetails()
		{
			Log.Message(Find.Storyteller.difficultyValues.DebugString(), false);
		}

		// Token: 0x040041B0 RID: 16816
		public const float DiseaseIntervalFactorCutoff = 100f;

		// Token: 0x040041B1 RID: 16817
		public const float FriendlyFireDefault = 0.4f;

		// Token: 0x040041B2 RID: 16818
		public const float AllowInstantKillChanceDefault = 1f;

		// Token: 0x040041B3 RID: 16819
		public const float MaintenanceCostFactorMin = 0.01f;

		// Token: 0x040041B4 RID: 16820
		public float threatScale = 1f;

		// Token: 0x040041B5 RID: 16821
		public bool allowBigThreats = true;

		// Token: 0x040041B6 RID: 16822
		public bool allowIntroThreats = true;

		// Token: 0x040041B7 RID: 16823
		public bool allowCaveHives = true;

		// Token: 0x040041B8 RID: 16824
		public bool peacefulTemples;

		// Token: 0x040041B9 RID: 16825
		public bool allowViolentQuests = true;

		// Token: 0x040041BA RID: 16826
		public bool predatorsHuntHumanlikes = true;

		// Token: 0x040041BB RID: 16827
		public float scariaRotChance;

		// Token: 0x040041BC RID: 16828
		public float colonistMoodOffset;

		// Token: 0x040041BD RID: 16829
		public float tradePriceFactorLoss;

		// Token: 0x040041BE RID: 16830
		public float cropYieldFactor = 1f;

		// Token: 0x040041BF RID: 16831
		public float mineYieldFactor = 1f;

		// Token: 0x040041C0 RID: 16832
		public float butcherYieldFactor = 1f;

		// Token: 0x040041C1 RID: 16833
		public float researchSpeedFactor = 1f;

		// Token: 0x040041C2 RID: 16834
		public float diseaseIntervalFactor = 1f;

		// Token: 0x040041C3 RID: 16835
		public float enemyReproductionRateFactor = 1f;

		// Token: 0x040041C4 RID: 16836
		public float playerPawnInfectionChanceFactor = 1f;

		// Token: 0x040041C5 RID: 16837
		public float manhunterChanceOnDamageFactor = 1f;

		// Token: 0x040041C6 RID: 16838
		public float deepDrillInfestationChanceFactor = 1f;

		// Token: 0x040041C7 RID: 16839
		public float foodPoisonChanceFactor = 1f;

		// Token: 0x040041C8 RID: 16840
		public float maintenanceCostFactor = 1f;

		// Token: 0x040041C9 RID: 16841
		public float enemyDeathOnDownedChanceFactor = 1f;

		// Token: 0x040041CA RID: 16842
		public float adaptationGrowthRateFactorOverZero = 1f;

		// Token: 0x040041CB RID: 16843
		public float adaptationEffectFactor = 1f;

		// Token: 0x040041CC RID: 16844
		public float questRewardValueFactor = 1f;

		// Token: 0x040041CD RID: 16845
		public float raidLootPointsFactor = 1f;

		// Token: 0x040041CE RID: 16846
		public bool allowTraps = true;

		// Token: 0x040041CF RID: 16847
		public bool allowTurrets = true;

		// Token: 0x040041D0 RID: 16848
		public bool allowMortars = true;

		// Token: 0x040041D1 RID: 16849
		public bool allowExtremeWeatherIncidents = true;

		// Token: 0x040041D2 RID: 16850
		public bool fixedWealthMode;

		// Token: 0x040041D3 RID: 16851
		public float fixedWealthTimeFactor = 1f;

		// Token: 0x040041D4 RID: 16852
		public float friendlyFireChanceFactor = 0.4f;

		// Token: 0x040041D5 RID: 16853
		public float allowInstantKillChance = 1f;
	}
}
