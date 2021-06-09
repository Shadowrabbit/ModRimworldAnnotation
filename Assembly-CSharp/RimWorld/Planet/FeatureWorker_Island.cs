using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002023 RID: 8227
	public class FeatureWorker_Island : FeatureWorker_FloodFill
	{
		// Token: 0x0600AE43 RID: 44611 RVA: 0x0032A75C File Offset: 0x0032895C
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x0600AE44 RID: 44612 RVA: 0x000716E0 File Offset: 0x0006F8E0
		protected override bool IsPossiblyAllowed(int tile)
		{
			return Find.WorldGrid[tile].biome == BiomeDefOf.Lake;
		}
	}
}
