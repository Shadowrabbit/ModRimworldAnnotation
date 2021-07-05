using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001120 RID: 4384
	public class QuestPart_TrackWhenExitMentalState : QuestPart
	{
		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06005FDB RID: 24539 RVA: 0x001E3470 File Offset: 0x001E1670
		private List<Pawn> TrackedPawns
		{
			get
			{
				if (this.cachedPawns == null)
				{
					this.cachedPawns = (from p in this.mapParent.Map.mapPawns.AllPawnsSpawned
					where p.InMentalState && p.MentalStateDef == this.mentalStateDef && !p.questTags.NullOrEmpty<string>() && p.questTags.Contains(this.tag)
					select p).ToList<Pawn>();
				}
				return this.cachedPawns;
			}
		}

		// Token: 0x06005FDC RID: 24540 RVA: 0x001E34BC File Offset: 0x001E16BC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!this.signalSent && this.inSignals.Contains(signal.tag))
			{
				Pawn pawn = this.TrackedPawns.Find((Pawn p) => p == signal.args.GetArg<Pawn>("SUBJECT"));
				if (pawn != null)
				{
					this.cachedPawns.Remove(pawn);
				}
				if (!this.cachedPawns.Any<Pawn>())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignal));
					this.signalSent = true;
				}
			}
		}

		// Token: 0x06005FDD RID: 24541 RVA: 0x001E3554 File Offset: 0x001E1754
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Defs.Look<MentalStateDef>(ref this.mentalStateDef, "mentalStateDef");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<bool>(ref this.signalSent, "signalSent", false, false);
		}

		// Token: 0x04004012 RID: 16402
		public string tag;

		// Token: 0x04004013 RID: 16403
		public List<string> inSignals;

		// Token: 0x04004014 RID: 16404
		public string outSignal;

		// Token: 0x04004015 RID: 16405
		public MapParent mapParent;

		// Token: 0x04004016 RID: 16406
		public MentalStateDef mentalStateDef;

		// Token: 0x04004017 RID: 16407
		private bool signalSent;

		// Token: 0x04004018 RID: 16408
		[Unsaved(false)]
		private List<Pawn> cachedPawns;
	}
}
