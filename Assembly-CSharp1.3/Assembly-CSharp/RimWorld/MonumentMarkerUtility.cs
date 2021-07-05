using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001094 RID: 4244
	public static class MonumentMarkerUtility
	{
		// Token: 0x0600653B RID: 25915 RVA: 0x0022344C File Offset: 0x0022164C
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
