using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001736 RID: 5942
	public class FeatureWorker_MountainRange : FeatureWorker_Cluster
	{
		// Token: 0x06008919 RID: 35097 RVA: 0x00314888 File Offset: 0x00312A88
		protected override bool IsRoot(int tile)
		{
			return Find.WorldGrid[tile].hilliness != Hilliness.Flat;
		}

		// Token: 0x0600891A RID: 35098 RVA: 0x003148A0 File Offset: 0x00312AA0
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].biome != BiomeDefOf.Ocean;
		}
	}
}
