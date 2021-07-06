using System;

namespace RimWorld
{
	// Token: 0x02001CA5 RID: 7333
	[DefOf]
	public static class SketchResolverDefOf
	{
		// Token: 0x06009FA8 RID: 40872 RVA: 0x0006A795 File Offset: 0x00068995
		static SketchResolverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SketchResolverDefOf));
		}

		// Token: 0x04006C67 RID: 27751
		public static SketchResolverDef Monument;

		// Token: 0x04006C68 RID: 27752
		public static SketchResolverDef MonumentRuin;

		// Token: 0x04006C69 RID: 27753
		public static SketchResolverDef Symmetry;

		// Token: 0x04006C6A RID: 27754
		public static SketchResolverDef AssignRandomStuff;

		// Token: 0x04006C6B RID: 27755
		public static SketchResolverDef FloorFill;

		// Token: 0x04006C6C RID: 27756
		public static SketchResolverDef AddColumns;

		// Token: 0x04006C6D RID: 27757
		public static SketchResolverDef AddCornerThings;

		// Token: 0x04006C6E RID: 27758
		public static SketchResolverDef AddThingsCentral;

		// Token: 0x04006C6F RID: 27759
		public static SketchResolverDef AddWallEdgeThings;

		// Token: 0x04006C70 RID: 27760
		public static SketchResolverDef AddInnerMonuments;

		// Token: 0x04006C71 RID: 27761
		public static SketchResolverDef DamageBuildings;

		// Token: 0x04006C72 RID: 27762
		[MayRequireRoyalty]
		public static SketchResolverDef MechCluster;

		// Token: 0x04006C73 RID: 27763
		[MayRequireRoyalty]
		public static SketchResolverDef MechClusterWalls;
	}
}
