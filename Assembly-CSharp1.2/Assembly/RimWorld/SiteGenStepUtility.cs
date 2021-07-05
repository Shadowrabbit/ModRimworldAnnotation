using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D9 RID: 4825
	public static class SiteGenStepUtility
	{
		// Token: 0x0600686B RID: 26731 RVA: 0x002035DC File Offset: 0x002017DC
		public static bool TryFindRootToSpawnAroundRectOfInterest(out CellRect rectToDefend, out IntVec3 singleCellToSpawnNear, Map map)
		{
			singleCellToSpawnNear = IntVec3.Invalid;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rectToDefend))
			{
				rectToDefend = CellRect.Empty;
				if (!RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 225, map, out singleCellToSpawnNear))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600686C RID: 26732 RVA: 0x00203638 File Offset: 0x00201838
		public static bool TryFindSpawnCellAroundOrNear(CellRect around, IntVec3 near, Map map, out IntVec3 spawnCell)
		{
			if (near.IsValid)
			{
				if (!CellFinder.TryFindRandomSpawnCellForPawnNear(near, map, out spawnCell, 10))
				{
					return false;
				}
			}
			else if (!CellFinder.TryFindRandomCellInsideWith(around.ExpandedBy(8), (IntVec3 x) => !around.Contains(x) && x.InBounds(map) && x.Standable(map) && !x.Fogged(map), out spawnCell))
			{
				return false;
			}
			return true;
		}
	}
}
