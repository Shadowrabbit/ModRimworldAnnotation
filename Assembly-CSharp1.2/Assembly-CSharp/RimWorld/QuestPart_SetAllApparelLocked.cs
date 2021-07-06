using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001111 RID: 4369
	public class QuestPart_SetAllApparelLocked : QuestPart
	{
		// Token: 0x06005F70 RID: 24432 RVA: 0x001E22BC File Offset: 0x001E04BC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].apparel != null)
					{
						this.pawns[i].apparel.LockAll();
					}
				}
			}
		}

		// Token: 0x06005F71 RID: 24433 RVA: 0x001E2324 File Offset: 0x001E0524
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06005F72 RID: 24434 RVA: 0x000420C9 File Offset: 0x000402C9
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04003FCF RID: 16335
		public string inSignal;

		// Token: 0x04003FD0 RID: 16336
		public List<Pawn> pawns = new List<Pawn>();
	}
}
