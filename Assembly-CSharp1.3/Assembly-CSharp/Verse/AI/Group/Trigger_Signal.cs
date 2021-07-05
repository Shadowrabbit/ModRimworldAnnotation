using System;

namespace Verse.AI.Group
{
	// Token: 0x0200069B RID: 1691
	public class Trigger_Signal : Trigger
	{
		// Token: 0x06002F40 RID: 12096 RVA: 0x001186C7 File Offset: 0x001168C7
		public Trigger_Signal(string signal)
		{
			this.signal = signal;
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x001186D6 File Offset: 0x001168D6
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Signal && signal.signal.tag == this.signal;
		}

		// Token: 0x04001CE6 RID: 7398
		private string signal;
	}
}
