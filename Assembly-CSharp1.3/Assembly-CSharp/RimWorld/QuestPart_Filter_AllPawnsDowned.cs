using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B28 RID: 2856
	public class QuestPart_Filter_AllPawnsDowned : QuestPart_Filter
	{
		// Token: 0x0600430C RID: 17164 RVA: 0x00166670 File Offset: 0x00164870
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

		// Token: 0x0600430D RID: 17165 RVA: 0x001666E4 File Offset: 0x001648E4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x0016673C File Offset: 0x0016493C
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

		// Token: 0x040028CD RID: 10445
		public List<Pawn> pawns;

		// Token: 0x040028CE RID: 10446
		public string inSignalRemovePawn;
	}
}
