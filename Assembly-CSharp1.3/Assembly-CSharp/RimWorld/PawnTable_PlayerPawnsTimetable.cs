using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A3 RID: 5027
	public class PawnTable_PlayerPawnsTimetable : PawnTable_PlayerPawns
	{
		// Token: 0x06007A6B RID: 31339 RVA: 0x002B30FF File Offset: 0x002B12FF
		public PawnTable_PlayerPawnsTimetable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
