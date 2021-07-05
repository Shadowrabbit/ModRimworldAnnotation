using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002024 RID: 8228
	public class FeatureWorker_Bay : FeatureWorker_Protrusion
	{
		// Token: 0x0600AE46 RID: 44614 RVA: 0x0032AED0 File Offset: 0x003290D0
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}
	}
}
