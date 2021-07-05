using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C94 RID: 3220
	public class RoadDefGenStep_DryWithFallback : RoadDefGenStep
	{
		// Token: 0x06004B2A RID: 19242 RVA: 0x0018F33B File Offset: 0x0018D53B
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, this.fallback);
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x0018F34C File Offset: 0x0018D54C
		public static void PlaceWorker(Map map, IntVec3 position, TerrainDef fallback)
		{
			while (map.terrainGrid.TerrainAt(position).driesTo != null)
			{
				map.terrainGrid.SetTerrain(position, map.terrainGrid.TerrainAt(position).driesTo);
			}
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(position);
			if (terrainDef.passability == Traversability.Impassable || terrainDef.IsRiver)
			{
				map.terrainGrid.SetTerrain(position, fallback);
			}
		}

		// Token: 0x04002D8F RID: 11663
		public TerrainDef fallback;
	}
}
