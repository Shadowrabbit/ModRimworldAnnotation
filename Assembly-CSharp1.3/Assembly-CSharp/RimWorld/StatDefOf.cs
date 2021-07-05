using System;

namespace RimWorld
{
	// Token: 0x0200142E RID: 5166
	[DefOf]
	public static class StatDefOf
	{
		// Token: 0x06007D21 RID: 32033 RVA: 0x002C484D File Offset: 0x002C2A4D
		static StatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StatDefOf));
		}

		// Token: 0x04004AB3 RID: 19123
		public static StatDef MaxHitPoints;

		// Token: 0x04004AB4 RID: 19124
		public static StatDef MarketValue;

		// Token: 0x04004AB5 RID: 19125
		public static StatDef MarketValueIgnoreHp;

		// Token: 0x04004AB6 RID: 19126
		public static StatDef RoyalFavorValue;

		// Token: 0x04004AB7 RID: 19127
		public static StatDef SellPriceFactor;

		// Token: 0x04004AB8 RID: 19128
		public static StatDef Beauty;

		// Token: 0x04004AB9 RID: 19129
		public static StatDef Cleanliness;

		// Token: 0x04004ABA RID: 19130
		public static StatDef Flammability;

		// Token: 0x04004ABB RID: 19131
		public static StatDef DeteriorationRate;

		// Token: 0x04004ABC RID: 19132
		public static StatDef WorkToMake;

		// Token: 0x04004ABD RID: 19133
		public static StatDef WorkToBuild;

		// Token: 0x04004ABE RID: 19134
		public static StatDef Mass;

		// Token: 0x04004ABF RID: 19135
		public static StatDef ConstructionSpeedFactor;

		// Token: 0x04004AC0 RID: 19136
		public static StatDef Nutrition;

		// Token: 0x04004AC1 RID: 19137
		public static StatDef FoodPoisonChanceFixedHuman;

		// Token: 0x04004AC2 RID: 19138
		public static StatDef MoveSpeed;

		// Token: 0x04004AC3 RID: 19139
		public static StatDef GlobalLearningFactor;

		// Token: 0x04004AC4 RID: 19140
		public static StatDef HungerRateMultiplier;

		// Token: 0x04004AC5 RID: 19141
		public static StatDef RestRateMultiplier;

		// Token: 0x04004AC6 RID: 19142
		public static StatDef PsychicSensitivity;

		// Token: 0x04004AC7 RID: 19143
		public static StatDef ToxicSensitivity;

		// Token: 0x04004AC8 RID: 19144
		public static StatDef MentalBreakThreshold;

		// Token: 0x04004AC9 RID: 19145
		public static StatDef EatingSpeed;

		// Token: 0x04004ACA RID: 19146
		public static StatDef ComfyTemperatureMin;

		// Token: 0x04004ACB RID: 19147
		public static StatDef ComfyTemperatureMax;

		// Token: 0x04004ACC RID: 19148
		public static StatDef Comfort;

		// Token: 0x04004ACD RID: 19149
		public static StatDef MeatAmount;

		// Token: 0x04004ACE RID: 19150
		public static StatDef LeatherAmount;

		// Token: 0x04004ACF RID: 19151
		public static StatDef MinimumHandlingSkill;

		// Token: 0x04004AD0 RID: 19152
		public static StatDef MeleeDPS;

		// Token: 0x04004AD1 RID: 19153
		public static StatDef PainShockThreshold;

		// Token: 0x04004AD2 RID: 19154
		public static StatDef ForagedNutritionPerDay;

		// Token: 0x04004AD3 RID: 19155
		public static StatDef FilthRate;

		// Token: 0x04004AD4 RID: 19156
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyMax;

		// Token: 0x04004AD5 RID: 19157
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyRecoveryRate;

		// Token: 0x04004AD6 RID: 19158
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyGain;

		// Token: 0x04004AD7 RID: 19159
		[MayRequireRoyalty]
		public static StatDef MeditationFocusGain;

		// Token: 0x04004AD8 RID: 19160
		[MayRequireIdeology]
		public static StatDef CertaintyLossFactor;

		// Token: 0x04004AD9 RID: 19161
		[MayRequireIdeology]
		public static StatDef SocialIdeoSpreadFrequencyFactor;

		// Token: 0x04004ADA RID: 19162
		[MayRequireIdeology]
		public static StatDef SuppressionPower;

		// Token: 0x04004ADB RID: 19163
		[MayRequireIdeology]
		public static StatDef SlaveSuppressionFallRate;

		// Token: 0x04004ADC RID: 19164
		public static StatDef AnimalsLearningFactor;

		// Token: 0x04004ADD RID: 19165
		public static StatDef BondAnimalChanceFactor;

		// Token: 0x04004ADE RID: 19166
		public static StatDef CaravanRidingSpeedFactor;

		// Token: 0x04004ADF RID: 19167
		public static StatDef WorkSpeedGlobal;

		// Token: 0x04004AE0 RID: 19168
		public static StatDef MiningSpeed;

		// Token: 0x04004AE1 RID: 19169
		public static StatDef DeepDrillingSpeed;

		// Token: 0x04004AE2 RID: 19170
		public static StatDef MiningYield;

		// Token: 0x04004AE3 RID: 19171
		public static StatDef ResearchSpeed;

		// Token: 0x04004AE4 RID: 19172
		public static StatDef ConstructionSpeed;

		// Token: 0x04004AE5 RID: 19173
		public static StatDef HuntingStealth;

		// Token: 0x04004AE6 RID: 19174
		public static StatDef PlantWorkSpeed;

		// Token: 0x04004AE7 RID: 19175
		public static StatDef SmoothingSpeed;

		// Token: 0x04004AE8 RID: 19176
		public static StatDef FoodPoisonChance;

		// Token: 0x04004AE9 RID: 19177
		public static StatDef CarryingCapacity;

		// Token: 0x04004AEA RID: 19178
		public static StatDef PlantHarvestYield;

		// Token: 0x04004AEB RID: 19179
		public static StatDef DrugHarvestYield;

		// Token: 0x04004AEC RID: 19180
		public static StatDef FixBrokenDownBuildingSuccessChance;

		// Token: 0x04004AED RID: 19181
		public static StatDef ConstructSuccessChance;

		// Token: 0x04004AEE RID: 19182
		public static StatDef GeneralLaborSpeed;

		// Token: 0x04004AEF RID: 19183
		[MayRequireIdeology]
		public static StatDef HackingSpeed;

		// Token: 0x04004AF0 RID: 19184
		public static StatDef MedicalTendSpeed;

		// Token: 0x04004AF1 RID: 19185
		public static StatDef MedicalTendQuality;

		// Token: 0x04004AF2 RID: 19186
		public static StatDef MedicalSurgerySuccessChance;

		// Token: 0x04004AF3 RID: 19187
		public static StatDef NegotiationAbility;

		// Token: 0x04004AF4 RID: 19188
		public static StatDef ArrestSuccessChance;

		// Token: 0x04004AF5 RID: 19189
		public static StatDef TradePriceImprovement;

		// Token: 0x04004AF6 RID: 19190
		public static StatDef DrugSellPriceImprovement;

		// Token: 0x04004AF7 RID: 19191
		public static StatDef SocialImpact;

		// Token: 0x04004AF8 RID: 19192
		public static StatDef PawnBeauty;

		// Token: 0x04004AF9 RID: 19193
		[MayRequireIdeology]
		public static StatDef AnimalProductsSellImprovement;

		// Token: 0x04004AFA RID: 19194
		[MayRequireIdeology]
		public static StatDef ConversionPower;

		// Token: 0x04004AFB RID: 19195
		public static StatDef AnimalGatherSpeed;

		// Token: 0x04004AFC RID: 19196
		public static StatDef AnimalGatherYield;

		// Token: 0x04004AFD RID: 19197
		public static StatDef TameAnimalChance;

		// Token: 0x04004AFE RID: 19198
		public static StatDef TrainAnimalChance;

		// Token: 0x04004AFF RID: 19199
		public static StatDef ShootingAccuracyPawn;

		// Token: 0x04004B00 RID: 19200
		public static StatDef ShootingAccuracyTurret;

		// Token: 0x04004B01 RID: 19201
		public static StatDef AimingDelayFactor;

		// Token: 0x04004B02 RID: 19202
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyOutdoorsDarkOffset;

		// Token: 0x04004B03 RID: 19203
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyOutdoorsLitOffset;

		// Token: 0x04004B04 RID: 19204
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyIndoorsDarkOffset;

		// Token: 0x04004B05 RID: 19205
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyIndoorsLitOffset;

		// Token: 0x04004B06 RID: 19206
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceOutdoorsDarkOffset;

		// Token: 0x04004B07 RID: 19207
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceOutdoorsLitOffset;

		// Token: 0x04004B08 RID: 19208
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceIndoorsDarkOffset;

		// Token: 0x04004B09 RID: 19209
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceIndoorsLitOffset;

		// Token: 0x04004B0A RID: 19210
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceOutdoorsDarkOffset;

		// Token: 0x04004B0B RID: 19211
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceOutdoorsLitOffset;

		// Token: 0x04004B0C RID: 19212
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceIndoorsDarkOffset;

		// Token: 0x04004B0D RID: 19213
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceIndoorsLitOffset;

		// Token: 0x04004B0E RID: 19214
		public static StatDef MeleeHitChance;

		// Token: 0x04004B0F RID: 19215
		public static StatDef MeleeDodgeChance;

		// Token: 0x04004B10 RID: 19216
		public static StatDef PawnTrapSpringChance;

		// Token: 0x04004B11 RID: 19217
		public static StatDef IncomingDamageFactor;

		// Token: 0x04004B12 RID: 19218
		public static StatDef MeleeWeapon_AverageDPS;

		// Token: 0x04004B13 RID: 19219
		public static StatDef MeleeWeapon_DamageMultiplier;

		// Token: 0x04004B14 RID: 19220
		public static StatDef MeleeWeapon_CooldownMultiplier;

		// Token: 0x04004B15 RID: 19221
		public static StatDef MeleeWeapon_AverageArmorPenetration;

		// Token: 0x04004B16 RID: 19222
		public static StatDef SharpDamageMultiplier;

		// Token: 0x04004B17 RID: 19223
		public static StatDef BluntDamageMultiplier;

		// Token: 0x04004B18 RID: 19224
		public static StatDef StuffPower_Armor_Sharp;

		// Token: 0x04004B19 RID: 19225
		public static StatDef StuffPower_Armor_Blunt;

		// Token: 0x04004B1A RID: 19226
		public static StatDef StuffPower_Armor_Heat;

		// Token: 0x04004B1B RID: 19227
		public static StatDef StuffPower_Insulation_Cold;

		// Token: 0x04004B1C RID: 19228
		public static StatDef StuffPower_Insulation_Heat;

		// Token: 0x04004B1D RID: 19229
		public static StatDef RangedWeapon_Cooldown;

		// Token: 0x04004B1E RID: 19230
		public static StatDef RangedWeapon_DamageMultiplier;

		// Token: 0x04004B1F RID: 19231
		public static StatDef AccuracyTouch;

		// Token: 0x04004B20 RID: 19232
		public static StatDef AccuracyShort;

		// Token: 0x04004B21 RID: 19233
		public static StatDef AccuracyMedium;

		// Token: 0x04004B22 RID: 19234
		public static StatDef AccuracyLong;

		// Token: 0x04004B23 RID: 19235
		public static StatDef StuffEffectMultiplierArmor;

		// Token: 0x04004B24 RID: 19236
		public static StatDef StuffEffectMultiplierInsulation_Cold;

		// Token: 0x04004B25 RID: 19237
		public static StatDef StuffEffectMultiplierInsulation_Heat;

		// Token: 0x04004B26 RID: 19238
		public static StatDef ArmorRating_Sharp;

		// Token: 0x04004B27 RID: 19239
		public static StatDef ArmorRating_Blunt;

		// Token: 0x04004B28 RID: 19240
		public static StatDef ArmorRating_Heat;

		// Token: 0x04004B29 RID: 19241
		public static StatDef Insulation_Cold;

		// Token: 0x04004B2A RID: 19242
		public static StatDef Insulation_Heat;

		// Token: 0x04004B2B RID: 19243
		public static StatDef EnergyShieldRechargeRate;

		// Token: 0x04004B2C RID: 19244
		public static StatDef EnergyShieldEnergyMax;

		// Token: 0x04004B2D RID: 19245
		public static StatDef SmokepopBeltRadius;

		// Token: 0x04004B2E RID: 19246
		[MayRequireRoyalty]
		public static StatDef JumpRange;

		// Token: 0x04004B2F RID: 19247
		public static StatDef EquipDelay;

		// Token: 0x04004B30 RID: 19248
		public static StatDef MedicalPotency;

		// Token: 0x04004B31 RID: 19249
		public static StatDef MedicalQualityMax;

		// Token: 0x04004B32 RID: 19250
		public static StatDef ImmunityGainSpeed;

		// Token: 0x04004B33 RID: 19251
		public static StatDef ImmunityGainSpeedFactor;

		// Token: 0x04004B34 RID: 19252
		public static StatDef InjuryHealingFactor;

		// Token: 0x04004B35 RID: 19253
		public static StatDef DoorOpenSpeed;

		// Token: 0x04004B36 RID: 19254
		public static StatDef BedRestEffectiveness;

		// Token: 0x04004B37 RID: 19255
		public static StatDef TrapMeleeDamage;

		// Token: 0x04004B38 RID: 19256
		public static StatDef TrapSpringChance;

		// Token: 0x04004B39 RID: 19257
		public static StatDef ResearchSpeedFactor;

		// Token: 0x04004B3A RID: 19258
		public static StatDef MedicalTendQualityOffset;

		// Token: 0x04004B3B RID: 19259
		public static StatDef WorkTableWorkSpeedFactor;

		// Token: 0x04004B3C RID: 19260
		public static StatDef WorkTableEfficiencyFactor;

		// Token: 0x04004B3D RID: 19261
		public static StatDef JoyGainFactor;

		// Token: 0x04004B3E RID: 19262
		public static StatDef SurgerySuccessChanceFactor;

		// Token: 0x04004B3F RID: 19263
		[MayRequireIdeology]
		public static StatDef StyleDominance;

		// Token: 0x04004B40 RID: 19264
		public static StatDef Ability_CastingTime;

		// Token: 0x04004B41 RID: 19265
		public static StatDef Ability_EntropyGain;

		// Token: 0x04004B42 RID: 19266
		public static StatDef Ability_PsyfocusCost;

		// Token: 0x04004B43 RID: 19267
		public static StatDef Ability_Duration;

		// Token: 0x04004B44 RID: 19268
		public static StatDef Ability_Range;

		// Token: 0x04004B45 RID: 19269
		public static StatDef Ability_EffectRadius;

		// Token: 0x04004B46 RID: 19270
		public static StatDef Ability_RequiredPsylink;

		// Token: 0x04004B47 RID: 19271
		public static StatDef Ability_GoodwillImpact;

		// Token: 0x04004B48 RID: 19272
		public static StatDef Ability_DetectChancePerEntropy;

		// Token: 0x04004B49 RID: 19273
		public static StatDef MeditationFocusStrength;

		// Token: 0x04004B4A RID: 19274
		[MayRequireIdeology]
		public static StatDef TerrorSource;

		// Token: 0x04004B4B RID: 19275
		[MayRequireIdeology]
		public static StatDef Terror;

		// Token: 0x04004B4C RID: 19276
		public static StatDef FilthMultiplier;
	}
}
