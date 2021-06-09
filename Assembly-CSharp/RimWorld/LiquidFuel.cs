using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001725 RID: 5925
	public class LiquidFuel : Filth
	{
		// Token: 0x060082B2 RID: 33458 RVA: 0x00057BEB File Offset: 0x00055DEB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x060082B3 RID: 33459 RVA: 0x00057C05 File Offset: 0x00055E05
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060082B4 RID: 33460 RVA: 0x00057C1F File Offset: 0x00055E1F
		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060082B5 RID: 33461 RVA: 0x00057C31 File Offset: 0x00055E31
		public override void Tick()
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060082B6 RID: 33462 RVA: 0x00057C52 File Offset: 0x00055E52
		public override void ThickenFilth()
		{
			base.ThickenFilth();
			this.Refill();
		}

		// Token: 0x040054BB RID: 21691
		private int spawnTick;

		// Token: 0x040054BC RID: 21692
		private const int DryOutTime = 1500;
	}
}
