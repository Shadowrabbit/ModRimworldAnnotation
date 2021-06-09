using System;

namespace RimWorld
{
	// Token: 0x02001C4C RID: 7244
	[DefOf]
	public static class FactionDefOf
	{
		// Token: 0x06009F4F RID: 40783 RVA: 0x0006A1AC File Offset: 0x000683AC
		static FactionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
		}

		// Token: 0x04006769 RID: 26473
		public static FactionDef PlayerColony;

		// Token: 0x0400676A RID: 26474
		public static FactionDef PlayerTribe;

		// Token: 0x0400676B RID: 26475
		public static FactionDef Ancients;

		// Token: 0x0400676C RID: 26476
		public static FactionDef AncientsHostile;

		// Token: 0x0400676D RID: 26477
		public static FactionDef Mechanoid;

		// Token: 0x0400676E RID: 26478
		public static FactionDef Insect;

		// Token: 0x0400676F RID: 26479
		public static FactionDef OutlanderCivil;

		// Token: 0x04006770 RID: 26480
		[MayRequireRoyalty]
		public static FactionDef Empire;

		// Token: 0x04006771 RID: 26481
		[MayRequireRoyalty]
		public static FactionDef OutlanderRefugee;
	}
}
