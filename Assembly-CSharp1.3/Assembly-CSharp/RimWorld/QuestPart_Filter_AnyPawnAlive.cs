using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B31 RID: 2865
	public class QuestPart_Filter_AnyPawnAlive : QuestPart_Filter
	{
		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06004327 RID: 17191 RVA: 0x00166B94 File Offset: 0x00164D94
		private int PawnsAliveCount
		{
			get
			{
				if (this.pawns.NullOrEmpty<Pawn>())
				{
					return 0;
				}
				int num = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Destroyed)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x00166BE0 File Offset: 0x00164DE0
		protected override bool Pass(SignalArgs args)
		{
			return this.PawnsAliveCount > 0;
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x00166BEC File Offset: 0x00164DEC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			if (signal.tag == this.inSignal)
			{
				signal.args.Add(this.PawnsAliveCount.Named("PAWNSALIVECOUNT"));
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x00166C78 File Offset: 0x00164E78
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

		// Token: 0x040028D8 RID: 10456
		public List<Pawn> pawns;

		// Token: 0x040028D9 RID: 10457
		public string inSignalRemovePawn;
	}
}
