using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200010D RID: 269
	public class ProjectileProperties
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0000BFDD File Offset: 0x0000A1DD
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

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x0000C00C File Offset: 0x0000A20C
		public float SpeedTilesPerTick
		{
			get
			{
				return this.speed / 100f;
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00092010 File Offset: 0x00090210
		public int GetDamageAmount(Thing weapon, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true) : 1f;
			return this.GetDamageAmount(weaponDamageMultiplier, explanation);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0009203C File Offset: 0x0009023C
		public int GetDamageAmount_NewTmp(ThingDef weapon, ThingDef weaponStuff, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValueAbstract(StatDefOf.RangedWeapon_DamageMultiplier, weaponStuff) : 1f;
			return this.GetDamageAmount(weaponDamageMultiplier, explanation);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00092068 File Offset: 0x00090268
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
					Log.ErrorOnce("Failed to find sane damage amount", 91094882, false);
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

		// Token: 0x0600076E RID: 1902 RVA: 0x00092154 File Offset: 0x00090354
		public float GetArmorPenetration(Thing weapon, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true) : 1f;
			return this.GetArmorPenetration(weaponDamageMultiplier, explanation);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00092180 File Offset: 0x00090380
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

		// Token: 0x06000770 RID: 1904 RVA: 0x0000C01A File Offset: 0x0000A21A
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

		// Token: 0x040004BA RID: 1210
		public float speed = 5f;

		// Token: 0x040004BB RID: 1211
		public bool flyOverhead;

		// Token: 0x040004BC RID: 1212
		public bool alwaysFreeIntercept;

		// Token: 0x040004BD RID: 1213
		public DamageDef damageDef;

		// Token: 0x040004BE RID: 1214
		private int damageAmountBase = -1;

		// Token: 0x040004BF RID: 1215
		private float armorPenetrationBase = -1f;

		// Token: 0x040004C0 RID: 1216
		public float stoppingPower = 0.5f;

		// Token: 0x040004C1 RID: 1217
		public List<ExtraDamage> extraDamages;

		// Token: 0x040004C2 RID: 1218
		public float arcHeightFactor;

		// Token: 0x040004C3 RID: 1219
		public float shadowSize;

		// Token: 0x040004C4 RID: 1220
		public SoundDef soundHitThickRoof;

		// Token: 0x040004C5 RID: 1221
		public SoundDef soundExplode;

		// Token: 0x040004C6 RID: 1222
		public SoundDef soundImpactAnticipate;

		// Token: 0x040004C7 RID: 1223
		public SoundDef soundAmbient;

		// Token: 0x040004C8 RID: 1224
		public float explosionRadius;

		// Token: 0x040004C9 RID: 1225
		public int explosionDelay;

		// Token: 0x040004CA RID: 1226
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x040004CB RID: 1227
		public float preExplosionSpawnChance = 1f;

		// Token: 0x040004CC RID: 1228
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x040004CD RID: 1229
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x040004CE RID: 1230
		public float postExplosionSpawnChance = 1f;

		// Token: 0x040004CF RID: 1231
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x040004D0 RID: 1232
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x040004D1 RID: 1233
		public float explosionChanceToStartFire;

		// Token: 0x040004D2 RID: 1234
		public bool explosionDamageFalloff;

		// Token: 0x040004D3 RID: 1235
		public EffecterDef explosionEffect;

		// Token: 0x040004D4 RID: 1236
		public bool ai_IsIncendiary;
	}
}
