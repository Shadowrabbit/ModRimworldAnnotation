using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A4 RID: 5028
	public class PawnTable_Wildlife : PawnTable
	{
		// Token: 0x06007A6C RID: 31340 RVA: 0x002B310C File Offset: 0x002B130C
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}

		// Token: 0x06007A6D RID: 31341 RVA: 0x002B30EA File Offset: 0x002B12EA
		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
