using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0A RID: 3850
	public class CompProperties_Explosive : CompProperties
	{
		// Token: 0x06005531 RID: 21809 RVA: 0x001C7BDC File Offset: 0x001C5DDC
		public CompProperties_Explosive()
		{
			this.compClass = typeof(CompExplosive);
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x0003B132 File Offset: 0x00039332
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.explosiveDamageType == null)
			{
				this.explosiveDamageType = DamageDefOf.Bomb;
			}
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x0003B14E File Offset: 0x0003934E
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

		// Token: 0x04003632 RID: 13874
		public float explosiveRadius = 1.9f;

		// Token: 0x04003633 RID: 13875
		public DamageDef explosiveDamageType;

		// Token: 0x04003634 RID: 13876
		public int damageAmountBase = -1;

		// Token: 0x04003635 RID: 13877
		public float armorPenetrationBase = -1f;

		// Token: 0x04003636 RID: 13878
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04003637 RID: 13879
		public float postExplosionSpawnChance;

		// Token: 0x04003638 RID: 13880
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04003639 RID: 13881
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x0400363A RID: 13882
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x0400363B RID: 13883
		public float preExplosionSpawnChance;

		// Token: 0x0400363C RID: 13884
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x0400363D RID: 13885
		public float chanceToStartFire;

		// Token: 0x0400363E RID: 13886
		public bool damageFalloff;

		// Token: 0x0400363F RID: 13887
		public bool explodeOnKilled;

		// Token: 0x04003640 RID: 13888
		public float explosiveExpandPerStackcount;

		// Token: 0x04003641 RID: 13889
		public float explosiveExpandPerFuel;

		// Token: 0x04003642 RID: 13890
		public EffecterDef explosionEffect;

		// Token: 0x04003643 RID: 13891
		public SoundDef explosionSound;

		// Token: 0x04003644 RID: 13892
		public List<DamageDef> startWickOnDamageTaken;

		// Token: 0x04003645 RID: 13893
		public float startWickHitPointsPercent = 0.2f;

		// Token: 0x04003646 RID: 13894
		public IntRange wickTicks = new IntRange(140, 150);

		// Token: 0x04003647 RID: 13895
		public float wickScale = 1f;

		// Token: 0x04003648 RID: 13896
		public float chanceNeverExplodeFromDamage;

		// Token: 0x04003649 RID: 13897
		public float destroyThingOnExplosionSize;

		// Token: 0x0400364A RID: 13898
		public DamageDef requiredDamageTypeToExplode;

		// Token: 0x0400364B RID: 13899
		public IntRange? countdownTicks;

		// Token: 0x0400364C RID: 13900
		public string extraInspectStringKey;
	}
}
