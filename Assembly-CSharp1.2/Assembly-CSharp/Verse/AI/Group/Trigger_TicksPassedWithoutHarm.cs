using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AF4 RID: 2804
	public class Trigger_TicksPassedWithoutHarm : Trigger_TicksPassed
	{
		// Token: 0x06004200 RID: 16896 RVA: 0x00031186 File Offset: 0x0002F386
		public Trigger_TicksPassedWithoutHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x000311B6 File Offset: 0x0002F3B6
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				base.Data.ticksPassed = 0;
			}
			return base.ActivateOn(lord, signal);
		}
	}
}
