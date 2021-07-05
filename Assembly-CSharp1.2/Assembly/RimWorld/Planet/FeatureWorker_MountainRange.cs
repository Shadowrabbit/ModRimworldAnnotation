using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200201B RID: 8219
	public class FeatureWorker_MountainRange : FeatureWorker_Cluster
	{
		// Token: 0x0600AE1A RID: 44570 RVA: 0x000714EF File Offset: 0x0006F6EF
		protected override bool IsRoot(int tile)
		{
			return Find.WorldGrid[tile].hilliness != Hilliness.Flat;
		}

		// Token: 0x0600AE1B RID: 44571 RVA: 0x00071507 File Offset: 0x0006F707
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].biome != BiomeDefOf.Ocean;
		}
	}
}
