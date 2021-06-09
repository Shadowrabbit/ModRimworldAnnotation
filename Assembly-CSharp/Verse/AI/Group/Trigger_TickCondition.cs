using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AF9 RID: 2809
	public class Trigger_TickCondition : Trigger
	{
		// Token: 0x0600420B RID: 16907 RVA: 0x00031282 File Offset: 0x0002F482
		public Trigger_TickCondition(Func<bool> condition, int checkEveryTicks = 1)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x0003129F File Offset: 0x0002F49F
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0 && this.condition();
		}

		// Token: 0x04002D56 RID: 11606
		private Func<bool> condition;

		// Token: 0x04002D57 RID: 11607
		private int checkEveryTicks = 1;
	}
}
