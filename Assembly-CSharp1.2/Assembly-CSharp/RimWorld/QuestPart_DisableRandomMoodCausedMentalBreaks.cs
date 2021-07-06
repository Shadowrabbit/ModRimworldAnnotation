using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C0 RID: 4288
	public class QuestPart_DisableRandomMoodCausedMentalBreaks : QuestPartActivable
	{
		// Token: 0x06005D85 RID: 23941 RVA: 0x001DCFB4 File Offset: 0x001DB1B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06005D86 RID: 23942 RVA: 0x00040D89 File Offset: 0x0003EF89
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04003E91 RID: 16017
		public List<Pawn> pawns = new List<Pawn>();
	}
}
