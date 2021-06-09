using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001062 RID: 4194
	public class QuestPart_Filter_AnyPawnUnhealthy : QuestPart_Filter
	{
		// Token: 0x06005B44 RID: 23364 RVA: 0x001D7C20 File Offset: 0x001D5E20
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

		// Token: 0x06005B45 RID: 23365 RVA: 0x001D7CAC File Offset: 0x001D5EAC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x001D7D04 File Offset: 0x001D5F04
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

		// Token: 0x04003D46 RID: 15686
		public List<Pawn> pawns;

		// Token: 0x04003D47 RID: 15687
		public string inSignalRemovePawn;
	}
}
