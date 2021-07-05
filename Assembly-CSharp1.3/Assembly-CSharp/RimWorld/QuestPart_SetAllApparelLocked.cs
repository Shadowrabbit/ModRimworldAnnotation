using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA6 RID: 2982
	public class QuestPart_SetAllApparelLocked : QuestPart
	{
		// Token: 0x06004596 RID: 17814 RVA: 0x00170BCC File Offset: 0x0016EDCC
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

		// Token: 0x06004597 RID: 17815 RVA: 0x00170C34 File Offset: 0x0016EE34
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

		// Token: 0x06004598 RID: 17816 RVA: 0x00170CA2 File Offset: 0x0016EEA2
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04002A5C RID: 10844
		public string inSignal;

		// Token: 0x04002A5D RID: 10845
		public List<Pawn> pawns = new List<Pawn>();
	}
}
