using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AFA RID: 2810
	public class Trigger_FractionPawnsLost : Trigger
	{
		// Token: 0x0600420D RID: 16909 RVA: 0x000312CA File Offset: 0x0002F4CA
		public Trigger_FractionPawnsLost(float fraction)
		{
			this.fraction = fraction;
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x000312E4 File Offset: 0x0002F4E4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (float)lord.numPawnsLostViolently >= (float)lord.numPawnsEverGained * this.fraction;
		}

		// Token: 0x04002D58 RID: 11608
		private float fraction = 0.5f;
	}
}
