using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C92 RID: 3218
	public class RoadDefGenStep_Bulldoze : RoadDefGenStep
	{
		// Token: 0x06004B26 RID: 19238 RVA: 0x0018EFB0 File Offset: 0x0018D1B0
		public override void Place(Map map, IntVec3 tile, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			while (tile.Impassable(map))
			{
				foreach (Thing thing in tile.GetThingList(map))
				{
					if (thing.def.passability == Traversability.Impassable)
					{
						thing.Destroy(DestroyMode.Vanish);
						break;
					}
				}
			}
		}
	}
}
