using System;

namespace Verse.AI.Group
{
	// Token: 0x02000695 RID: 1685
	public class Trigger_Custom : Trigger
	{
		// Token: 0x06002F33 RID: 12083 RVA: 0x00118597 File Offset: 0x00116797
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x001185A6 File Offset: 0x001167A6
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}

		// Token: 0x04001CDE RID: 7390
		private Func<TriggerSignal, bool> condition;
	}
}
