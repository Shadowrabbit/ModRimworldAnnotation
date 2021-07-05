using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B33 RID: 2867
	public class QuestPart_Filter_AnyPawnUnhealthy : QuestPart_Filter
	{
		// Token: 0x06004330 RID: 17200 RVA: 0x00166E24 File Offset: 0x00165024
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (pawn.Destroyed || pawn.InMentalState || pawn.health.hediffSet.BleedRateTotal > 0.001f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x00166EB0 File Offset: 0x001650B0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x00166F08 File Offset: 0x00165108
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

		// Token: 0x040028DC RID: 10460
		public List<Pawn> pawns;

		// Token: 0x040028DD RID: 10461
		public string inSignalRemovePawn;
	}
}
