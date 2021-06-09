using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AFC RID: 2812
	public class Trigger_ChanceOnSignal : Trigger
	{
		// Token: 0x06004211 RID: 16913 RVA: 0x0003133F File Offset: 0x0002F53F
		public Trigger_ChanceOnSignal(TriggerSignalType signalType, float chance)
		{
			this.signalType = signalType;
			this.chance = chance;
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x00031355 File Offset: 0x0002F555
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == this.signalType && Rand.Value < this.chance;
		}

		// Token: 0x04002D5A RID: 11610
		private TriggerSignalType signalType;

		// Token: 0x04002D5B RID: 11611
		private float chance;
	}
}
