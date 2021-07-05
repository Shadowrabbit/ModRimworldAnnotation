using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F2 RID: 2290
	public class ShipJob_WaitTime : ShipJob_Wait
	{
		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003C09 RID: 15369 RVA: 0x0014EAFB File Offset: 0x0014CCFB
		protected override bool ShouldEnd
		{
			get
			{
				return this.startTick >= 0 && Find.TickManager.TicksGame >= this.startTick + this.duration;
			}
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x0014EB24 File Offset: 0x0014CD24
		public override bool TryStart()
		{
			if (!this.transportShip.ShipExistsAndIsSpawned)
			{
				return false;
			}
			if (!base.TryStart())
			{
				return false;
			}
			if (this.startTick < 0)
			{
				this.startTick = Find.TickManager.TicksGame;
			}
			return true;
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x0014EB59 File Offset: 0x0014CD59
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x040020A4 RID: 8356
		public int duration;

		// Token: 0x040020A5 RID: 8357
		private int startTick = -1;
	}
}
