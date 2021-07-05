using System;

namespace RimWorld
{
	// Token: 0x02001465 RID: 5221
	[DefOf]
	public static class SketchResolverDefOf
	{
		// Token: 0x06007D58 RID: 32088 RVA: 0x002C4BF4 File Offset: 0x002C2DF4
		static SketchResolverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SketchResolverDefOf));
		}

		// Token: 0x04004D59 RID: 19801
		public static SketchResolverDef Monument;

		// Token: 0x04004D5A RID: 19802
		public static SketchResolverDef MonumentRuin;

		// Token: 0x04004D5B RID: 19803
		public static SketchResolverDef Symmetry;

		// Token: 0x04004D5C RID: 19804
		public static SketchResolverDef AssignRandomStuff;

		// Token: 0x04004D5D RID: 19805
		public static SketchResolverDef FloorFill;

		// Token: 0x04004D5E RID: 19806
		public static SketchResolverDef AddColumns;

		// Token: 0x04004D5F RID: 19807
		public static SketchResolverDef AddCornerThings;

		// Token: 0x04004D60 RID: 19808
		public static SketchResolverDef AddThingsCentral;

		// Token: 0x04004D61 RID: 19809
		public static SketchResolverDef AddWallEdgeThings;

		// Token: 0x04004D62 RID: 19810
		public static SketchResolverDef AddInnerMonuments;

		// Token: 0x04004D63 RID: 19811
		public static SketchResolverDef DamageBuildings;

		// Token: 0x04004D64 RID: 19812
		[MayRequireRoyalty]
		public static SketchResolverDef MechCluster;

		// Token: 0x04004D65 RID: 19813
		[MayRequireRoyalty]
		public static SketchResolverDef MechClusterWalls;

		// Token: 0x04004D66 RID: 19814
		[MayRequireIdeology]
		public static SketchResolverDef AncientUtilityBuilding;

		// Token: 0x04004D67 RID: 19815
		[MayRequireIdeology]
		public static SketchResolverDef AncientLandingPad;
	}
}
