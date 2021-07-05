using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001664 RID: 5732
	public class QuestPart_PawnsNoLongerRequiredForShuttleOnRescue : QuestPart
	{
		// Token: 0x06008598 RID: 34200 RVA: 0x002FEEA4 File Offset: 0x002FD0A4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignal && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
				if (compShuttle != null)
				{
					compShuttle.requiredPawns.Remove(item);
				}
				this.pawns.Remove(item);
			}
		}

		// Token: 0x06008599 RID: 34201 RVA: 0x002FEF10 File Offset: 0x002FD110
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04005371 RID: 21361
		public string inSignal;

		// Token: 0x04005372 RID: 21362
		public Thing shuttle;

		// Token: 0x04005373 RID: 21363
		public List<Pawn> pawns = new List<Pawn>();
	}
}
