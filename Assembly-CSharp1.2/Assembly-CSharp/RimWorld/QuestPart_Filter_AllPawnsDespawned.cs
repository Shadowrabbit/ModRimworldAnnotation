using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001055 RID: 4181
	public class QuestPart_Filter_AllPawnsDespawned : QuestPart_Filter
	{
		// Token: 0x06005B17 RID: 23319 RVA: 0x001D7514 File Offset: 0x001D5714
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
					if (enumerator.Current.Spawned)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x001D757C File Offset: 0x001D577C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06005B19 RID: 23321 RVA: 0x001D75D4 File Offset: 0x001D57D4
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

		// Token: 0x04003D2C RID: 15660
		public List<Pawn> pawns;

		// Token: 0x04003D2D RID: 15661
		public string inSignalRemovePawn;
	}
}
