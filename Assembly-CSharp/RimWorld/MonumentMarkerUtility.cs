using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F3 RID: 5875
	public static class MonumentMarkerUtility
	{
		// Token: 0x06008123 RID: 33059 RVA: 0x00264D78 File Offset: 0x00262F78
		public static Building GetFirstAdjacentBuilding(SketchEntity entity, IntVec3 offset, List<Thing> monumentThings, Map map)
		{
			if (entity.IsSameSpawnedOrBlueprintOrFrame(entity.pos + offset, map))
			{
				return null;
			}
			foreach (IntVec3 c in entity.OccupiedRect.MovedBy(offset).ExpandedBy(1).EdgeCells)
			{
				if (c.InBounds(map))
				{
					Building firstBuilding = c.GetFirstBuilding(map);
					if (firstBuilding != null && !monumentThings.Contains(firstBuilding) && (firstBuilding.Faction == null || firstBuilding.Faction == Faction.OfPlayer))
					{
						return firstBuilding;
					}
				}
			}
			return null;
		}
	}
}
