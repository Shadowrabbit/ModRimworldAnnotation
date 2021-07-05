using System;

namespace Verse.AI.Group
{
	// Token: 0x02000697 RID: 1687
	public class Trigger_TickCondition : Trigger
	{
		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06002F37 RID: 12087 RVA: 0x001185C8 File Offset: 0x001167C8
		private TriggerData_Reason Data
		{
			get
			{
				return (TriggerData_Reason)this.data;
			}
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x001185D5 File Offset: 0x001167D5
		public Trigger_TickCondition(Func<bool> condition, int checkEveryTicks = 1)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x001185F2 File Offset: 0x001167F2
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0 && this.condition();
		}

		// Token: 0x04001CE0 RID: 7392
		private Func<bool> condition;

		// Token: 0x04001CE1 RID: 7393
		private int checkEveryTicks = 1;
	}
}
