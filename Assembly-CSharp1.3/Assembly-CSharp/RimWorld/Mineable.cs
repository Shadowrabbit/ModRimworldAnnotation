using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001049 RID: 4169
	public class Mineable : Building
	{
		// Token: 0x06006293 RID: 25235 RVA: 0x00216A9C File Offset: 0x00214C9C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.yieldPct, "yieldPct", 0f, false);
		}

		// Token: 0x06006294 RID: 25236 RVA: 0x00216ABC File Offset: 0x00214CBC
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

		// Token: 0x06006295 RID: 25237 RVA: 0x00216B3C File Offset: 0x00214D3C
		public void DestroyMined(Pawn pawn)
		{
			Map map = base.Map;
			base.Destroy(DestroyMode.KillFinalize);
			this.TrySpawnYield(map, this.yieldPct, true, pawn);
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.Mined, pawn.Named(HistoryEventArgsNames.Doer)), true);
		}

		// Token: 0x06006296 RID: 25238 RVA: 0x00216B88 File Offset: 0x00214D88
		public override void Destroy(DestroyMode mode)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				this.TrySpawnYield(map, 0.2f, false, null);
			}
		}

		// Token: 0x06006297 RID: 25239 RVA: 0x00216BB8 File Offset: 0x00214DB8
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

		// Token: 0x06006298 RID: 25240 RVA: 0x00216C80 File Offset: 0x00214E80
		public void Notify_TookMiningDamage(int amount, Pawn miner)
		{
			float num = (float)Mathf.Min(amount, this.HitPoints) / (float)base.MaxHitPoints;
			this.yieldPct += num * miner.GetStatValue(StatDefOf.MiningYield, true);
		}

		// Token: 0x040037ED RID: 14317
		private float yieldPct;

		// Token: 0x040037EE RID: 14318
		private const float YieldChanceOnNonMiningKill = 0.2f;
	}
}
