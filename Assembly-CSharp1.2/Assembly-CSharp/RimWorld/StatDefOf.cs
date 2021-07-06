using System;

namespace RimWorld
{
	// Token: 0x02001C6E RID: 7278
	[DefOf]
	public static class StatDefOf
	{
		// Token: 0x06009F71 RID: 40817 RVA: 0x0006A3EE File Offset: 0x000685EE
		static StatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StatDefOf));
		}

		// Token: 0x04006A07 RID: 27143
		public static StatDef MaxHitPoints;

		// Token: 0x04006A08 RID: 27144
		public static StatDef MarketValue;

		// Token: 0x04006A09 RID: 27145
		public static StatDef MarketValueIgnoreHp;

		// Token: 0x04006A0A RID: 27146
		public static StatDef RoyalFavorValue;

		// Token: 0x04006A0B RID: 27147
		public static StatDef SellPriceFactor;

		// Token: 0x04006A0C RID: 27148
		public static StatDef Beauty;

		// Token: 0x04006A0D RID: 27149
		public static StatDef Cleanliness;

		// Token: 0x04006A0E RID: 27150
		public static StatDef Flammability;

		// Token: 0x04006A0F RID: 27151
		public static StatDef DeteriorationRate;

		// Token: 0x04006A10 RID: 27152
		public static StatDef WorkToMake;

		// Token: 0x04006A11 RID: 27153
		public static StatDef WorkToBuild;

		// Token: 0x04006A12 RID: 27154
		public static StatDef Mass;

		// Token: 0x04006A13 RID: 27155
		public static StatDef ConstructionSpeedFactor;

		// Token: 0x04006A14 RID: 27156
		public static StatDef Nutrition;

		// Token: 0x04006A15 RID: 27157
		public static StatDef FoodPoisonChanceFixedHuman;

		// Token: 0x04006A16 RID: 27158
		public static StatDef MoveSpeed;

		// Token: 0x04006A17 RID: 27159
		public static StatDef GlobalLearningFactor;

		// Token: 0x04006A18 RID: 27160
		public static StatDef HungerRateMultiplier;

		// Token: 0x04006A19 RID: 27161
		public static StatDef RestRateMultiplier;

		// Token: 0x04006A1A RID: 27162
		public static StatDef PsychicSensitivity;

		// Token: 0x04006A1B RID: 27163
		public static StatDef ToxicSensitivity;

		// Token: 0x04006A1C RID: 27164
		public static StatDef MentalBreakThreshold;

		// Token: 0x04006A1D RID: 27165
		public static StatDef EatingSpeed;

		// Token: 0x04006A1E RID: 27166
		public static StatDef ComfyTemperatureMin;

		// Token: 0x04006A1F RID: 27167
		public static StatDef ComfyTemperatureMax;

		// Token: 0x04006A20 RID: 27168
		public static StatDef Comfort;

		// Token: 0x04006A21 RID: 27169
		public static StatDef MeatAmount;

		// Token: 0x04006A22 RID: 27170
		public static StatDef LeatherAmount;

		// Token: 0x04006A23 RID: 27171
		public static StatDef MinimumHandlingSkill;

		// Token: 0x04006A24 RID: 27172
		public static StatDef MeleeDPS;

		// Token: 0x04006A25 RID: 27173
		public static StatDef PainShockThreshold;

		// Token: 0x04006A26 RID: 27174
		public static StatDef ForagedNutritionPerDay;

		// Token: 0x04006A27 RID: 27175
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyMax;

		// Token: 0x04006A28 RID: 27176
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyRecoveryRate;

		// Token: 0x04006A29 RID: 27177
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyGain;

		// Token: 0x04006A2A RID: 27178
		[MayRequireRoyalty]
		public static StatDef MeditationFocusGain;

		// Token: 0x04006A2B RID: 27179
		public static StatDef WorkSpeedGlobal;

		// Token: 0x04006A2C RID: 27180
		public static StatDef MiningSpeed;

		// Token: 0x04006A2D RID: 27181
		public static StatDef DeepDrillingSpeed;

		// Token: 0x04006A2E RID: 27182
		public static StatDef MiningYield;

		// Token: 0x04006A2F RID: 27183
		public static StatDef ResearchSpeed;

		// Token: 0x04006A30 RID: 27184
		public static StatDef ConstructionSpeed;

		// Token: 0x04006A31 RID: 27185
		public static StatDef HuntingStealth;

		// Token: 0x04006A32 RID: 27186
		public static StatDef PlantWorkSpeed;

		// Token: 0x04006A33 RID: 27187
		public static StatDef SmoothingSpeed;

		// Token: 0x04006A34 RID: 27188
		public static StatDef FoodPoisonChance;

		// Token: 0x04006A35 RID: 27189
		public static StatDef CarryingCapacity;

		// Token: 0x04006A36 RID: 27190
		public static StatDef PlantHarvestYield;

		// Token: 0x04006A37 RID: 27191
		public static StatDef FixBrokenDownBuildingSuccessChance;

		// Token: 0x04006A38 RID: 27192
		public static StatDef ConstructSuccessChance;

		// Token: 0x04006A39 RID: 27193
		public static StatDef GeneralLaborSpeed;

		// Token: 0x04006A3A RID: 27194
		[DefAlias("GeneralLaborSpeed")]
		[Obsolete("Use StatDefOf.GeneralLaborSpeed, this field is only here for legacy reasons and will be removed in the future.")]
		public static StatDef UnskilledLaborSpeed;

		// Token: 0x04006A3B RID: 27195
		public static StatDef MedicalTendSpeed;

		// Token: 0x04006A3C RID: 27196
		public static StatDef MedicalTendQuality;

		// Token: 0x04006A3D RID: 27197
		public static StatDef MedicalSurgerySuccessChance;

		// Token: 0x04006A3E RID: 27198
		public static StatDef NegotiationAbility;

		// Token: 0x04006A3F RID: 27199
		public static StatDef ArrestSuccessChance;

		// Token: 0x04006A40 RID: 27200
		public static StatDef TradePriceImprovement;

		// Token: 0x04006A41 RID: 27201
		public static StatDef SocialImpact;

		// Token: 0x04006A42 RID: 27202
		public static StatDef PawnBeauty;

		// Token: 0x04006A43 RID: 27203
		public static StatDef AnimalGatherSpeed;

		// Token: 0x04006A44 RID: 27204
		public static StatDef AnimalGatherYield;

		// Token: 0x04006A45 RID: 27205
		public static StatDef TameAnimalChance;

		// Token: 0x04006A46 RID: 27206
		public static StatDef TrainAnimalChance;

		// Token: 0x04006A47 RID: 27207
		public static StatDef ShootingAccuracyPawn;

		// Token: 0x04006A48 RID: 27208
		public static StatDef ShootingAccuracyTurret;

		// Token: 0x04006A49 RID: 27209
		public static StatDef AimingDelayFactor;

		// Token: 0x04006A4A RID: 27210
		public static StatDef MeleeHitChance;

		// Token: 0x04006A4B RID: 27211
		public static StatDef MeleeDodgeChance;

		// Token: 0x04006A4C RID: 27212
		public static StatDef PawnTrapSpringChance;

		// Token: 0x04006A4D RID: 27213
		public static StatDef IncomingDamageFactor;

		// Token: 0x04006A4E RID: 27214
		public static StatDef MeleeWeapon_AverageDPS;

		// Token: 0x04006A4F RID: 27215
		public static StatDef MeleeWeapon_DamageMultiplier;

		// Token: 0x04006A50 RID: 27216
		public static StatDef MeleeWeapon_CooldownMultiplier;

		// Token: 0x04006A51 RID: 27217
		public static StatDef MeleeWeapon_AverageArmorPenetration;

		// Token: 0x04006A52 RID: 27218
		public static StatDef SharpDamageMultiplier;

		// Token: 0x04006A53 RID: 27219
		public static StatDef BluntDamageMultiplier;

		// Token: 0x04006A54 RID: 27220
		public static StatDef StuffPower_Armor_Sharp;

		// Token: 0x04006A55 RID: 27221
		public static StatDef StuffPower_Armor_Blunt;

		// Token: 0x04006A56 RID: 27222
		public static StatDef StuffPower_Armor_Heat;

		// Token: 0x04006A57 RID: 27223
		public static StatDef StuffPower_Insulation_Cold;

		// Token: 0x04006A58 RID: 27224
		public static StatDef StuffPower_Insulation_Heat;

		// Token: 0x04006A59 RID: 27225
		public static StatDef RangedWeapon_Cooldown;

		// Token: 0x04006A5A RID: 27226
		public static StatDef RangedWeapon_DamageMultiplier;

		// Token: 0x04006A5B RID: 27227
		public static StatDef AccuracyTouch;

		// Token: 0x04006A5C RID: 27228
		public static StatDef AccuracyShort;

		// Token: 0x04006A5D RID: 27229
		public static StatDef AccuracyMedium;

		// Token: 0x04006A5E RID: 27230
		public static StatDef AccuracyLong;

		// Token: 0x04006A5F RID: 27231
		public static StatDef StuffEffectMultiplierArmor;

		// Token: 0x04006A60 RID: 27232
		public static StatDef StuffEffectMultiplierInsulation_Cold;

		// Token: 0x04006A61 RID: 27233
		public static StatDef StuffEffectMultiplierInsulation_Heat;

		// Token: 0x04006A62 RID: 27234
		public static StatDef ArmorRating_Sharp;

		// Token: 0x04006A63 RID: 27235
		public static StatDef ArmorRating_Blunt;

		// Token: 0x04006A64 RID: 27236
		public static StatDef ArmorRating_Heat;

		// Token: 0x04006A65 RID: 27237
		public static StatDef Insulation_Cold;

		// Token: 0x04006A66 RID: 27238
		public static StatDef Insulation_Heat;

		// Token: 0x04006A67 RID: 27239
		public static StatDef EnergyShieldRechargeRate;

		// Token: 0x04006A68 RID: 27240
		public static StatDef EnergyShieldEnergyMax;

		// Token: 0x04006A69 RID: 27241
		public static StatDef SmokepopBeltRadius;

		// Token: 0x04006A6A RID: 27242
		[MayRequireRoyalty]
		public static StatDef JumpRange;

		// Token: 0x04006A6B RID: 27243
		public static StatDef EquipDelay;

		// Token: 0x04006A6C RID: 27244
		public static StatDef MedicalPotency;

		// Token: 0x04006A6D RID: 27245
		public static StatDef MedicalQualityMax;

		// Token: 0x04006A6E RID: 27246
		public static StatDef ImmunityGainSpeed;

		// Token: 0x04006A6F RID: 27247
		public static StatDef ImmunityGainSpeedFactor;

		// Token: 0x04006A70 RID: 27248
		public static StatDef DoorOpenSpeed;

		// Token: 0x04006A71 RID: 27249
		public static StatDef BedRestEffectiveness;

		// Token: 0x04006A72 RID: 27250
		public static StatDef TrapMeleeDamage;

		// Token: 0x04006A73 RID: 27251
		public static StatDef TrapSpringChance;

		// Token: 0x04006A74 RID: 27252
		public static StatDef ResearchSpeedFactor;

		// Token: 0x04006A75 RID: 27253
		public static StatDef MedicalTendQualityOffset;

		// Token: 0x04006A76 RID: 27254
		public static StatDef WorkTableWorkSpeedFactor;

		// Token: 0x04006A77 RID: 27255
		public static StatDef WorkTableEfficiencyFactor;

		// Token: 0x04006A78 RID: 27256
		public static StatDef JoyGainFactor;

		// Token: 0x04006A79 RID: 27257
		public static StatDef SurgerySuccessChanceFactor;

		// Token: 0x04006A7A RID: 27258
		public static StatDef Ability_CastingTime;

		// Token: 0x04006A7B RID: 27259
		public static StatDef Ability_EntropyGain;

		// Token: 0x04006A7C RID: 27260
		public static StatDef Ability_PsyfocusCost;

		// Token: 0x04006A7D RID: 27261
		public static StatDef Ability_Duration;

		// Token: 0x04006A7E RID: 27262
		public static StatDef Ability_Range;

		// Token: 0x04006A7F RID: 27263
		public static StatDef Ability_EffectRadius;

		// Token: 0x04006A80 RID: 27264
		public static StatDef Ability_RequiredPsylink;

		// Token: 0x04006A81 RID: 27265
		public static StatDef Ability_GoodwillImpact;

		// Token: 0x04006A82 RID: 27266
		public static StatDef Ability_DetectChancePerEntropy;

		// Token: 0x04006A83 RID: 27267
		[Obsolete("Will be removed in the future")]
		public static StatDef Bladelink_DetectionChance;

		// Token: 0x04006A84 RID: 27268
		public static StatDef MeditationFocusStrength;
	}
}
