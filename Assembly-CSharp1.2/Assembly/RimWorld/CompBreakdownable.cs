using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A5 RID: 6053
	public class CompBreakdownable : ThingComp
	{
		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x060085C2 RID: 34242 RVA: 0x00059A2F File Offset: 0x00057C2F
		public bool BrokenDown
		{
			get
			{
				return this.brokenDownInt;
			}
		}

		// Token: 0x060085C3 RID: 34243 RVA: 0x00059A37 File Offset: 0x00057C37
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.brokenDownInt, "brokenDown", false, false);
		}

		// Token: 0x060085C4 RID: 34244 RVA: 0x00059A51 File Offset: 0x00057C51
		public override void PostDraw()
		{
			if (this.brokenDownInt)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.BrokenDown);
			}
		}

		// Token: 0x060085C5 RID: 34245 RVA: 0x00059A78 File Offset: 0x00057C78
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.parent.Map.GetComponent<BreakdownManager>().Register(this);
		}

		// Token: 0x060085C6 RID: 34246 RVA: 0x00059AA8 File Offset: 0x00057CA8
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.GetComponent<BreakdownManager>().Deregister(this);
		}

		// Token: 0x060085C7 RID: 34247 RVA: 0x00059ABD File Offset: 0x00057CBD
		public void CheckForBreakdown()
		{
			if (this.CanBreakdownNow() && Rand.MTBEventOccurs(13680000f, 1f, 1041f))
			{
				this.DoBreakdown();
			}
		}

		// Token: 0x060085C8 RID: 34248 RVA: 0x00059AE3 File Offset: 0x00057CE3
		protected bool CanBreakdownNow()
		{
			return !this.BrokenDown && (this.powerComp == null || this.powerComp.PowerOn);
		}

		// Token: 0x060085C9 RID: 34249 RVA: 0x00276EB4 File Offset: 0x002750B4
		public void Notify_Repaired()
		{
			this.brokenDownInt = false;
			this.parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(this.parent);
			if (this.parent is Building_PowerSwitch)
			{
				this.parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(this.parent.GetComp<CompPower>());
			}
		}

		// Token: 0x060085CA RID: 34250 RVA: 0x00059B04 File Offset: 0x00057D04
		public void DoBreakdown()
		{
			this.brokenDownInt = true;
			this.parent.BroadcastCompSignal("Breakdown");
			this.parent.Map.GetComponent<BreakdownManager>().Notify_BrokenDown(this.parent);
		}

		// Token: 0x060085CB RID: 34251 RVA: 0x00059B38 File Offset: 0x00057D38
		public override string CompInspectStringExtra()
		{
			if (this.BrokenDown)
			{
				return "BrokenDown".Translate();
			}
			return null;
		}

		// Token: 0x04005646 RID: 22086
		private bool brokenDownInt;

		// Token: 0x04005647 RID: 22087
		private CompPowerTrader powerComp;

		// Token: 0x04005648 RID: 22088
		private const int BreakdownMTBTicks = 13680000;

		// Token: 0x04005649 RID: 22089
		public const string BreakdownSignal = "Breakdown";
	}
}
