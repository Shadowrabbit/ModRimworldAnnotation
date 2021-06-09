using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CF RID: 4303
	public class QuestPart_FeedPawns : QuestPart
	{
		// Token: 0x06005DD7 RID: 24023 RVA: 0x001DDABC File Offset: 0x001DBCBC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				if (this.pawns != null)
				{
					foreach (Pawn pawn in this.pawns)
					{
						pawn.needs.food.CurLevel = pawn.needs.food.MaxLevel;
					}
				}
				if (this.pawnsInTransporter != null)
				{
					using (IEnumerator<Thing> enumerator2 = ((IEnumerable<Thing>)this.pawnsInTransporter.TryGetComp<CompTransporter>().innerContainer).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Pawn pawn2;
							if ((pawn2 = (enumerator2.Current as Pawn)) != null)
							{
								pawn2.needs.food.CurLevel = pawn2.needs.food.MaxLevel;
							}
						}
					}
				}
			}
		}

		// Token: 0x06005DD8 RID: 24024 RVA: 0x001DDBB8 File Offset: 0x001DBDB8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.pawnsInTransporter, "pawnsInTransporter", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06005DD9 RID: 24025 RVA: 0x000410F2 File Offset: 0x0003F2F2
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawns != null)
			{
				this.pawns.Replace(replace, with);
			}
		}

		// Token: 0x04003EC1 RID: 16065
		public string inSignal;

		// Token: 0x04003EC2 RID: 16066
		public Thing pawnsInTransporter;

		// Token: 0x04003EC3 RID: 16067
		public List<Pawn> pawns = new List<Pawn>();
	}
}
