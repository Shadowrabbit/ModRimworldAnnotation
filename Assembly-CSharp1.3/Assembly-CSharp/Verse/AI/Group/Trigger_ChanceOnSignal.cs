using System;

namespace Verse.AI.Group
{
	// Token: 0x0200069A RID: 1690
	public class Trigger_ChanceOnSignal : Trigger
	{
		// Token: 0x06002F3E RID: 12094 RVA: 0x00118692 File Offset: 0x00116892
		public Trigger_ChanceOnSignal(TriggerSignalType signalType, float chance)
		{
			this.signalType = signalType;
			this.chance = chance;
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x001186A8 File Offset: 0x001168A8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == this.signalType && Rand.Value < this.chance;
		}

		// Token: 0x04001CE4 RID: 7396
		private TriggerSignalType signalType;

		// Token: 0x04001CE5 RID: 7397
		private float chance;
	}
}
