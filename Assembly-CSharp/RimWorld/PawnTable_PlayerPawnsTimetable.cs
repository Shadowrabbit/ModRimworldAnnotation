using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B9C RID: 7068
	public class PawnTable_PlayerPawnsTimetable : PawnTable_PlayerPawns
	{
		// Token: 0x06009BD4 RID: 39892 RVA: 0x00067A4F File Offset: 0x00065C4F
		public PawnTable_PlayerPawnsTimetable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
