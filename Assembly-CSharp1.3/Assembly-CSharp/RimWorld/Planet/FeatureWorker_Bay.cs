using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200173B RID: 5947
	public class FeatureWorker_Bay : FeatureWorker_Protrusion
	{
		// Token: 0x06008932 RID: 35122 RVA: 0x00315060 File Offset: 0x00313260
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}
	}
}
