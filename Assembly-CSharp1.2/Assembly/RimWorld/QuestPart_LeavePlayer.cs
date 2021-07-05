using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E2 RID: 4322
	public class QuestPart_LeavePlayer : QuestPart
	{
		// Token: 0x06005E53 RID: 24147 RVA: 0x001DEFE8 File Offset: 0x001DD1E8
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

		// Token: 0x06005E54 RID: 24148 RVA: 0x000415A2 File Offset: 0x0003F7A2
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.replacementFaction == faction)
			{
				this.replacementFaction = null;
			}
		}

		// Token: 0x06005E55 RID: 24149 RVA: 0x000415B4 File Offset: 0x0003F7B4
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06005E56 RID: 24150 RVA: 0x000415C2 File Offset: 0x0003F7C2
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x06005E57 RID: 24151 RVA: 0x001DF0C4 File Offset: 0x001DD2C4
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

		// Token: 0x04003F0B RID: 16139
		public string inSignal;

		// Token: 0x04003F0C RID: 16140
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003F0D RID: 16141
		public Faction replacementFaction;

		// Token: 0x04003F0E RID: 16142
		public string inSignalRemovePawn;
	}
}
