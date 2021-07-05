using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001734 RID: 5940
	public class FeatureWorker_Archipelago : FeatureWorker_Cluster
	{
		// Token: 0x06008906 RID: 35078 RVA: 0x003142C8 File Offset: 0x003124C8
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06008907 RID: 35079 RVA: 0x003142FB File Offset: 0x003124FB
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			return true;
		}

		// Token: 0x06008908 RID: 35080 RVA: 0x00314304 File Offset: 0x00312504
		protected override bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			bool flag;
			return base.IsMember(tile, out flag);
		}
	}
}
