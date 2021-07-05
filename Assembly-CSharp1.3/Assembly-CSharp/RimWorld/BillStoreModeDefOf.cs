using System;

namespace RimWorld
{
	// Token: 0x02001449 RID: 5193
	[DefOf]
	public static class BillStoreModeDefOf
	{
		// Token: 0x06007D3C RID: 32060 RVA: 0x002C4A18 File Offset: 0x002C2C18
		static BillStoreModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillStoreModeDefOf));
		}

		// Token: 0x04004CCA RID: 19658
		public static BillStoreModeDef DropOnFloor;

		// Token: 0x04004CCB RID: 19659
		public static BillStoreModeDef BestStockpile;

		// Token: 0x04004CCC RID: 19660
		public static BillStoreModeDef SpecificStockpile;
	}
}
