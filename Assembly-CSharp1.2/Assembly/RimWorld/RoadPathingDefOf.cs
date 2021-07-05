using System;

namespace RimWorld
{
	// Token: 0x02001C83 RID: 7299
	[DefOf]
	public static class RoadPathingDefOf
	{
		// Token: 0x06009F86 RID: 40838 RVA: 0x0006A553 File Offset: 0x00068753
		static RoadPathingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadPathingDefOf));
		}

		// Token: 0x04006BB9 RID: 27577
		public static RoadPathingDef Avoid;

		// Token: 0x04006BBA RID: 27578
		public static RoadPathingDef Bulldoze;
	}
}
