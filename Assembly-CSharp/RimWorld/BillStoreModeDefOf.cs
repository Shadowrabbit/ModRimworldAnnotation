using System;

namespace RimWorld
{
	// Token: 0x02001C88 RID: 7304
	[DefOf]
	public static class BillStoreModeDefOf
	{
		// Token: 0x06009F8B RID: 40843 RVA: 0x0006A5A8 File Offset: 0x000687A8
		static BillStoreModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillStoreModeDefOf));
		}

		// Token: 0x04006BD2 RID: 27602
		public static BillStoreModeDef DropOnFloor;

		// Token: 0x04006BD3 RID: 27603
		public static BillStoreModeDef BestStockpile;

		// Token: 0x04006BD4 RID: 27604
		public static BillStoreModeDef SpecificStockpile;
	}
}
