using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AF6 RID: 2806
	public class Trigger_TicksPassedAfterConditionMet : Trigger_TicksPassed
	{
		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06004204 RID: 16900 RVA: 0x0003121A File Offset: 0x0002F41A
		protected new TriggerData_TicksPassedAfterConditionMet Data
		{
			get
			{
				return (TriggerData_TicksPassedAfterConditionMet)this.data;
			}
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x00031227 File Offset: 0x0002F427
		public Trigger_TicksPassedAfterConditionMet(int tickLimit, Func<bool> condition, int checkEveryTicks = 1) : base(tickLimit)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
			this.data = new TriggerData_TicksPassedAfterConditionMet();
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x00188E34 File Offset: 0x00187034
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (!this.Data.conditionMet && signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0)
			{
				this.Data.conditionMet = this.condition();
			}
			return this.Data.conditionMet && base.ActivateOn(lord, signal);
		}

		// Token: 0x04002D52 RID: 11602
		private Func<bool> condition;

		// Token: 0x04002D53 RID: 11603
		private int checkEveryTicks;
	}
}
