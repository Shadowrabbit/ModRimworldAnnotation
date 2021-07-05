using System;

namespace Verse.AI.Group
{
	// Token: 0x0200068D RID: 1677
	public class TriggerData_TicksPassed : TriggerData
	{
		// Token: 0x06002F20 RID: 12064 RVA: 0x00118378 File Offset: 0x00116578
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x04001CD5 RID: 7381
		public int ticksPassed;
	}
}
