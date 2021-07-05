using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B26 RID: 2854
	public class QuestPart_Filter_AllPawnsDespawned : QuestPart_Filter
	{
		// Token: 0x06004304 RID: 17156 RVA: 0x00166410 File Offset: 0x00164610
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

		// Token: 0x06004305 RID: 17157 RVA: 0x00166478 File Offset: 0x00164678
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x001664D0 File Offset: 0x001646D0
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

		// Token: 0x040028C9 RID: 10441
		public List<Pawn> pawns;

		// Token: 0x040028CA RID: 10442
		public string inSignalRemovePawn;
	}
}
