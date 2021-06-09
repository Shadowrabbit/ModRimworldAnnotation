using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AFF RID: 2815
	public class Trigger_ChanceOnTickInteval : Trigger
	{
		// Token: 0x06004217 RID: 16919 RVA: 0x000313D2 File Offset: 0x0002F5D2
		public Trigger_ChanceOnTickInteval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x000313E8 File Offset: 0x0002F5E8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.interval == 0 && Rand.Value < this.chancePerInterval;
		}

		// Token: 0x04002D5E RID: 11614
		private float chancePerInterval;

		// Token: 0x04002D5F RID: 11615
		private int interval;
	}
}
