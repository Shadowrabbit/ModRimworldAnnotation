using System;

namespace RimWorld
{
	// Token: 0x02001C82 RID: 7298
	[DefOf]
	public static class RoadDefOf
	{
		// Token: 0x06009F85 RID: 40837 RVA: 0x0006A542 File Offset: 0x00068742
		static RoadDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadDefOf));
		}

		// Token: 0x04006BB6 RID: 27574
		public static RoadDef DirtRoad;

		// Token: 0x04006BB7 RID: 27575
		public static RoadDef AncientAsphaltRoad;

		// Token: 0x04006BB8 RID: 27576
		public static RoadDef AncientAsphaltHighway;
	}
}
