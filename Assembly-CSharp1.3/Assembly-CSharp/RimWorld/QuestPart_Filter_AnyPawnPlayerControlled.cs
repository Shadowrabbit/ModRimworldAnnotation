using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B32 RID: 2866
	public class QuestPart_Filter_AnyPawnPlayerControlled : QuestPart_Filter
	{
		// Token: 0x0600432C RID: 17196 RVA: 0x00166CE8 File Offset: 0x00164EE8
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (!pawn.Destroyed && pawn.IsColonistPlayerControlled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x00166D5C File Offset: 0x00164F5C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x00166DB4 File Offset: 0x00164FB4
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

		// Token: 0x040028DA RID: 10458
		public List<Pawn> pawns;

		// Token: 0x040028DB RID: 10459
		public string inSignalRemovePawn;
	}
}
