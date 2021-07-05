using System;

namespace RimWorld
{
	// Token: 0x02001443 RID: 5187
	[DefOf]
	public static class RoadPathingDefOf
	{
		// Token: 0x06007D36 RID: 32054 RVA: 0x002C49B2 File Offset: 0x002C2BB2
		static RoadPathingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadPathingDefOf));
		}

		// Token: 0x04004CA5 RID: 19621
		public static RoadPathingDef Avoid;

		// Token: 0x04004CA6 RID: 19622
		public static RoadPathingDef Bulldoze;
	}
}
