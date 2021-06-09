using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AFD RID: 2813
	public class Trigger_Signal : Trigger
	{
		// Token: 0x06004213 RID: 16915 RVA: 0x00031374 File Offset: 0x0002F574
		public Trigger_Signal(string signal)
		{
			this.signal = signal;
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x00031383 File Offset: 0x0002F583
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Signal && signal.signal.tag == this.signal;
		}

		// Token: 0x04002D5C RID: 11612
		private string signal;
	}
}
