using System;

namespace Verse.AI.Group
{
	// Token: 0x0200069D RID: 1693
	public class Trigger_ChanceOnTickInteval : Trigger
	{
		// Token: 0x06002F44 RID: 12100 RVA: 0x00118725 File Offset: 0x00116925
		public Trigger_ChanceOnTickInteval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x0011873B File Offset: 0x0011693B
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.interval == 0 && Rand.Value < this.chancePerInterval;
		}

		// Token: 0x04001CE8 RID: 7400
		private float chancePerInterval;

		// Token: 0x04001CE9 RID: 7401
		private int interval;
	}
}
