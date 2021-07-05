using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001457 RID: 5207
	[DefOf]
	public static class TerrainAffordanceDefOf
	{
		// Token: 0x06007D4A RID: 32074 RVA: 0x002C4B06 File Offset: 0x002C2D06
		static TerrainAffordanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
		}

		// Token: 0x04004D1D RID: 19741
		public static TerrainAffordanceDef Light;

		// Token: 0x04004D1E RID: 19742
		public static TerrainAffordanceDef Medium;

		// Token: 0x04004D1F RID: 19743
		public static TerrainAffordanceDef Heavy;

		// Token: 0x04004D20 RID: 19744
		public static TerrainAffordanceDef GrowSoil;

		// Token: 0x04004D21 RID: 19745
		public static TerrainAffordanceDef Diggable;

		// Token: 0x04004D22 RID: 19746
		public static TerrainAffordanceDef SmoothableStone;

		// Token: 0x04004D23 RID: 19747
		public static TerrainAffordanceDef MovingFluid;

		// Token: 0x04004D24 RID: 19748
		public static TerrainAffordanceDef Bridgeable;
	}
}
