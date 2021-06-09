using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000126 RID: 294
	public class VerbProperties
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0000C49E File Offset: 0x0000A69E
		public bool CausesTimeSlowdown
		{
			get
			{
				return this.ai_IsWeapon && this.forceNormalTimeSpeed;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
		public bool LaunchesProjectile
		{
			get
			{
				return typeof(Verb_LaunchProjectile).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x00094210 File Offset: 0x00092410
		public string AccuracySummaryString
		{
			get
			{
				return string.Concat(new string[]
				{
					this.accuracyTouch.ToStringPercent(),
					" - ",
					this.accuracyShort.ToStringPercent(),
					" - ",
					this.accuracyMedium.ToStringPercent(),
					" - ",
					this.accuracyLong.ToStringPercent()
				});
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0000C4C7 File Offset: 0x0000A6C7
		public bool IsMeleeAttack
		{
			get
			{
				return typeof(Verb_MeleeAttack).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x00094278 File Offset: 0x00092478
		public bool CausesExplosion
		{
			get
			{
				return this.defaultProjectile != null && (typeof(Projectile_Explosive).IsAssignableFrom(this.defaultProjectile.thingClass) || typeof(Projectile_DoomsdayRocket).IsAssignableFrom(this.defaultProjectile.thingClass) || this.defaultProjectile.GetCompProperties<CompProperties_Explosive>() != null);
			}
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0000C4DE File Offset: 0x0000A6DE
		public float AdjustedMeleeDamageAmount(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee damage amount for a verb with different verb props. verb=" + ownerVerb, 5469809, false);
				return 0f;
			}
			return this.AdjustedMeleeDamageAmount(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000942D8 File Offset: 0x000924D8
		public float AdjustedMeleeDamageAmount(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238, false);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount(equipment, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= this.GetDamageFactorFor(tool, attacker, hediffCompSource);
			}
			return num;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00094330 File Offset: 0x00092530
		public float AdjustedMeleeDamageAmount_NewTmp(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238, false);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount_NewTmp(equipment, equipmentStuff, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= this.GetDamageFactorFor(tool, attacker, hediffCompSource);
			}
			return num;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0000C51E File Offset: 0x0000A71E
		public float AdjustedArmorPenetration(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate armor penetration for a verb with different verb props. verb=" + ownerVerb, 9865767, false);
				return 0f;
			}
			return this.AdjustedArmorPenetration(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0009438C File Offset: 0x0009258C
		public float AdjustedArmorPenetration(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			float num;
			if (tool != null)
			{
				num = tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				num = this.AdjustedMeleeDamageAmount(tool, attacker, equipment, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValue = equipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true);
				num *= statValue;
			}
			return num;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000943E0 File Offset: 0x000925E0
		public float AdjustedArmorPenetration_NewTmp(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			float num;
			if (tool != null)
			{
				num = tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				num = this.AdjustedMeleeDamageAmount_NewTmp(tool, attacker, equipment, equipmentStuff, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValueAbstract = equipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, equipmentStuff);
				num *= statValueAbstract;
			}
			return num;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0000C55E File Offset: 0x0000A75E
		private float AdjustedExpectedDamageForVerbUsableInMelee(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount(tool, attacker, equipment, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount(equipment, null);
			}
			return 0f;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00094434 File Offset: 0x00092634
		private float AdjustedExpectedDamageForVerbUsableInMelee_NewTmp(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount_NewTmp(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount_NewTmp(equipment, equipmentStuff, null);
			}
			return 0f;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00094484 File Offset: 0x00092684
		public float AdjustedMeleeSelectionWeight(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee selection weight for a verb with different verb props. verb=" + ownerVerb, 385716351, false);
				return 0f;
			}
			return this.AdjustedMeleeSelectionWeight(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource, ownerVerb.DirectOwner is Pawn);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000944E0 File Offset: 0x000926E0
		public float AdjustedMeleeSelectionWeight(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
		{
			if (!this.IsMeleeAttack)
			{
				return 0f;
			}
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee(tool, attacker, equipment, hediffCompSource);
			if (num2 >= 0.001f || !typeof(Verb_MeleeApplyHediff).IsAssignableFrom(this.verbClass))
			{
				num *= num2 * num2;
			}
			num *= this.commonality;
			if (tool != null)
			{
				num *= tool.chanceFactor;
			}
			if (comesFromPawnNativeVerbs && (tool == null || !tool.alwaysTreatAsWeapon))
			{
				num *= 0.3f;
			}
			return num;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0009457C File Offset: 0x0009277C
		public float AdjustedMeleeSelectionWeight_NewTmp(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
		{
			if (!this.IsMeleeAttack)
			{
				return 0f;
			}
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee_NewTmp(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			if (num2 >= 0.001f || !typeof(Verb_MeleeApplyHediff).IsAssignableFrom(this.verbClass))
			{
				num *= num2 * num2;
			}
			num *= this.commonality;
			if (tool != null)
			{
				num *= tool.chanceFactor;
			}
			if (comesFromPawnNativeVerbs && (tool == null || !tool.alwaysTreatAsWeapon))
			{
				num *= 0.3f;
			}
			return num;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0000C59D File Offset: 0x0000A79D
		public float AdjustedCooldown(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate cooldown for a verb with different verb props. verb=" + ownerVerb, 19485711, false);
				return 0f;
			}
			return this.AdjustedCooldown(ownerVerb.tool, attacker, ownerVerb.EquipmentSource);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0000C5D7 File Offset: 0x0000A7D7
		public float AdjustedCooldown(Tool tool, Pawn attacker, Thing equipment)
		{
			if (tool != null)
			{
				return tool.AdjustedCooldown(equipment);
			}
			if (equipment != null && !this.IsMeleeAttack)
			{
				return equipment.GetStatValue(StatDefOf.RangedWeapon_Cooldown, true);
			}
			return this.defaultCooldownTime;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0000C602 File Offset: 0x0000A802
		public float AdjustedCooldown_NewTmp(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff)
		{
			if (tool != null)
			{
				return tool.AdjustedCooldown_NewTmp(equipment, equipmentStuff);
			}
			if (equipment != null && !this.IsMeleeAttack)
			{
				return equipment.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, equipmentStuff);
			}
			return this.defaultCooldownTime;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0000C630 File Offset: 0x0000A830
		public int AdjustedCooldownTicks(Verb ownerVerb, Pawn attacker)
		{
			return this.AdjustedCooldown(ownerVerb, attacker).SecondsToTicks();
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0009461C File Offset: 0x0009281C
		private float AdjustedAccuracy(VerbProperties.RangeCategory cat, Thing equipment)
		{
			if (equipment != null)
			{
				StatDef stat = null;
				switch (cat)
				{
				case VerbProperties.RangeCategory.Touch:
					stat = StatDefOf.AccuracyTouch;
					break;
				case VerbProperties.RangeCategory.Short:
					stat = StatDefOf.AccuracyShort;
					break;
				case VerbProperties.RangeCategory.Medium:
					stat = StatDefOf.AccuracyMedium;
					break;
				case VerbProperties.RangeCategory.Long:
					stat = StatDefOf.AccuracyLong;
					break;
				}
				return equipment.GetStatValue(stat, true);
			}
			switch (cat)
			{
			case VerbProperties.RangeCategory.Touch:
				return this.accuracyTouch;
			case VerbProperties.RangeCategory.Short:
				return this.accuracyShort;
			case VerbProperties.RangeCategory.Medium:
				return this.accuracyMedium;
			case VerbProperties.RangeCategory.Long:
				return this.accuracyLong;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0000C63F File Offset: 0x0000A83F
		public float AdjustedFullCycleTime(Verb ownerVerb, Pawn attacker)
		{
			return this.warmupTime + this.AdjustedCooldown(ownerVerb, attacker) + ((this.burstShotCount - 1) * this.ticksBetweenBurstShots).TicksToSeconds();
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0000C665 File Offset: 0x0000A865
		public float GetDamageFactorFor(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate damage factor for a verb with different verb props. verb=" + ownerVerb, 94324562, false);
				return 1f;
			}
			return this.GetDamageFactorFor(ownerVerb.tool, attacker, ownerVerb.HediffCompSource);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x000946A8 File Offset: 0x000928A8
		public float GetDamageFactorFor(Tool tool, Pawn attacker, HediffComp_VerbGiver hediffCompSource)
		{
			float num = 1f;
			if (attacker != null)
			{
				if (hediffCompSource != null)
				{
					num *= PawnCapacityUtility.CalculatePartEfficiency(hediffCompSource.Pawn.health.hediffSet, hediffCompSource.parent.Part, true, null);
				}
				else if (attacker != null && this.AdjustedLinkedBodyPartsGroup(tool) != null)
				{
					float num2 = PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(attacker.health.hediffSet, this.AdjustedLinkedBodyPartsGroup(tool));
					if (this.AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(tool))
					{
						num2 = Mathf.Max(num2, 0.4f);
					}
					num *= num2;
				}
				if (attacker != null && this.IsMeleeAttack)
				{
					num *= attacker.ageTracker.CurLifeStage.meleeDamageFactor;
				}
			}
			return num;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0000C69F File Offset: 0x0000A89F
		public BodyPartGroupDef AdjustedLinkedBodyPartsGroup(Tool tool)
		{
			if (tool != null)
			{
				return tool.linkedBodyPartsGroup;
			}
			return this.linkedBodyPartsGroup;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0000C6B1 File Offset: 0x0000A8B1
		public bool AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(Tool tool)
		{
			if (tool != null)
			{
				return tool.ensureLinkedBodyPartsGroupAlwaysUsable;
			}
			return this.ensureLinkedBodyPartsGroupAlwaysUsable;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0000C6C3 File Offset: 0x0000A8C3
		public float EffectiveMinRange(LocalTargetInfo target, Thing caster)
		{
			return this.EffectiveMinRange(VerbUtility.AllowAdjacentShot(target, caster));
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00094748 File Offset: 0x00092948
		public float EffectiveMinRange(bool allowAdjacentShot)
		{
			float num = this.minRange;
			if (!allowAdjacentShot && !this.IsMeleeAttack && this.LaunchesProjectile)
			{
				num = Mathf.Max(num, 1.421f);
			}
			return num;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0009477C File Offset: 0x0009297C
		public float GetHitChanceFactor(Thing equipment, float dist)
		{
			float value;
			if (dist <= 3f)
			{
				value = this.AdjustedAccuracy(VerbProperties.RangeCategory.Touch, equipment);
			}
			else if (dist <= 12f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Touch, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Short, equipment), (dist - 3f) / 9f);
			}
			else if (dist <= 25f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Short, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Medium, equipment), (dist - 12f) / 13f);
			}
			else if (dist <= 40f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Medium, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Long, equipment), (dist - 25f) / 15f);
			}
			else
			{
				value = this.AdjustedAccuracy(VerbProperties.RangeCategory.Long, equipment);
			}
			return Mathf.Clamp(value, 0.01f, 1f);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00094844 File Offset: 0x00092A44
		public void DrawRadiusRing(IntVec3 center)
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!this.IsMeleeAttack && this.targetable)
			{
				float num = this.EffectiveMinRange(true);
				if (num > 0f && num < GenRadial.MaxRadialPatternRadius)
				{
					GenDraw.DrawRadiusRing(center, num);
				}
				if (this.range < (float)(Find.CurrentMap.Size.x + Find.CurrentMap.Size.z) && this.range < GenRadial.MaxRadialPatternRadius)
				{
					Func<IntVec3, bool> predicate = null;
					if (this.drawHighlightWithLineOfSight)
					{
						predicate = ((IntVec3 c) => GenSight.LineOfSight(center, c, Find.CurrentMap, false, null, 0, 0));
					}
					GenDraw.DrawRadiusRing(center, this.range, Color.white, predicate);
				}
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00094904 File Offset: 0x00092B04
		public override string ToString()
		{
			string str;
			if (!this.label.NullOrEmpty())
			{
				str = this.label;
			}
			else
			{
				str = string.Concat(new object[]
				{
					"range=",
					this.range,
					", defaultProjectile=",
					this.defaultProjectile.ToStringSafe<ThingDef>()
				});
			}
			return "VerbProperties(" + str + ")";
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0000C6D2 File Offset: 0x0000A8D2
		public new VerbProperties MemberwiseClone()
		{
			return (VerbProperties)base.MemberwiseClone();
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0000C6DF File Offset: 0x0000A8DF
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (parent.race != null && this.linkedBodyPartsGroup != null && !parent.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(this.linkedBodyPartsGroup)))
			{
				yield return string.Concat(new object[]
				{
					"has verb with linkedBodyPartsGroup ",
					this.linkedBodyPartsGroup,
					" but body ",
					parent.race.body,
					" has no parts with that group."
				});
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null && this.forcedMissRadius > 0f != this.CausesExplosion)
			{
				yield return "has incorrect forcedMiss settings; explosive projectiles and only explosive projectiles should have forced miss enabled";
			}
			yield break;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0000C6F6 File Offset: 0x0000A8F6
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x0400058B RID: 1419
		public VerbCategory category = VerbCategory.Misc;

		// Token: 0x0400058C RID: 1420
		[TranslationHandle]
		public Type verbClass = typeof(Verb);

		// Token: 0x0400058D RID: 1421
		[MustTranslate]
		public string label;

		// Token: 0x0400058E RID: 1422
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x0400058F RID: 1423
		public bool isPrimary = true;

		// Token: 0x04000590 RID: 1424
		public bool violent = true;

		// Token: 0x04000591 RID: 1425
		public float minRange;

		// Token: 0x04000592 RID: 1426
		public float range = 1.42f;

		// Token: 0x04000593 RID: 1427
		public int burstShotCount = 1;

		// Token: 0x04000594 RID: 1428
		public int ticksBetweenBurstShots = 15;

		// Token: 0x04000595 RID: 1429
		public float noiseRadius = 3f;

		// Token: 0x04000596 RID: 1430
		public bool hasStandardCommand;

		// Token: 0x04000597 RID: 1431
		public bool targetable = true;

		// Token: 0x04000598 RID: 1432
		public bool nonInterruptingSelfCast;

		// Token: 0x04000599 RID: 1433
		public TargetingParameters targetParams = new TargetingParameters();

		// Token: 0x0400059A RID: 1434
		public bool requireLineOfSight = true;

		// Token: 0x0400059B RID: 1435
		public bool mustCastOnOpenGround;

		// Token: 0x0400059C RID: 1436
		public bool forceNormalTimeSpeed = true;

		// Token: 0x0400059D RID: 1437
		public bool onlyManualCast;

		// Token: 0x0400059E RID: 1438
		public bool stopBurstWithoutLos = true;

		// Token: 0x0400059F RID: 1439
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x040005A0 RID: 1440
		public float commonality = 1f;

		// Token: 0x040005A1 RID: 1441
		public Intelligence minIntelligence;

		// Token: 0x040005A2 RID: 1442
		public float consumeFuelPerShot;

		// Token: 0x040005A3 RID: 1443
		public float warmupTime;

		// Token: 0x040005A4 RID: 1444
		public float defaultCooldownTime;

		// Token: 0x040005A5 RID: 1445
		public string commandIcon;

		// Token: 0x040005A6 RID: 1446
		public SoundDef soundCast;

		// Token: 0x040005A7 RID: 1447
		public SoundDef soundCastTail;

		// Token: 0x040005A8 RID: 1448
		public SoundDef soundAiming;

		// Token: 0x040005A9 RID: 1449
		public float muzzleFlashScale;

		// Token: 0x040005AA RID: 1450
		public ThingDef impactMote;

		// Token: 0x040005AB RID: 1451
		public bool drawAimPie = true;

		// Token: 0x040005AC RID: 1452
		public EffecterDef warmupEffecter;

		// Token: 0x040005AD RID: 1453
		public bool drawHighlightWithLineOfSight;

		// Token: 0x040005AE RID: 1454
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x040005AF RID: 1455
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x040005B0 RID: 1456
		public DamageDef meleeDamageDef;

		// Token: 0x040005B1 RID: 1457
		public int meleeDamageBaseAmount = 1;

		// Token: 0x040005B2 RID: 1458
		public float meleeArmorPenetrationBase = -1f;

		// Token: 0x040005B3 RID: 1459
		public bool ai_IsWeapon = true;

		// Token: 0x040005B4 RID: 1460
		public bool ai_IsBuildingDestroyer;

		// Token: 0x040005B5 RID: 1461
		public float ai_AvoidFriendlyFireRadius;

		// Token: 0x040005B6 RID: 1462
		public ThingDef defaultProjectile;

		// Token: 0x040005B7 RID: 1463
		public float forcedMissRadius;

		// Token: 0x040005B8 RID: 1464
		public float accuracyTouch = 1f;

		// Token: 0x040005B9 RID: 1465
		public float accuracyShort = 1f;

		// Token: 0x040005BA RID: 1466
		public float accuracyMedium = 1f;

		// Token: 0x040005BB RID: 1467
		public float accuracyLong = 1f;

		// Token: 0x040005BC RID: 1468
		public ThingDef spawnDef;

		// Token: 0x040005BD RID: 1469
		public TaleDef colonyWideTaleDef;

		// Token: 0x040005BE RID: 1470
		public BodyPartTagDef bodypartTagTarget;

		// Token: 0x040005BF RID: 1471
		public RulePackDef rangedFireRulepack;

		// Token: 0x040005C0 RID: 1472
		public const float DefaultArmorPenetrationPerDamage = 0.015f;

		// Token: 0x040005C1 RID: 1473
		private const float VerbSelectionWeightFactor_BodyPart = 0.3f;

		// Token: 0x040005C2 RID: 1474
		private const float MinLinkedBodyPartGroupEfficiencyIfMustBeAlwaysUsable = 0.4f;

		// Token: 0x02000127 RID: 295
		private enum RangeCategory : byte
		{
			// Token: 0x040005C4 RID: 1476
			Touch,
			// Token: 0x040005C5 RID: 1477
			Short,
			// Token: 0x040005C6 RID: 1478
			Medium,
			// Token: 0x040005C7 RID: 1479
			Long
		}
	}
}
