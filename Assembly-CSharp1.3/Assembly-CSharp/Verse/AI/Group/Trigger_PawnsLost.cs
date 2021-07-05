using System;

namespace Verse.AI.Group
{
	// Token: 0x02000699 RID: 1689
	public class Trigger_PawnsLost : Trigger
	{
		// Token: 0x06002F3C RID: 12092 RVA: 0x0011865E File Offset: 0x0011685E
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x00118674 File Offset: 0x00116874
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}

		// Token: 0x04001CE3 RID: 7395
		private int count = 1;
	}
}
