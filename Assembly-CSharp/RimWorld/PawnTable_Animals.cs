using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B99 RID: 7065
	public class PawnTable_Animals : PawnTable
	{
		// Token: 0x06009BC8 RID: 39880 RVA: 0x002DAB20 File Offset: 0x002D8D20
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return (from p in input
			orderby p.Name == null || p.Name.Numerical
			select p).ThenBy(delegate(Pawn p)
			{
				if (!(p.Name is NameSingle))
				{
					return 0;
				}
				return ((NameSingle)p.Name).Number;
			}).ThenBy((Pawn p) => p.def.label);
		}

		// Token: 0x06009BC9 RID: 39881 RVA: 0x002DAB9C File Offset: 0x002D8D9C
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.petness descending, p.RaceProps.baseBodySize
			select p;
		}

		// Token: 0x06009BCA RID: 39882 RVA: 0x000679DC File Offset: 0x00065BDC
		public PawnTable_Animals(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
