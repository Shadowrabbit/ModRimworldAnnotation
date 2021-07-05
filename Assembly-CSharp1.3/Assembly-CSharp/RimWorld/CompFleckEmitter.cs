using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001134 RID: 4404
	public class CompFleckEmitter : ThingComp
	{
		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x060069D3 RID: 27091 RVA: 0x0023A5EA File Offset: 0x002387EA
		private CompProperties_FleckEmitter Props
		{
			get
			{
				return (CompProperties_FleckEmitter)this.props;
			}
		}

		// Token: 0x060069D4 RID: 27092 RVA: 0x0023A5F8 File Offset: 0x002387F8
		public override void CompTick()
		{
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return;
			}
			CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
			if (comp2 != null && comp2.ticksLeft <= 0)
			{
				return;
			}
			CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
			if (comp3 != null && !comp3.Initiated)
			{
				return;
			}
			if (this.Props.emissionInterval != -1)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.Emit();
					this.ticksSinceLastEmitted = 0;
					return;
				}
				this.ticksSinceLastEmitted++;
			}
		}

		// Token: 0x060069D5 RID: 27093 RVA: 0x0023A68C File Offset: 0x0023888C
		protected void Emit()
		{
			FleckMaker.Static(this.parent.DrawPos + this.Props.offset, this.parent.MapHeld, this.Props.fleck, 1f);
			if (!this.Props.soundOnEmission.NullOrUndefined())
			{
				this.Props.soundOnEmission.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
		}

		// Token: 0x060069D6 RID: 27094 RVA: 0x0023A708 File Offset: 0x00238908
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, ((this.Props.saveKeysPrefix != null) ? (this.Props.saveKeysPrefix + "_") : "") + "ticksSinceLastEmitted", 0, false);
		}

		// Token: 0x04003B17 RID: 15127
		public int ticksSinceLastEmitted;
	}
}
