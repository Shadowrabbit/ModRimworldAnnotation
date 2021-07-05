using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B9B RID: 7067
	public class PawnTable_PlayerPawns : PawnTable
	{
		// Token: 0x06009BD2 RID: 39890 RVA: 0x00067A47 File Offset: 0x00065C47
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return PlayerPawnsDisplayOrderUtility.InOrder(input);
		}

		// Token: 0x06009BD3 RID: 39891 RVA: 0x000679DC File Offset: 0x00065BDC
		public PawnTable_PlayerPawns(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
