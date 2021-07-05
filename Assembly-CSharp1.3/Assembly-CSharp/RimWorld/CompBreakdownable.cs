using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001113 RID: 4371
	public class CompBreakdownable : ThingComp
	{
		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x060068F9 RID: 26873 RVA: 0x00236D9C File Offset: 0x00234F9C
		public bool BrokenDown
		{
			get
			{
				return this.brokenDownInt;
			}
		}

		// Token: 0x060068FA RID: 26874 RVA: 0x00236DA4 File Offset: 0x00234FA4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.brokenDownInt, "brokenDown", false, false);
		}

		// Token: 0x060068FB RID: 26875 RVA: 0x00236DC0 File Offset: 0x00234FC0
		private void UpdateOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayBrokenDown);
			if (this.brokenDownInt)
			{
				this.overlayBrokenDown = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.BrokenDown));
			}
		}

		// Token: 0x060068FC RID: 26876 RVA: 0x00236E2C File Offset: 0x0023502C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.parent.Map.GetComponent<BreakdownManager>().Register(this);
			this.UpdateOverlays();
		}

		// Token: 0x060068FD RID: 26877 RVA: 0x00236E62 File Offset: 0x00235062
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.GetComponent<BreakdownManager>().Deregister(this);
		}

		// Token: 0x060068FE RID: 26878 RVA: 0x00236E77 File Offset: 0x00235077
		public void CheckForBreakdown()
		{
			if (this.CanBreakdownNow() && Rand.MTBEventOccurs(13680000f, 1f, 1041f))
			{
				this.DoBreakdown();
			}
		}

		// Token: 0x060068FF RID: 26879 RVA: 0x00236E9D File Offset: 0x0023509D
		protected bool CanBreakdownNow()
		{
			return !this.BrokenDown && (this.powerComp == null || this.powerComp.PowerOn);
		}

		// Token: 0x06006900 RID: 26880 RVA: 0x00236EC0 File Offset: 0x002350C0
		public void Notify_Repaired()
		{
			this.brokenDownInt = false;
			this.parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(this.parent);
			if (this.parent is Building_PowerSwitch)
			{
				this.parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(this.parent.GetComp<CompPower>());
			}
			this.UpdateOverlays();
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x00236F22 File Offset: 0x00235122
		public void DoBreakdown()
		{
			this.brokenDownInt = true;
			this.parent.BroadcastCompSignal("Breakdown");
			this.parent.Map.GetComponent<BreakdownManager>().Notify_BrokenDown(this.parent);
			this.UpdateOverlays();
		}

		// Token: 0x06006902 RID: 26882 RVA: 0x00236F5C File Offset: 0x0023515C
		public override string CompInspectStringExtra()
		{
			if (this.BrokenDown)
			{
				return "BrokenDown".Translate();
			}
			return null;
		}

		// Token: 0x04003AC7 RID: 15047
		private bool brokenDownInt;

		// Token: 0x04003AC8 RID: 15048
		private CompPowerTrader powerComp;

		// Token: 0x04003AC9 RID: 15049
		private const int BreakdownMTBTicks = 13680000;

		// Token: 0x04003ACA RID: 15050
		public const string BreakdownSignal = "Breakdown";

		// Token: 0x04003ACB RID: 15051
		private OverlayHandle? overlayBrokenDown;
	}
}
