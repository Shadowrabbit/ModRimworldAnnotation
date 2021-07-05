using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200118F RID: 4495
	public class CompSendSignalOnCountdown : ThingComp
	{
		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06006C1C RID: 27676 RVA: 0x00243F5A File Offset: 0x0024215A
		private CompProperties_SendSignalOnCountdown Props
		{
			get
			{
				return (CompProperties_SendSignalOnCountdown)this.props;
			}
		}

		// Token: 0x06006C1D RID: 27677 RVA: 0x00243F67 File Offset: 0x00242167
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksLeft = Mathf.CeilToInt(Rand.ByCurve(this.Props.countdownCurveTicks));
		}

		// Token: 0x06006C1E RID: 27678 RVA: 0x00243F9C File Offset: 0x0024219C
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Activate",
				action = delegate()
				{
					Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
					SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					this.ticksLeft = 0;
				}
			};
			yield break;
		}

		// Token: 0x06006C1F RID: 27679 RVA: 0x00243FAC File Offset: 0x002421AC
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x06006C20 RID: 27680 RVA: 0x00243FCC File Offset: 0x002421CC
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickRareWorker();
		}

		// Token: 0x06006C21 RID: 27681 RVA: 0x00243FDC File Offset: 0x002421DC
		public void TickRareWorker()
		{
			if (this.ticksLeft <= 0 || !this.parent.Spawned)
			{
				return;
			}
			this.ticksLeft -= 250;
			if (this.ticksLeft <= 0)
			{
				Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x06006C22 RID: 27682 RVA: 0x0024406C File Offset: 0x0024226C
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			if (this.ticksLeft <= 0)
			{
				return "expired".Translate().CapitalizeFirst();
			}
			return "SendSignalOnCountdownCompTime".Translate(this.ticksLeft.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x06006C23 RID: 27683 RVA: 0x002440CC File Offset: 0x002422CC
		public override void Notify_SignalReceived(Signal signal)
		{
			Thing thing;
			if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
			{
				this.ticksLeft = 0;
			}
		}

		// Token: 0x06006C24 RID: 27684 RVA: 0x00244143 File Offset: 0x00242343
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04003C19 RID: 15385
		public string signalTag;

		// Token: 0x04003C1A RID: 15386
		public int ticksLeft;

		// Token: 0x04003C1B RID: 15387
		private const float MaxDistActivationByOther = 40f;
	}
}
