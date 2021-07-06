using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110A RID: 4362
	public class QuestPart_ReservePawns : QuestPart
	{
		// Token: 0x06005F4E RID: 24398 RVA: 0x00041F0C File Offset: 0x0004010C
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06005F4F RID: 24399 RVA: 0x001E2044 File Offset: 0x001E0244
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04003FBF RID: 16319
		public List<Pawn> pawns = new List<Pawn>();
	}
}
