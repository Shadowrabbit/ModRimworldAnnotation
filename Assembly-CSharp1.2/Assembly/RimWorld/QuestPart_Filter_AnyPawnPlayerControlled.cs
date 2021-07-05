using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001060 RID: 4192
	public class QuestPart_Filter_AnyPawnPlayerControlled : QuestPart_Filter
	{
		// Token: 0x06005B3D RID: 23357 RVA: 0x001D7AE4 File Offset: 0x001D5CE4
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

		// Token: 0x06005B3E RID: 23358 RVA: 0x001D7B58 File Offset: 0x001D5D58
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06005B3F RID: 23359 RVA: 0x001D7BB0 File Offset: 0x001D5DB0
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

		// Token: 0x04003D42 RID: 15682
		public List<Pawn> pawns;

		// Token: 0x04003D43 RID: 15683
		public string inSignalRemovePawn;
	}
}
