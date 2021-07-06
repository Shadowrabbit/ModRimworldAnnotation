using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107E RID: 4222
	public class QuestPart_ShuttleLeaveDelay : QuestPart_Delay
	{
		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06005BED RID: 23533 RVA: 0x0003FBF0 File Offset: 0x0003DDF0
		public override AlertReport AlertReport
		{
			get
			{
				if (this.shuttle == null || !this.shuttle.Spawned)
				{
					return false;
				}
				return AlertReport.CulpritIs(this.shuttle);
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06005BEE RID: 23534 RVA: 0x0003FB09 File Offset: 0x0003DD09
		public override bool AlertCritical
		{
			get
			{
				return base.TicksLeft < 60000;
			}
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06005BEF RID: 23535 RVA: 0x0003FC1E File Offset: 0x0003DE1E
		public override string AlertLabel
		{
			get
			{
				return "QuestPartShuttleLeaveDelay".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06005BF0 RID: 23536 RVA: 0x001D95C4 File Offset: 0x001D77C4
		public override string AlertExplanation
		{
			get
			{
				if (this.quest.hidden)
				{
					return "QuestPartShuttleLeaveDelayDescHidden".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor));
				}
				return "QuestPartShuttleLeaveDelayDesc".Translate(this.quest.name, base.TicksLeft.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor), this.shuttle.TryGetComp<CompShuttle>().RequiredThingsLabel);
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06005BF1 RID: 23537 RVA: 0x0003FC43 File Offset: 0x0003DE43
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.shuttle != null)
				{
					yield return this.shuttle;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06005BF2 RID: 23538 RVA: 0x0003FC53 File Offset: 0x0003DE53
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (base.State == QuestPartState.Enabled && this.inSignalsDisable.Contains(signal.tag))
			{
				this.Disable();
			}
		}

		// Token: 0x06005BF3 RID: 23539 RVA: 0x0003FC7E File Offset: 0x0003DE7E
		public override string ExtraInspectString(ISelectable target)
		{
			if (target == this.shuttle)
			{
				return "ShuttleLeaveDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x06005BF4 RID: 23540 RVA: 0x001D9660 File Offset: 0x001D7860
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Collections.Look<string>(ref this.inSignalsDisable, "inSignalsDisable", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.inSignalsDisable == null)
			{
				this.inSignalsDisable = new List<string>();
			}
		}

		// Token: 0x06005BF5 RID: 23541 RVA: 0x0003FCAE File Offset: 0x0003DEAE
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x04003DA4 RID: 15780
		public Thing shuttle;

		// Token: 0x04003DA5 RID: 15781
		public List<string> inSignalsDisable = new List<string>();
	}
}
