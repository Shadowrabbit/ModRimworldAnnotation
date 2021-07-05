using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009FF RID: 2559
	public class CompProperties_Explosive : CompProperties
	{
		// Token: 0x06003EE6 RID: 16102 RVA: 0x00157AA0 File Offset: 0x00155CA0
		public CompProperties_Explosive()
		{
			this.compClass = typeof(CompExplosive);
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x00157B19 File Offset: 0x00155D19
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.explosiveDamageType == null)
			{
				this.explosiveDamageType = DamageDefOf.Bomb;
			}
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x00157B35 File Offset: 0x00155D35
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return "CompExplosive requires Normal ticker type";
			}
			yield break;
			yield break;
		}

		// Token: 0x040021B3 RID: 8627
		public float explosiveRadius = 1.9f;

		// Token: 0x040021B4 RID: 8628
		public DamageDef explosiveDamageType;

		// Token: 0x040021B5 RID: 8629
		public int damageAmountBase = -1;

		// Token: 0x040021B6 RID: 8630
		public float armorPenetrationBase = -1f;

		// Token: 0x040021B7 RID: 8631
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x040021B8 RID: 8632
		public float postExplosionSpawnChance;

		// Token: 0x040021B9 RID: 8633
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x040021BA RID: 8634
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x040021BB RID: 8635
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x040021BC RID: 8636
		public float preExplosionSpawnChance;

		// Token: 0x040021BD RID: 8637
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x040021BE RID: 8638
		public float chanceToStartFire;

		// Token: 0x040021BF RID: 8639
		public bool damageFalloff;

		// Token: 0x040021C0 RID: 8640
		public bool explodeOnKilled;

		// Token: 0x040021C1 RID: 8641
		public float explosiveExpandPerStackcount;

		// Token: 0x040021C2 RID: 8642
		public float explosiveExpandPerFuel;

		// Token: 0x040021C3 RID: 8643
		public EffecterDef explosionEffect;

		// Token: 0x040021C4 RID: 8644
		public SoundDef explosionSound;

		// Token: 0x040021C5 RID: 8645
		public List<DamageDef> startWickOnDamageTaken;

		// Token: 0x040021C6 RID: 8646
		public float startWickHitPointsPercent = 0.2f;

		// Token: 0x040021C7 RID: 8647
		public IntRange wickTicks = new IntRange(140, 150);

		// Token: 0x040021C8 RID: 8648
		public float wickScale = 1f;

		// Token: 0x040021C9 RID: 8649
		public float chanceNeverExplodeFromDamage;

		// Token: 0x040021CA RID: 8650
		public float destroyThingOnExplosionSize;

		// Token: 0x040021CB RID: 8651
		public DamageDef requiredDamageTypeToExplode;

		// Token: 0x040021CC RID: 8652
		public IntRange? countdownTicks;

		// Token: 0x040021CD RID: 8653
		public string extraInspectStringKey;

		// Token: 0x040021CE RID: 8654
		public List<WickMessage> wickMessages;
	}
}
