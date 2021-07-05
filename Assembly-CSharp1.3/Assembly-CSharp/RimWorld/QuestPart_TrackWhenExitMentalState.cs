using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB3 RID: 2995
	public class QuestPart_TrackWhenExitMentalState : QuestPart
	{
		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x060045E6 RID: 17894 RVA: 0x00172284 File Offset: 0x00170484
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

		// Token: 0x060045E7 RID: 17895 RVA: 0x001722D0 File Offset: 0x001704D0
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

		// Token: 0x060045E8 RID: 17896 RVA: 0x00172368 File Offset: 0x00170568
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

		// Token: 0x04002A97 RID: 10903
		public string tag;

		// Token: 0x04002A98 RID: 10904
		public List<string> inSignals;

		// Token: 0x04002A99 RID: 10905
		public string outSignal;

		// Token: 0x04002A9A RID: 10906
		public MapParent mapParent;

		// Token: 0x04002A9B RID: 10907
		public MentalStateDef mentalStateDef;

		// Token: 0x04002A9C RID: 10908
		private bool signalSent;

		// Token: 0x04002A9D RID: 10909
		[Unsaved(false)]
		private List<Pawn> cachedPawns;
	}
}
