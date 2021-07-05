using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA2 RID: 2978
	public class QuestPart_ReservePawns : QuestPart
	{
		// Token: 0x06004587 RID: 17799 RVA: 0x0017094E File Offset: 0x0016EB4E
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x0017095C File Offset: 0x0016EB5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04002A56 RID: 10838
		public List<Pawn> pawns = new List<Pawn>();
	}
}
