using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B9D RID: 7069
	public class PawnTable_Wildlife : PawnTable
	{
		// Token: 0x06009BD5 RID: 39893 RVA: 0x002DABF4 File Offset: 0x002D8DF4
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}

		// Token: 0x06009BD6 RID: 39894 RVA: 0x000679DC File Offset: 0x00065BDC
		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
