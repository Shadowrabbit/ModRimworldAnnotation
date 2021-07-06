using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1F RID: 2847
	public interface IBillGiver
	{
		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x060042B4 RID: 17076
		Map Map { get; }

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x060042B5 RID: 17077
		BillStack BillStack { get; }

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x060042B6 RID: 17078
		IEnumerable<IntVec3> IngredientStackCells { get; }

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x060042B7 RID: 17079
		string LabelShort { get; }

		// Token: 0x060042B8 RID: 17080
		bool CurrentlyUsableForBills();

		// Token: 0x060042B9 RID: 17081
		bool UsableForBillsAfterFueling();
	}
}
