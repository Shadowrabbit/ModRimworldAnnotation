using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001409 RID: 5129
	[DefOf]
	public static class TerrainDefOf
	{
		// Token: 0x06007CFC RID: 31996 RVA: 0x002C45D8 File Offset: 0x002C27D8
		static TerrainDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainDefOf));
		}

		// Token: 0x0400475C RID: 18268
		public static TerrainDef Sand;

		// Token: 0x0400475D RID: 18269
		public static TerrainDef Soil;

		// Token: 0x0400475E RID: 18270
		public static TerrainDef SoilRich;

		// Token: 0x0400475F RID: 18271
		public static TerrainDef Underwall;

		// Token: 0x04004760 RID: 18272
		public static TerrainDef Concrete;

		// Token: 0x04004761 RID: 18273
		public static TerrainDef MetalTile;

		// Token: 0x04004762 RID: 18274
		public static TerrainDef Gravel;

		// Token: 0x04004763 RID: 18275
		public static TerrainDef WaterDeep;

		// Token: 0x04004764 RID: 18276
		public static TerrainDef WaterShallow;

		// Token: 0x04004765 RID: 18277
		public static TerrainDef WaterMovingChestDeep;

		// Token: 0x04004766 RID: 18278
		public static TerrainDef WaterMovingShallow;

		// Token: 0x04004767 RID: 18279
		public static TerrainDef WaterOceanDeep;

		// Token: 0x04004768 RID: 18280
		public static TerrainDef WaterOceanShallow;

		// Token: 0x04004769 RID: 18281
		public static TerrainDef PavedTile;

		// Token: 0x0400476A RID: 18282
		public static TerrainDef WoodPlankFloor;

		// Token: 0x0400476B RID: 18283
		public static TerrainDef TileSandstone;

		// Token: 0x0400476C RID: 18284
		public static TerrainDef Ice;

		// Token: 0x0400476D RID: 18285
		public static TerrainDef FlagstoneSandstone;

		// Token: 0x0400476E RID: 18286
		public static TerrainDef Bridge;

		// Token: 0x0400476F RID: 18287
		public static TerrainDef Sandstone_Smooth;

		// Token: 0x04004770 RID: 18288
		public static TerrainDef PackedDirt;

		// Token: 0x04004771 RID: 18289
		[MayRequireIdeology]
		public static TerrainDef FungalGravel;
	}
}
