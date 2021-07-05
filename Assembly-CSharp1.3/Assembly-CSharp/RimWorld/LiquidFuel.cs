using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C3 RID: 4291
	public class LiquidFuel : Filth
	{
		// Token: 0x060066B2 RID: 26290 RVA: 0x0022B31C File Offset: 0x0022951C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x060066B3 RID: 26291 RVA: 0x0022B336 File Offset: 0x00229536
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060066B4 RID: 26292 RVA: 0x0022B350 File Offset: 0x00229550
		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060066B5 RID: 26293 RVA: 0x0022B362 File Offset: 0x00229562
		public override void Tick()
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060066B6 RID: 26294 RVA: 0x0022B383 File Offset: 0x00229583
		public override void ThickenFilth()
		{
			base.ThickenFilth();
			this.Refill();
		}

		// Token: 0x040039FA RID: 14842
		private int spawnTick;

		// Token: 0x040039FB RID: 14843
		private const int DryOutTime = 1500;
	}
}
