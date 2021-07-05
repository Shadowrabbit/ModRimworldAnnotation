using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A1 RID: 5025
	public class PawnTable_Animals : PawnTable
	{
		// Token: 0x06007A67 RID: 31335 RVA: 0x002B3028 File Offset: 0x002B1228
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return (from p in input
			orderby p.Name == null || p.Name.Numerical, p.RaceProps.petness, p.RaceProps.baseBodySize
			select p).ThenBy(delegate(Pawn p)
			{
				if (!(p.Name is NameSingle))
				{
					return 0;
				}
				return ((NameSingle)p.Name).Number;
			}).ThenBy((Pawn p) => p.def.label);
		}

		// Token: 0x06007A68 RID: 31336 RVA: 0x002B30EA File Offset: 0x002B12EA
		public PawnTable_Animals(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
