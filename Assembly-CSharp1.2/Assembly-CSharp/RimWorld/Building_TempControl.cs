using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200169D RID: 5789
	public class Building_TempControl : Building
	{
		// Token: 0x06007E9E RID: 32414 RVA: 0x00055169 File Offset: 0x00053369
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compTempControl = base.GetComp<CompTempControl>();
			this.compPowerTrader = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x04005262 RID: 21090
		public CompTempControl compTempControl;

		// Token: 0x04005263 RID: 21091
		public CompPowerTrader compPowerTrader;
	}
}
