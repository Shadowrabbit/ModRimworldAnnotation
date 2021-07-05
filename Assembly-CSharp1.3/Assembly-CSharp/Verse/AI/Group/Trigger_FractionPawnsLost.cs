using System;

namespace Verse.AI.Group
{
	// Token: 0x02000698 RID: 1688
	public class Trigger_FractionPawnsLost : Trigger
	{
		// Token: 0x06002F3A RID: 12090 RVA: 0x0011861D File Offset: 0x0011681D
		public Trigger_FractionPawnsLost(float fraction)
		{
			this.fraction = fraction;
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x00118637 File Offset: 0x00116837
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (float)lord.numPawnsLostViolently >= (float)lord.numPawnsEverGained * this.fraction;
		}

		// Token: 0x04001CE2 RID: 7394
		private float fraction = 0.5f;
	}
}
