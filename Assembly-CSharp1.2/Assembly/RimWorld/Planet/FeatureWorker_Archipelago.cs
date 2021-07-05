using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002017 RID: 8215
	public class FeatureWorker_Archipelago : FeatureWorker_Cluster
	{
		// Token: 0x0600ADFE RID: 44542 RVA: 0x0032A75C File Offset: 0x0032895C
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x0600ADFF RID: 44543 RVA: 0x00071408 File Offset: 0x0006F608
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			return true;
		}

		// Token: 0x0600AE00 RID: 44544 RVA: 0x0032A790 File Offset: 0x00328990
		protected override bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			bool flag;
			return base.IsMember(tile, out flag);
		}
	}
}
