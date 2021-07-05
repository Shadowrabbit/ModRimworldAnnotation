using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A7 RID: 167
	public class ProjectileProperties
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001B8C3 File Offset: 0x00019AC3
		public float StoppingPower
		{
			get
			{
				if (this.stoppingPower != 0f)
				{
					return this.stoppingPower;
				}
				if (this.damageDef != null)
				{
					return this.damageDef.defaultStoppingPower;
				}
				return 0f;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0001B8F2 File Offset: 0x00019AF2
		public float SpeedTilesPerTick
		{
			get
			{
				return this.speed / 100f;
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001B900 File Offset: 0x00019B00
		public int GetDamageAmount(Thing weapon, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true) : 1f;
			return this.GetDamageAmount(weaponDamageMultiplier, explanation);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001B92C File Offset: 0x00019B2C
		public int GetDamageAmount(ThingDef weapon, ThingDef weaponStuff, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValueAbstract(StatDefOf.RangedWeapon_DamageMultiplier, weaponStuff) : 1f;
			return this.GetDamageAmount(weaponDamageMultiplier, explanation);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001B958 File Offset: 0x00019B58
		public int GetDamageAmount(float weaponDamageMultiplier, StringBuilder explanation = null)
		{
			int num;
			if (this.damageAmountBase != -1)
			{
				num = this.damageAmountBase;
			}
			else
			{
				if (this.damageDef == null)
				{
					Log.ErrorOnce("Failed to find sane damage amount", 91094882);
					return 1;
				}
				num = this.damageDef.defaultDamage;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + num);
				explanation.Append("StatsReport_QualityMultiplier".Translate() + ": " + weaponDamageMultiplier.ToStringPercent());
			}
			num = Mathf.RoundToInt((float)num * weaponDamageMultiplier);
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("StatsReport_FinalValue".Translate() + ": " + num);
			}
			return num;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001BA40 File Offset: 0x00019C40
		public float GetArmorPenetration(Thing weapon, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true) : 1f;
			return this.GetArmorPenetration(weaponDamageMultiplier, explanation);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001BA6C File Offset: 0x00019C6C
		public float GetArmorPenetration(float weaponDamageMultiplier, StringBuilder explanation = null)
		{
			if (this.damageDef.armorCategory == null)
			{
				return 0f;
			}
			float num;
			if (this.damageAmountBase != -1 || this.armorPenetrationBase >= 0f)
			{
				num = this.armorPenetrationBase;
			}
			else
			{
				if (this.damageDef == null)
				{
					return 0f;
				}
				num = this.damageDef.defaultArmorPenetration;
			}
			if (num < 0f)
			{
				num = (float)this.GetDamageAmount(null, null) * 0.015f;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToStringPercent());
				explanation.AppendLine();
				explanation.Append("StatsReport_QualityMultiplier".Translate() + ": " + weaponDamageMultiplier.ToStringPercent());
			}
			num *= weaponDamageMultiplier;
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("StatsReport_FinalValue".Translate() + ": " + num.ToStringPercent());
			}
			return num;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001BB80 File Offset: 0x00019D80
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (this.alwaysFreeIntercept && this.flyOverhead)
			{
				yield return "alwaysFreeIntercept and flyOverhead are both true";
			}
			if (this.damageAmountBase == -1 && this.damageDef != null && this.damageDef.defaultDamage == -1)
			{
				yield return "no damage amount specified for projectile";
			}
			yield break;
		}

		// Token: 0x040002D8 RID: 728
		public float speed = 5f;

		// Token: 0x040002D9 RID: 729
		public bool flyOverhead;

		// Token: 0x040002DA RID: 730
		public bool alwaysFreeIntercept;

		// Token: 0x040002DB RID: 731
		public DamageDef damageDef;

		// Token: 0x040002DC RID: 732
		private int damageAmountBase = -1;

		// Token: 0x040002DD RID: 733
		private float armorPenetrationBase = -1f;

		// Token: 0x040002DE RID: 734
		public float stoppingPower = 0.5f;

		// Token: 0x040002DF RID: 735
		public List<ExtraDamage> extraDamages;

		// Token: 0x040002E0 RID: 736
		public float arcHeightFactor;

		// Token: 0x040002E1 RID: 737
		public float shadowSize;

		// Token: 0x040002E2 RID: 738
		public SoundDef soundHitThickRoof;

		// Token: 0x040002E3 RID: 739
		public SoundDef soundExplode;

		// Token: 0x040002E4 RID: 740
		public SoundDef soundImpactAnticipate;

		// Token: 0x040002E5 RID: 741
		public SoundDef soundAmbient;

		// Token: 0x040002E6 RID: 742
		public float explosionRadius;

		// Token: 0x040002E7 RID: 743
		public int explosionDelay;

		// Token: 0x040002E8 RID: 744
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x040002E9 RID: 745
		public float preExplosionSpawnChance = 1f;

		// Token: 0x040002EA RID: 746
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x040002EB RID: 747
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x040002EC RID: 748
		public float postExplosionSpawnChance = 1f;

		// Token: 0x040002ED RID: 749
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x040002EE RID: 750
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x040002EF RID: 751
		public float explosionChanceToStartFire;

		// Token: 0x040002F0 RID: 752
		public bool explosionDamageFalloff;

		// Token: 0x040002F1 RID: 753
		public EffecterDef explosionEffect;

		// Token: 0x040002F2 RID: 754
		public bool ai_IsIncendiary;
	}
}
