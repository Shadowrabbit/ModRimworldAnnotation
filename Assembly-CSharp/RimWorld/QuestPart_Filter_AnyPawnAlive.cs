using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105D RID: 4189
	public class QuestPart_Filter_AnyPawnAlive : QuestPart_Filter
	{
		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06005B32 RID: 23346 RVA: 0x001D799C File Offset: 0x001D5B9C
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

		// Token: 0x06005B33 RID: 23347 RVA: 0x0003F400 File Offset: 0x0003D600
		protected override bool Pass(SignalArgs args)
		{
			return this.PawnsAliveCount > 0;
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x001D79E8 File Offset: 0x001D5BE8
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

		// Token: 0x06005B35 RID: 23349 RVA: 0x001D7A74 File Offset: 0x001D5C74
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

		// Token: 0x04003D3C RID: 15676
		public List<Pawn> pawns;

		// Token: 0x04003D3D RID: 15677
		public string inSignalRemovePawn;
	}
}
