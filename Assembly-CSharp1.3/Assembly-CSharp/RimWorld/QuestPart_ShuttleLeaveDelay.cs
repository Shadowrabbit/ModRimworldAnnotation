using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4D RID: 2893
	public class QuestPart_ShuttleLeaveDelay : QuestPart_Delay
	{
		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x060043B3 RID: 17331 RVA: 0x001688F2 File Offset: 0x00166AF2
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

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x060043B4 RID: 17332 RVA: 0x001686EF File Offset: 0x001668EF
		public override bool AlertCritical
		{
			get
			{
				return base.TicksLeft < 60000;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x060043B5 RID: 17333 RVA: 0x00168920 File Offset: 0x00166B20
		public override string AlertLabel
		{
			get
			{
				return "QuestPartShuttleLeaveDelay".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x060043B6 RID: 17334 RVA: 0x00168948 File Offset: 0x00166B48
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

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x060043B7 RID: 17335 RVA: 0x001689E2 File Offset: 0x00166BE2
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

		// Token: 0x060043B8 RID: 17336 RVA: 0x001689F2 File Offset: 0x00166BF2
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (base.State == QuestPartState.Enabled && this.inSignalsDisable.Contains(signal.tag))
			{
				this.Disable();
			}
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x00168A1D File Offset: 0x00166C1D
		public override string ExtraInspectString(ISelectable target)
		{
			if (target == this.shuttle)
			{
				return "ShuttleLeaveDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x00168A50 File Offset: 0x00166C50
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

		// Token: 0x060043BB RID: 17339 RVA: 0x00168AA5 File Offset: 0x00166CA5
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x0400291C RID: 10524
		public Thing shuttle;

		// Token: 0x0400291D RID: 10525
		public List<string> inSignalsDisable = new List<string>();
	}
}
