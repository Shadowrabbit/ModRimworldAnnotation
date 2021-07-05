using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000387 RID: 903
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001A76 RID: 6774 RVA: 0x00099A4C File Offset: 0x00097C4C
		protected override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x00099AB0 File Offset: 0x00097CB0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}

		// Token: 0x04001134 RID: 4404
		protected CompPowerTrader powerComp;

		// Token: 0x04001135 RID: 4405
		protected CompRefuelable refuelableComp;

		// Token: 0x04001136 RID: 4406
		protected CompBreakdownable breakdownableComp;
	}
}
