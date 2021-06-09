using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C97 RID: 7319
	[DefOf]
	public static class TerrainAffordanceDefOf
	{
		// Token: 0x06009F9A RID: 40858 RVA: 0x0006A6A7 File Offset: 0x000688A7
		static TerrainAffordanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
		}

		// Token: 0x04006C29 RID: 27689
		public static TerrainAffordanceDef Light;

		// Token: 0x04006C2A RID: 27690
		public static TerrainAffordanceDef Medium;

		// Token: 0x04006C2B RID: 27691
		public static TerrainAffordanceDef Heavy;

		// Token: 0x04006C2C RID: 27692
		public static TerrainAffordanceDef GrowSoil;

		// Token: 0x04006C2D RID: 27693
		public static TerrainAffordanceDef Diggable;

		// Token: 0x04006C2E RID: 27694
		public static TerrainAffordanceDef SmoothableStone;

		// Token: 0x04006C2F RID: 27695
		public static TerrainAffordanceDef MovingFluid;

		// Token: 0x04006C30 RID: 27696
		public static TerrainAffordanceDef Bridgeable;
	}
}
