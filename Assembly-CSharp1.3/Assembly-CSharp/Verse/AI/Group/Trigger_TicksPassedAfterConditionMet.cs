using System;

namespace Verse.AI.Group
{
	// Token: 0x02000693 RID: 1683
	public class Trigger_TicksPassedAfterConditionMet : Trigger_TicksPassed
	{
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06002F2E RID: 12078 RVA: 0x001184E9 File Offset: 0x001166E9
		protected new TriggerData_TicksPassedAfterConditionMet Data
		{
			get
			{
				return (TriggerData_TicksPassedAfterConditionMet)this.data;
			}
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x001184F6 File Offset: 0x001166F6
		public Trigger_TicksPassedAfterConditionMet(int tickLimit, Func<bool> condition, int checkEveryTicks = 1) : base(tickLimit)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
			this.data = new TriggerData_TicksPassedAfterConditionMet();
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x00118518 File Offset: 0x00116718
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (!this.Data.conditionMet && signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0)
			{
				this.Data.conditionMet = this.condition();
			}
			return this.Data.conditionMet && base.ActivateOn(lord, signal);
		}

		// Token: 0x04001CDB RID: 7387
		private Func<bool> condition;

		// Token: 0x04001CDC RID: 7388
		private int checkEveryTicks;
	}
}
