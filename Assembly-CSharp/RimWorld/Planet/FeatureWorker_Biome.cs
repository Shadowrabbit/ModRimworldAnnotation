using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200201E RID: 8222
	public class FeatureWorker_Biome : FeatureWorker_FloodFill
	{
		// Token: 0x0600AE25 RID: 44581 RVA: 0x0007157C File Offset: 0x0006F77C
		protected override bool IsRoot(int tile)
		{
			return this.def.rootBiomes.Contains(Find.WorldGrid[tile].biome);
		}

		// Token: 0x0600AE26 RID: 44582 RVA: 0x0007159E File Offset: 0x0006F79E
		protected override bool IsPossiblyAllowed(int tile)
		{
			return this.def.acceptableBiomes.Contains(Find.WorldGrid[tile].biome);
		}
	}
}
