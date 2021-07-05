using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AFB RID: 2811
	public class Trigger_PawnsLost : Trigger
	{
		// Token: 0x0600420F RID: 16911 RVA: 0x0003130B File Offset: 0x0002F50B
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x00031321 File Offset: 0x0002F521
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}

		// Token: 0x04002D59 RID: 11609
		private int count = 1;
	}
}
