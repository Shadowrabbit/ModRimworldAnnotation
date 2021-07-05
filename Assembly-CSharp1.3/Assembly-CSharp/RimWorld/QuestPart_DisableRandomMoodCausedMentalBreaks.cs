using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B71 RID: 2929
	public class QuestPart_DisableRandomMoodCausedMentalBreaks : QuestPartActivable
	{
		// Token: 0x06004486 RID: 17542 RVA: 0x0016B9D0 File Offset: 0x00169BD0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0016BA2C File Offset: 0x00169C2C
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04002996 RID: 10646
		public List<Pawn> pawns = new List<Pawn>();
	}
}
