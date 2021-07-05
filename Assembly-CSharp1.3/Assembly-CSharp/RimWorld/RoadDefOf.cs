using System;

namespace RimWorld
{
	// Token: 0x02001442 RID: 5186
	[DefOf]
	public static class RoadDefOf
	{
		// Token: 0x06007D35 RID: 32053 RVA: 0x002C49A1 File Offset: 0x002C2BA1
		static RoadDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadDefOf));
		}

		// Token: 0x04004CA2 RID: 19618
		public static RoadDef DirtRoad;

		// Token: 0x04004CA3 RID: 19619
		public static RoadDef AncientAsphaltRoad;

		// Token: 0x04004CA4 RID: 19620
		public static RoadDef AncientAsphaltHighway;
	}
}
