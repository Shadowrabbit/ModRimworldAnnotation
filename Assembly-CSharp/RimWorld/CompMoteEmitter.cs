using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020017F9 RID: 6137
	public class CompMoteEmitter : ThingComp
	{
		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x060087D3 RID: 34771 RVA: 0x0005B1EB File Offset: 0x000593EB
		private CompProperties_MoteEmitter Props
		{
			get
			{
				return (CompProperties_MoteEmitter)this.props;
			}
		}

		// Token: 0x060087D4 RID: 34772 RVA: 0x0027C88C File Offset: 0x0027AA8C
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
			if (this.Props.emissionInterval != -1 && !this.Props.maintain)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.Emit();
					this.ticksSinceLastEmitted = 0;
				}
				else
				{
					this.ticksSinceLastEmitted++;
				}
			}
			else if (this.mote == null)
			{
				this.Emit();
			}
			if (this.Props.maintain && this.mote != null)
			{
				this.mote.Maintain();
			}
		}

		// Token: 0x060087D5 RID: 34773 RVA: 0x0027C960 File Offset: 0x0027AB60
		protected void Emit()
		{
			this.mote = MoteMaker.MakeStaticMote(this.parent.DrawPos + this.Props.offset, this.parent.Map, this.Props.mote, 1f);
			if (!this.Props.soundOnEmission.NullOrUndefined())
			{
				this.Props.soundOnEmission.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
		}

		// Token: 0x060087D6 RID: 34774 RVA: 0x0027C9E4 File Offset: 0x0027ABE4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, ((this.Props.saveKeysPrefix != null) ? (this.Props.saveKeysPrefix + "_") : "") + "ticksSinceLastEmitted", 0, false);
		}

		// Token: 0x0400571A RID: 22298
		public int ticksSinceLastEmitted;

		// Token: 0x0400571B RID: 22299
		protected Mote mote;
	}
}
