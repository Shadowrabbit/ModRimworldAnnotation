using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129E RID: 4766
	public class RoadDefGenStep_DryWithFallback : RoadDefGenStep
	{
		// Token: 0x060067A3 RID: 26531 RVA: 0x00046AC4 File Offset: 0x00044CC4
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, this.fallback);
		}

		// Token: 0x060067A4 RID: 26532 RVA: 0x001FF0B0 File Offset: 0x001FD2B0
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

		// Token: 0x040044FD RID: 17661
		public TerrainDef fallback;
	}
}
