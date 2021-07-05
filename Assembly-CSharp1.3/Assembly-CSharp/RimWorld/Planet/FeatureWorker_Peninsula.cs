using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200173C RID: 5948
	public class FeatureWorker_Peninsula : FeatureWorker_Protrusion
	{
		// Token: 0x06008934 RID: 35124 RVA: 0x00315098 File Offset: 0x00313298
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}
	}
}
