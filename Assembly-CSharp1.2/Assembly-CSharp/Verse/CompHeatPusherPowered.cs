using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000528 RID: 1320
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x00107BB0 File Offset: 0x00105DB0
		protected override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x0001D4C3 File Offset: 0x0001B6C3
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}

		// Token: 0x040016FC RID: 5884
		protected CompPowerTrader powerComp;

		// Token: 0x040016FD RID: 5885
		protected CompRefuelable refuelableComp;

		// Token: 0x040016FE RID: 5886
		protected CompBreakdownable breakdownableComp;
	}
}
