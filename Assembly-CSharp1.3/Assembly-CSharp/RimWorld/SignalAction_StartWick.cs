using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A3 RID: 4259
	public class SignalAction_StartWick : SignalAction_Delay
	{
		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x0600657B RID: 25979 RVA: 0x0022435A File Offset: 0x0022255A
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null && this.thingWithWick.def == ThingDefOf.AncientFuelNode)
				{
					this.cachedAlert = new Alert_FuelNodeIgnition(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x0600657C RID: 25980 RVA: 0x00224388 File Offset: 0x00222588
		public override bool ShouldRemoveNow
		{
			get
			{
				if (this.thingWithWick == null || this.thingWithWick.Destroyed)
				{
					return true;
				}
				CompExplosive compExplosive = this.thingWithWick.TryGetComp<CompExplosive>();
				return compExplosive == null || compExplosive.wickStarted;
			}
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x002243C8 File Offset: 0x002225C8
		protected override void Complete()
		{
			base.Complete();
			if (this.ShouldRemoveNow)
			{
				return;
			}
			CompExplosive compExplosive = this.thingWithWick.TryGetComp<CompExplosive>();
			if (compExplosive != null)
			{
				compExplosive.StartWick(this.instigator);
			}
		}

		// Token: 0x0600657E RID: 25982 RVA: 0x002243FF File Offset: 0x002225FF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_References.Look<Thing>(ref this.thingWithWick, "thingWithWick", false);
		}

		// Token: 0x04003929 RID: 14633
		public Thing instigator;

		// Token: 0x0400392A RID: 14634
		public Thing thingWithWick;

		// Token: 0x0400392B RID: 14635
		private Alert_ActionDelay cachedAlert;
	}
}
