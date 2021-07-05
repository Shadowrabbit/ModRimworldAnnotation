using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001477 RID: 5239
	[DefOf]
	public static class InventoryStockGroupDefOf
	{
		// Token: 0x06007D69 RID: 32105 RVA: 0x002C4D15 File Offset: 0x002C2F15
		static InventoryStockGroupDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InventoryStockGroupDefOf));
		}

		// Token: 0x04004E2F RID: 20015
		public static InventoryStockGroupDef Medicine;
	}
}
