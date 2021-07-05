using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001674 RID: 5748
	public class Mineable : Building
	{
		// Token: 0x06007D5B RID: 32091 RVA: 0x00054420 File Offset: 0x00052620
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.yieldPct, "yieldPct", 0f, false);
		}

		// Token: 0x06007D5C RID: 32092 RVA: 0x00256C4C File Offset: 0x00254E4C
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (this.def.building.mineableThing != null && this.def.building.mineableYieldWasteable && dinfo.Def == DamageDefOf.Mining && dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.Notify_TookMiningDamage(GenMath.RoundRandom(dinfo.Amount), (Pawn)dinfo.Instigator);
			}
			absorbed = false;
		}

		// Token: 0x06007D5D RID: 32093 RVA: 0x00256CCC File Offset: 0x00254ECC
		public void DestroyMined(Pawn pawn)
		{
			Map map = base.Map;
			base.Destroy(DestroyMode.KillFinalize);
			this.TrySpawnYield(map, this.yieldPct, true, pawn);
		}

		// Token: 0x06007D5E RID: 32094 RVA: 0x00256CF8 File Offset: 0x00254EF8
		public override void Destroy(DestroyMode mode)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				this.TrySpawnYield(map, 0.2f, false, null);
			}
		}

		// Token: 0x06007D5F RID: 32095 RVA: 0x00256D28 File Offset: 0x00254F28
		private void TrySpawnYield(Map map, float yieldChance, bool moteOnWaste, Pawn pawn)
		{
			Mineable.<>c__DisplayClass6_0 CS$<>8__locals1 = new Mineable.<>c__DisplayClass6_0();
			CS$<>8__locals1.pawn = pawn;
			if (this.def.building.mineableThing == null)
			{
				return;
			}
			if (Rand.Value > this.def.building.mineableDropChance)
			{
				return;
			}
			int num = Mathf.Max(1, this.def.building.EffectiveMineableYield);
			if (this.def.building.mineableYieldWasteable)
			{
				num = Mathf.Max(1, GenMath.RoundRandom((float)num * this.yieldPct));
			}
			Thing thing = ThingMaker.MakeThing(this.def.building.mineableThing, null);
			thing.stackCount = num;
			GenPlace.TryPlaceThing(thing, base.Position, map, ThingPlaceMode.Near, new Action<Thing, int>(CS$<>8__locals1.<TrySpawnYield>g__ForbidIfNecessary|0), null, default(Rot4));
		}

		// Token: 0x06007D60 RID: 32096 RVA: 0x00256DF0 File Offset: 0x00254FF0
		public void Notify_TookMiningDamage(int amount, Pawn miner)
		{
			float num = (float)Mathf.Min(amount, this.HitPoints) / (float)base.MaxHitPoints;
			this.yieldPct += num * miner.GetStatValue(StatDefOf.MiningYield, true);
		}

		// Token: 0x040051BD RID: 20925
		private float yieldPct;

		// Token: 0x040051BE RID: 20926
		private const float YieldChanceOnNonMiningKill = 0.2f;
	}
}
