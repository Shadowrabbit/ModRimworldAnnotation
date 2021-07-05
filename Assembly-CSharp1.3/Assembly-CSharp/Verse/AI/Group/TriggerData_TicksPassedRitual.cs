using System;

namespace Verse.AI.Group
{
	// Token: 0x0200068F RID: 1679
	public class TriggerData_TicksPassedRitual : TriggerData
	{
		// Token: 0x06002F26 RID: 12070 RVA: 0x0011843D File Offset: 0x0011663D
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.ticksPassed, "ticksPassed", 0f, false);
		}

		// Token: 0x04001CD8 RID: 7384
		public float ticksPassed;
	}
}
