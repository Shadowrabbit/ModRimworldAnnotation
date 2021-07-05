using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200173A RID: 5946
	public class FeatureWorker_Island : FeatureWorker_FloodFill
	{
		// Token: 0x0600892F RID: 35119 RVA: 0x00315014 File Offset: 0x00313214
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06008930 RID: 35120 RVA: 0x00315047 File Offset: 0x00313247
		protected override bool IsPossiblyAllowed(int tile)
		{
			return Find.WorldGrid[tile].biome == BiomeDefOf.Lake;
		}
	}
}
