using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001057 RID: 4183
	public class QuestPart_Filter_AllPawnsDestroyed : QuestPart_Filter
	{
		// Token: 0x06005B1E RID: 23326 RVA: 0x001D7644 File Offset: 0x001D5844
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Destroyed)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005B1F RID: 23327 RVA: 0x001D76AC File Offset: 0x001D58AC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06005B20 RID: 23328 RVA: 0x001D7704 File Offset: 0x001D5904
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

		// Token: 0x04003D30 RID: 15664
		public List<Pawn> pawns;

		// Token: 0x04003D31 RID: 15665
		public string inSignalRemovePawn;
	}
}
