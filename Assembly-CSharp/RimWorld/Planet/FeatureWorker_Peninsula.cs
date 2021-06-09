using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002025 RID: 8229
	public class FeatureWorker_Peninsula : FeatureWorker_Protrusion
	{
		// Token: 0x0600AE48 RID: 44616 RVA: 0x0032A75C File Offset: 0x0032895C
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}
	}
}
