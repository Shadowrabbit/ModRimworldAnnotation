using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000BE RID: 190
	public class VerbProperties
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001D2CB File Offset: 0x0001B4CB
		public bool CausesTimeSlowdown
		{
			get
			{
				return this.ai_IsWeapon && this.forceNormalTimeSpeed;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0001D2DD File Offset: 0x0001B4DD
		public bool LaunchesProjectile
		{
			get
			{
				return typeof(Verb_LaunchProjectile).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0001D2F4 File Offset: 0x0001B4F4
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

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0001D35C File Offset: 0x0001B55C
		public bool IsMeleeAttack
		{
			get
			{
				return typeof(Verb_MeleeAttack).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x0001D374 File Offset: 0x0001B574
		public bool CausesExplosion
		{
			get
			{
				return this.defaultProjectile != null && (typeof(Projectile_Explosive).IsAssignableFrom(this.defaultProjectile.thingClass) || typeof(Projectile_DoomsdayRocket).IsAssignableFrom(this.defaultProjectile.thingClass) || this.defaultProjectile.GetCompProperties<CompProperties_Explosive>() != null);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0001D3D4 File Offset: 0x0001B5D4
		public float ForcedMissRadius
		{
			get
			{
				if (this.isMortar && this.forcedMissRadiusClassicMortars >= 0f)
				{
					Storyteller storyteller = Find.Storyteller;
					if (((storyteller != null) ? storyteller.difficulty : null) != null && Find.Storyteller.difficulty.classicMortars)
					{
						return this.forcedMissRadiusClassicMortars;
					}
				}
				return this.forcedMissRadius;
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001D427 File Offset: 0x0001B627
		public float AdjustedMeleeDamageAmount(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee damage amount for a verb with different verb props. verb=" + ownerVerb, 5469809);
				return 0f;
			}
			return this.AdjustedMeleeDamageAmount(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001D468 File Offset: 0x0001B668
		public float AdjustedMeleeDamageAmount(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238);
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

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001D4C0 File Offset: 0x0001B6C0
		public float AdjustedMeleeDamageAmount(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount(equipment, equipmentStuff, this.meleeDamageDef);
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

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001D518 File Offset: 0x0001B718
		public float AdjustedArmorPenetration(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate armor penetration for a verb with different verb props. verb=" + ownerVerb, 9865767);
				return 0f;
			}
			return this.AdjustedArmorPenetration(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001D558 File Offset: 0x0001B758
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

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001D5AC File Offset: 0x0001B7AC
		public float AdjustedArmorPenetration(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
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
				num = this.AdjustedMeleeDamageAmount(tool, attacker, equipment, equipmentStuff, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValueAbstract = equipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, equipmentStuff);
				num *= statValueAbstract;
			}
			return num;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001D600 File Offset: 0x0001B800
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

		// Token: 0x060005BA RID: 1466 RVA: 0x0001D640 File Offset: 0x0001B840
		private float AdjustedExpectedDamageForVerbUsableInMelee(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount(equipment, equipmentStuff, null);
			}
			return 0f;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001D690 File Offset: 0x0001B890
		public float AdjustedMeleeSelectionWeight(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee selection weight for a verb with different verb props. verb=" + ownerVerb, 385716351);
				return 0f;
			}
			return this.AdjustedMeleeSelectionWeight(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource, ownerVerb.DirectOwner is Pawn);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001D6E8 File Offset: 0x0001B8E8
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

		// Token: 0x060005BD RID: 1469 RVA: 0x0001D784 File Offset: 0x0001B984
		public float AdjustedMeleeSelectionWeight(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
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
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee(tool, attacker, equipment, equipmentStuff, hediffCompSource);
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

		// Token: 0x060005BE RID: 1470 RVA: 0x0001D821 File Offset: 0x0001BA21
		public float AdjustedCooldown(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate cooldown for a verb with different verb props. verb=" + ownerVerb, 19485711);
				return 0f;
			}
			return this.AdjustedCooldown(ownerVerb.tool, attacker, ownerVerb.EquipmentSource);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001D85A File Offset: 0x0001BA5A
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

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001D885 File Offset: 0x0001BA85
		public float AdjustedCooldown(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff)
		{
			if (tool != null)
			{
				return tool.AdjustedCooldown(equipment, equipmentStuff);
			}
			if (equipment != null && !this.IsMeleeAttack)
			{
				return equipment.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, equipmentStuff);
			}
			return this.defaultCooldownTime;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001D8B3 File Offset: 0x0001BAB3
		public int AdjustedCooldownTicks(Verb ownerVerb, Pawn attacker)
		{
			return this.AdjustedCooldown(ownerVerb, attacker).SecondsToTicks();
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001D8C4 File Offset: 0x0001BAC4
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

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001D94E File Offset: 0x0001BB4E
		public float AdjustedFullCycleTime(Verb ownerVerb, Pawn attacker)
		{
			return this.warmupTime + this.AdjustedCooldown(ownerVerb, attacker) + ((this.burstShotCount - 1) * this.ticksBetweenBurstShots).TicksToSeconds();
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001D974 File Offset: 0x0001BB74
		public float GetDamageFactorFor(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate damage factor for a verb with different verb props. verb=" + ownerVerb, 94324562);
				return 1f;
			}
			return this.GetDamageFactorFor(ownerVerb.tool, attacker, ownerVerb.HediffCompSource);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001D9B0 File Offset: 0x0001BBB0
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

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001DA4F File Offset: 0x0001BC4F
		public BodyPartGroupDef AdjustedLinkedBodyPartsGroup(Tool tool)
		{
			if (tool != null)
			{
				return tool.linkedBodyPartsGroup;
			}
			return this.linkedBodyPartsGroup;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001DA61 File Offset: 0x0001BC61
		public bool AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(Tool tool)
		{
			if (tool != null)
			{
				return tool.ensureLinkedBodyPartsGroupAlwaysUsable;
			}
			return this.ensureLinkedBodyPartsGroupAlwaysUsable;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001DA73 File Offset: 0x0001BC73
		public float EffectiveMinRange(LocalTargetInfo target, Thing caster)
		{
			return this.EffectiveMinRange(VerbUtility.AllowAdjacentShot(target, caster));
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001DA84 File Offset: 0x0001BC84
		public float EffectiveMinRange(bool allowAdjacentShot)
		{
			float num = this.minRange;
			if (!allowAdjacentShot && !this.IsMeleeAttack && this.LaunchesProjectile)
			{
				num = Mathf.Max(num, 1.421f);
			}
			return num;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001DAB8 File Offset: 0x0001BCB8
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

		// Token: 0x060005CB RID: 1483 RVA: 0x0001DB80 File Offset: 0x0001BD80
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

		// Token: 0x060005CC RID: 1484 RVA: 0x0001DC40 File Offset: 0x0001BE40
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

		// Token: 0x060005CD RID: 1485 RVA: 0x0001DCAB File Offset: 0x0001BEAB
		public new VerbProperties MemberwiseClone()
		{
			return (VerbProperties)base.MemberwiseClone();
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001DCB8 File Offset: 0x0001BEB8
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

		// Token: 0x060005CF RID: 1487 RVA: 0x0001DCCF File Offset: 0x0001BECF
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x040003A8 RID: 936
		public VerbCategory category = VerbCategory.Misc;

		// Token: 0x040003A9 RID: 937
		[TranslationHandle]
		public Type verbClass = typeof(Verb);

		// Token: 0x040003AA RID: 938
		[MustTranslate]
		public string label;

		// Token: 0x040003AB RID: 939
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x040003AC RID: 940
		public bool isPrimary = true;

		// Token: 0x040003AD RID: 941
		public bool violent = true;

		// Token: 0x040003AE RID: 942
		public float minRange;

		// Token: 0x040003AF RID: 943
		public float range = 1.42f;

		// Token: 0x040003B0 RID: 944
		public int burstShotCount = 1;

		// Token: 0x040003B1 RID: 945
		public int ticksBetweenBurstShots = 15;

		// Token: 0x040003B2 RID: 946
		public float noiseRadius = 3f;

		// Token: 0x040003B3 RID: 947
		public bool hasStandardCommand;

		// Token: 0x040003B4 RID: 948
		public bool targetable = true;

		// Token: 0x040003B5 RID: 949
		public bool nonInterruptingSelfCast;

		// Token: 0x040003B6 RID: 950
		public TargetingParameters targetParams = new TargetingParameters();

		// Token: 0x040003B7 RID: 951
		public bool requireLineOfSight = true;

		// Token: 0x040003B8 RID: 952
		public bool mustCastOnOpenGround;

		// Token: 0x040003B9 RID: 953
		public bool forceNormalTimeSpeed = true;

		// Token: 0x040003BA RID: 954
		public bool onlyManualCast;

		// Token: 0x040003BB RID: 955
		public bool stopBurstWithoutLos = true;

		// Token: 0x040003BC RID: 956
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x040003BD RID: 957
		public float commonality = 1f;

		// Token: 0x040003BE RID: 958
		public Intelligence minIntelligence;

		// Token: 0x040003BF RID: 959
		public float consumeFuelPerShot;

		// Token: 0x040003C0 RID: 960
		public bool stunTargetOnCastStart;

		// Token: 0x040003C1 RID: 961
		public float warmupTime;

		// Token: 0x040003C2 RID: 962
		public float defaultCooldownTime;

		// Token: 0x040003C3 RID: 963
		public string commandIcon;

		// Token: 0x040003C4 RID: 964
		public SoundDef soundCast;

		// Token: 0x040003C5 RID: 965
		public SoundDef soundCastTail;

		// Token: 0x040003C6 RID: 966
		public SoundDef soundAiming;

		// Token: 0x040003C7 RID: 967
		public float muzzleFlashScale;

		// Token: 0x040003C8 RID: 968
		public ThingDef impactMote;

		// Token: 0x040003C9 RID: 969
		public FleckDef impactFleck;

		// Token: 0x040003CA RID: 970
		public bool drawAimPie = true;

		// Token: 0x040003CB RID: 971
		public EffecterDef warmupEffecter;

		// Token: 0x040003CC RID: 972
		public bool drawHighlightWithLineOfSight;

		// Token: 0x040003CD RID: 973
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x040003CE RID: 974
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x040003CF RID: 975
		public DamageDef meleeDamageDef;

		// Token: 0x040003D0 RID: 976
		public int meleeDamageBaseAmount = 1;

		// Token: 0x040003D1 RID: 977
		public float meleeArmorPenetrationBase = -1f;

		// Token: 0x040003D2 RID: 978
		public bool ai_IsWeapon = true;

		// Token: 0x040003D3 RID: 979
		public bool ai_IsBuildingDestroyer;

		// Token: 0x040003D4 RID: 980
		public float ai_AvoidFriendlyFireRadius;

		// Token: 0x040003D5 RID: 981
		public ThingDef defaultProjectile;

		// Token: 0x040003D6 RID: 982
		private float forcedMissRadius;

		// Token: 0x040003D7 RID: 983
		private float forcedMissRadiusClassicMortars = -1f;

		// Token: 0x040003D8 RID: 984
		private bool isMortar;

		// Token: 0x040003D9 RID: 985
		public float accuracyTouch = 1f;

		// Token: 0x040003DA RID: 986
		public float accuracyShort = 1f;

		// Token: 0x040003DB RID: 987
		public float accuracyMedium = 1f;

		// Token: 0x040003DC RID: 988
		public float accuracyLong = 1f;

		// Token: 0x040003DD RID: 989
		public ThingDef spawnDef;

		// Token: 0x040003DE RID: 990
		public TaleDef colonyWideTaleDef;

		// Token: 0x040003DF RID: 991
		public BodyPartTagDef bodypartTagTarget;

		// Token: 0x040003E0 RID: 992
		public RulePackDef rangedFireRulepack;

		// Token: 0x040003E1 RID: 993
		public const float DefaultArmorPenetrationPerDamage = 0.015f;

		// Token: 0x040003E2 RID: 994
		private const float VerbSelectionWeightFactor_BodyPart = 0.3f;

		// Token: 0x040003E3 RID: 995
		private const float MinLinkedBodyPartGroupEfficiencyIfMustBeAlwaysUsable = 0.4f;

		// Token: 0x020018CF RID: 6351
		private enum RangeCategory : byte
		{
			// Token: 0x04005EFB RID: 24315
			Touch,
			// Token: 0x04005EFC RID: 24316
			Short,
			// Token: 0x04005EFD RID: 24317
			Medium,
			// Token: 0x04005EFE RID: 24318
			Long
		}
	}
}
