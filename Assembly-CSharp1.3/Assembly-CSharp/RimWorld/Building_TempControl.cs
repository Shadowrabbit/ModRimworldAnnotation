using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001065 RID: 4197
	public class Building_TempControl : Building
	{
		// Token: 0x0600637C RID: 25468 RVA: 0x00219E38 File Offset: 0x00218038
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compTempControl = base.GetComp<CompTempControl>();
			this.compPowerTrader = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x04003859 RID: 14425
		public CompTempControl compTempControl;

		// Token: 0x0400385A RID: 14426
		public CompPowerTrader compPowerTrader;
	}
}
