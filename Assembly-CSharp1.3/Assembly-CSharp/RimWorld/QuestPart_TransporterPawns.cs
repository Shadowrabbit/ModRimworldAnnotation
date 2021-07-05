using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB4 RID: 2996
	public abstract class QuestPart_TransporterPawns : QuestPart
	{
		// Token: 0x060045EB RID: 17899 RVA: 0x00172420 File Offset: 0x00170620
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				if (this.pawns != null)
				{
					foreach (Pawn pawn in this.pawns)
					{
						this.Process(pawn);
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
								this.Process(pawn2);
							}
						}
					}
				}
			}
		}

		// Token: 0x060045EC RID: 17900
		public abstract void Process(Pawn pawn);

		// Token: 0x060045ED RID: 17901 RVA: 0x001724E8 File Offset: 0x001706E8
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

		// Token: 0x060045EE RID: 17902 RVA: 0x00172567 File Offset: 0x00170767
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawns != null)
			{
				this.pawns.Replace(replace, with);
			}
		}

		// Token: 0x04002A9E RID: 10910
		public string inSignal;

		// Token: 0x04002A9F RID: 10911
		public Thing pawnsInTransporter;

		// Token: 0x04002AA0 RID: 10912
		public List<Pawn> pawns = new List<Pawn>();
	}
}
