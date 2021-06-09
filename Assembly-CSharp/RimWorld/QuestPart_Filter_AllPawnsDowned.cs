using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001059 RID: 4185
	public class QuestPart_Filter_AllPawnsDowned : QuestPart_Filter
	{
		// Token: 0x06005B25 RID: 23333 RVA: 0x001D7774 File Offset: 0x001D5974
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (!pawn.Dead && !pawn.Downed)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005B26 RID: 23334 RVA: 0x001D77E8 File Offset: 0x001D59E8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06005B27 RID: 23335 RVA: 0x001D7840 File Offset: 0x001D5A40
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04003D34 RID: 15668
		public List<Pawn> pawns;

		// Token: 0x04003D35 RID: 15669
		public string inSignalRemovePawn;
	}
}
