using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A2 RID: 5026
	public class PawnTable_PlayerPawns : PawnTable
	{
		// Token: 0x06007A69 RID: 31337 RVA: 0x002B30F7 File Offset: 0x002B12F7
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return PlayerPawnsDisplayOrderUtility.InOrder(input);
		}

		// Token: 0x06007A6A RID: 31338 RVA: 0x002B30EA File Offset: 0x002B12EA
		public PawnTable_PlayerPawns(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
