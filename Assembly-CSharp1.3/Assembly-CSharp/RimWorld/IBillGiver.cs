using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B8 RID: 1720
	public interface IBillGiver
	{
		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002FC6 RID: 12230
		Map Map { get; }

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002FC7 RID: 12231
		BillStack BillStack { get; }

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06002FC8 RID: 12232
		IEnumerable<IntVec3> IngredientStackCells { get; }

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06002FC9 RID: 12233
		string LabelShort { get; }

		// Token: 0x06002FCA RID: 12234
		bool CurrentlyUsableForBills();

		// Token: 0x06002FCB RID: 12235
		bool UsableForBillsAfterFueling();
	}
}
