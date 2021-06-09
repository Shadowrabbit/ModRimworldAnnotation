using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AF2 RID: 2802
	public class TriggerData_TicksPassed : TriggerData
	{
		// Token: 0x060041FC RID: 16892 RVA: 0x0003116A File Offset: 0x0002F36A
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x04002D4F RID: 11599
		public int ticksPassed;
	}
}
