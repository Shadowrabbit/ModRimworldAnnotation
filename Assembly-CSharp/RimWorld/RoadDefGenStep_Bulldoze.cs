using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129C RID: 4764
	public class RoadDefGenStep_Bulldoze : RoadDefGenStep
	{
		// Token: 0x0600679F RID: 26527 RVA: 0x001FED08 File Offset: 0x001FCF08
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
