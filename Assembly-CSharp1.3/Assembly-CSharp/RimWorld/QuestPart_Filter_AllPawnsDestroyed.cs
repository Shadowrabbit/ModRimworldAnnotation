using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B27 RID: 2855
	public class QuestPart_Filter_AllPawnsDestroyed : QuestPart_Filter
	{
		// Token: 0x06004308 RID: 17160 RVA: 0x00166540 File Offset: 0x00164740
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

		// Token: 0x06004309 RID: 17161 RVA: 0x001665A8 File Offset: 0x001647A8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x00166600 File Offset: 0x00164800
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

		// Token: 0x040028CB RID: 10443
		public List<Pawn> pawns;

		// Token: 0x040028CC RID: 10444
		public string inSignalRemovePawn;
	}
}
