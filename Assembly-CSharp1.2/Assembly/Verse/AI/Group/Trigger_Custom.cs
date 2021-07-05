using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AF8 RID: 2808
	public class Trigger_Custom : Trigger
	{
		// Token: 0x06004209 RID: 16905 RVA: 0x00031265 File Offset: 0x0002F465
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x00031274 File Offset: 0x0002F474
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}

		// Token: 0x04002D55 RID: 11605
		private Func<TriggerSignal, bool> condition;
	}
}
