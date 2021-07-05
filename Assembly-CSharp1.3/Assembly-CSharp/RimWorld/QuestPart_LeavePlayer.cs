using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B83 RID: 2947
	public class QuestPart_LeavePlayer : QuestPart
	{
		// Token: 0x060044E4 RID: 17636 RVA: 0x0016CFF4 File Offset: 0x0016B1F4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].Faction == Faction.OfPlayer)
					{
						this.pawns[i].SetFaction(this.replacementFaction, null);
					}
				}
			}
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x0016D0CF File Offset: 0x0016B2CF
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.replacementFaction == faction)
			{
				this.replacementFaction = null;
			}
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x0016D0E1 File Offset: 0x0016B2E1
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x0016D0EF File Offset: 0x0016B2EF
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x0016D100 File Offset: 0x0016B300
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.replacementFaction, "replacementFaction", false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040029CB RID: 10699
		public string inSignal;

		// Token: 0x040029CC RID: 10700
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040029CD RID: 10701
		public Faction replacementFaction;

		// Token: 0x040029CE RID: 10702
		public string inSignalRemovePawn;
	}
}
