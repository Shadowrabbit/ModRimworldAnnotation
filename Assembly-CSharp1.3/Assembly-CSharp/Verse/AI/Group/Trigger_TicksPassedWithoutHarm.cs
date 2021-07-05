using System;

namespace Verse.AI.Group
{
	// Token: 0x02000691 RID: 1681
	public class Trigger_TicksPassedWithoutHarm : Trigger_TicksPassed
	{
		// Token: 0x06002F2A RID: 12074 RVA: 0x00118455 File Offset: 0x00116655
		public Trigger_TicksPassedWithoutHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x00118485 File Offset: 0x00116685
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
