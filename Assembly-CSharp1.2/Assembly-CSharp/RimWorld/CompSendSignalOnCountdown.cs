using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001834 RID: 6196
	public class CompSendSignalOnCountdown : ThingComp
	{
		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x06008958 RID: 35160 RVA: 0x0005C39A File Offset: 0x0005A59A
		private CompProperties_SendSignalOnCountdown Props
		{
			get
			{
				return (CompProperties_SendSignalOnCountdown)this.props;
			}
		}

		// Token: 0x06008959 RID: 35161 RVA: 0x0005C3A7 File Offset: 0x0005A5A7
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksLeft = Mathf.CeilToInt(Rand.ByCurve(this.Props.countdownCurveTicks));
		}

		// Token: 0x0600895A RID: 35162 RVA: 0x0005C3DC File Offset: 0x0005A5DC
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

		// Token: 0x0600895B RID: 35163 RVA: 0x0005C3EC File Offset: 0x0005A5EC
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x0600895C RID: 35164 RVA: 0x0005C40C File Offset: 0x0005A60C
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickRareWorker();
		}

		// Token: 0x0600895D RID: 35165 RVA: 0x0028202C File Offset: 0x0028022C
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

		// Token: 0x0600895E RID: 35166 RVA: 0x002820BC File Offset: 0x002802BC
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

		// Token: 0x0600895F RID: 35167 RVA: 0x0028211C File Offset: 0x0028031C
		public override void Notify_SignalReceived(Signal signal)
		{
			Thing thing;
			if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
			{
				this.ticksLeft = 0;
			}
		}

		// Token: 0x06008960 RID: 35168 RVA: 0x0005C41A File Offset: 0x0005A61A
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04005813 RID: 22547
		public string signalTag;

		// Token: 0x04005814 RID: 22548
		public int ticksLeft;

		// Token: 0x04005815 RID: 22549
		private const float MaxDistActivationByOther = 40f;
	}
}
