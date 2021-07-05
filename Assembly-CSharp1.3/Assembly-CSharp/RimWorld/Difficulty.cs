using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD6 RID: 3030
	public class Difficulty : IExposable
	{
		// Token: 0x06004723 RID: 18211 RVA: 0x0017873C File Offset: 0x0017693C
		public Difficulty()
		{
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x0017886C File Offset: 0x00176A6C
		public Difficulty(DifficultyDef src)
		{
			this.CopyFrom(src);
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06004725 RID: 18213 RVA: 0x001789A1 File Offset: 0x00176BA1
		public float EffectiveQuestRewardValueFactor
		{
			get
			{
				return 1f / Mathf.Clamp(this.threatScale, 0.3f, 10f) * this.questRewardValueFactor;
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06004726 RID: 18214 RVA: 0x001789C5 File Offset: 0x00176BC5
		public float EffectiveRaidLootPointsFactor
		{
			get
			{
				return 1f / Mathf.Clamp(this.threatScale, 0.3f, 10f) * this.raidLootPointsFactor;
			}
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x001789EC File Offset: 0x00176BEC
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

		// Token: 0x06004728 RID: 18216 RVA: 0x00178A54 File Offset: 0x00176C54
		public bool AllowedBy(DifficultyConditionConfig cfg)
		{
			return cfg == null || ((this.allowBigThreats || !cfg.bigThreatsDisabled) && (this.allowTraps || !cfg.trapsDisabled) && (this.allowTurrets || !cfg.turretsDisabled) && (this.allowMortars || !cfg.mortarsDisabled) && (this.allowExtremeWeatherIncidents || !cfg.extremeWeatherIncidentsDisabled));
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x00178AC4 File Offset: 0x00176CC4
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
			this.classicMortars = src.classicMortars;
			this.allowExtremeWeatherIncidents = src.allowExtremeWeatherIncidents;
			this.fixedWealthMode = src.fixedWealthMode;
			this.fixedWealthTimeFactor = 1f;
			this.friendlyFireChanceFactor = 0.4f;
			this.allowInstantKillChance = 1f;
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x00178C74 File Offset: 0x00176E74
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
			Scribe_Values.Look<bool>(ref this.classicMortars, "classicMortars", true, false);
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

		// Token: 0x0600472B RID: 18219 RVA: 0x00178F74 File Offset: 0x00177174
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
			stringBuilder.Append("classicMortars".PadRight(40)).Append(this.classicMortars).AppendLine();
			stringBuilder.Append("allowExtremeWeatherIncidents".PadRight(40)).Append(this.allowExtremeWeatherIncidents).AppendLine();
			stringBuilder.Append("fixedWealthMode".PadRight(40)).Append(this.fixedWealthMode).AppendLine();
			stringBuilder.Append("fixedWealthTimeFactor".PadRight(40)).Append(this.fixedWealthTimeFactor).AppendLine();
			stringBuilder.Append("friendlyFireChanceFactor".PadRight(40)).Append(this.friendlyFireChanceFactor).AppendLine();
			stringBuilder.Append("allowInstantKillChance".PadRight(40)).Append(this.allowInstantKillChance).AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x00179454 File Offset: 0x00177654
		[DebugOutput]
		public static void DifficultyDetails()
		{
			Log.Message(Find.Storyteller.difficulty.DebugString());
		}

		// Token: 0x04002B9C RID: 11164
		public const float DiseaseIntervalFactorCutoff = 100f;

		// Token: 0x04002B9D RID: 11165
		public const float FriendlyFireDefault = 0.4f;

		// Token: 0x04002B9E RID: 11166
		public const float AllowInstantKillChanceDefault = 1f;

		// Token: 0x04002B9F RID: 11167
		public const float MaintenanceCostFactorMin = 0.01f;

		// Token: 0x04002BA0 RID: 11168
		public float threatScale = 1f;

		// Token: 0x04002BA1 RID: 11169
		public bool allowBigThreats = true;

		// Token: 0x04002BA2 RID: 11170
		public bool allowIntroThreats = true;

		// Token: 0x04002BA3 RID: 11171
		public bool allowCaveHives = true;

		// Token: 0x04002BA4 RID: 11172
		public bool peacefulTemples;

		// Token: 0x04002BA5 RID: 11173
		public bool allowViolentQuests = true;

		// Token: 0x04002BA6 RID: 11174
		public bool predatorsHuntHumanlikes = true;

		// Token: 0x04002BA7 RID: 11175
		public float scariaRotChance;

		// Token: 0x04002BA8 RID: 11176
		public float colonistMoodOffset;

		// Token: 0x04002BA9 RID: 11177
		public float tradePriceFactorLoss;

		// Token: 0x04002BAA RID: 11178
		public float cropYieldFactor = 1f;

		// Token: 0x04002BAB RID: 11179
		public float mineYieldFactor = 1f;

		// Token: 0x04002BAC RID: 11180
		public float butcherYieldFactor = 1f;

		// Token: 0x04002BAD RID: 11181
		public float researchSpeedFactor = 1f;

		// Token: 0x04002BAE RID: 11182
		public float diseaseIntervalFactor = 1f;

		// Token: 0x04002BAF RID: 11183
		public float enemyReproductionRateFactor = 1f;

		// Token: 0x04002BB0 RID: 11184
		public float playerPawnInfectionChanceFactor = 1f;

		// Token: 0x04002BB1 RID: 11185
		public float manhunterChanceOnDamageFactor = 1f;

		// Token: 0x04002BB2 RID: 11186
		public float deepDrillInfestationChanceFactor = 1f;

		// Token: 0x04002BB3 RID: 11187
		public float foodPoisonChanceFactor = 1f;

		// Token: 0x04002BB4 RID: 11188
		public float maintenanceCostFactor = 1f;

		// Token: 0x04002BB5 RID: 11189
		public float enemyDeathOnDownedChanceFactor = 1f;

		// Token: 0x04002BB6 RID: 11190
		public float adaptationGrowthRateFactorOverZero = 1f;

		// Token: 0x04002BB7 RID: 11191
		public float adaptationEffectFactor = 1f;

		// Token: 0x04002BB8 RID: 11192
		public float questRewardValueFactor = 1f;

		// Token: 0x04002BB9 RID: 11193
		public float raidLootPointsFactor = 1f;

		// Token: 0x04002BBA RID: 11194
		public bool allowTraps = true;

		// Token: 0x04002BBB RID: 11195
		public bool allowTurrets = true;

		// Token: 0x04002BBC RID: 11196
		public bool allowMortars = true;

		// Token: 0x04002BBD RID: 11197
		public bool classicMortars;

		// Token: 0x04002BBE RID: 11198
		public bool allowExtremeWeatherIncidents = true;

		// Token: 0x04002BBF RID: 11199
		public bool fixedWealthMode;

		// Token: 0x04002BC0 RID: 11200
		public float fixedWealthTimeFactor = 1f;

		// Token: 0x04002BC1 RID: 11201
		public float friendlyFireChanceFactor = 0.4f;

		// Token: 0x04002BC2 RID: 11202
		public float allowInstantKillChance = 1f;
	}
}
