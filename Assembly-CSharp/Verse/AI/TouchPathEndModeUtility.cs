using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A61 RID: 2657
	public static class TouchPathEndModeUtility
	{
		// Token: 0x06003F3A RID: 16186 RVA: 0x0017CD64 File Offset: 0x0017AF64
		public static bool IsCornerTouchAllowed(int cornerX, int cornerZ, int adjCardinal1X, int adjCardinal1Z, int adjCardinal2X, int adjCardinal2Z, Map map)
		{
			Building building = map.edificeGrid[new IntVec3(cornerX, 0, cornerZ)];
			if (building != null && TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(building.def))
			{
				return true;
			}
			IntVec3 intVec = new IntVec3(adjCardinal1X, 0, adjCardinal1Z);
			IntVec3 intVec2 = new IntVec3(adjCardinal2X, 0, adjCardinal2Z);
			return (map.pathGrid.Walkable(intVec) && intVec.GetDoor(map) == null) || (map.pathGrid.Walkable(intVec2) && intVec2.GetDoor(map) == null);
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x0017CDE4 File Offset: 0x0017AFE4
		public static bool MakesOccupiedCellsAlwaysReachableDiagonally(ThingDef def)
		{
			ThingDef thingDef = def.IsFrame ? (def.entityDefToBuild as ThingDef) : def;
			return thingDef != null && thingDef.CanInteractThroughCorners;
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x0017CE18 File Offset: 0x0017B018
		public static bool IsAdjacentCornerAndNotAllowed(IntVec3 cell, IntVec3 BL, IntVec3 TL, IntVec3 TR, IntVec3 BR, Map map)
		{
			return (cell == BL && !TouchPathEndModeUtility.IsCornerTouchAllowed(BL.x + 1, BL.z + 1, BL.x + 1, BL.z, BL.x, BL.z + 1, map)) || (cell == TL && !TouchPathEndModeUtility.IsCornerTouchAllowed(TL.x + 1, TL.z - 1, TL.x + 1, TL.z, TL.x, TL.z - 1, map)) || (cell == TR && !TouchPathEndModeUtility.IsCornerTouchAllowed(TR.x - 1, TR.z - 1, TR.x - 1, TR.z, TR.x, TR.z - 1, map)) || (cell == BR && !TouchPathEndModeUtility.IsCornerTouchAllowed(BR.x - 1, BR.z + 1, BR.x - 1, BR.z, BR.x, BR.z + 1, map));
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x0017CF30 File Offset: 0x0017B130
		public static void AddAllowedAdjacentRegions(LocalTargetInfo dest, TraverseParms traverseParams, Map map, List<Region> regions)
		{
			IntVec3 bl;
			IntVec3 tl;
			IntVec3 tr;
			IntVec3 br;
			GenAdj.GetAdjacentCorners(dest, out bl, out tl, out tr, out br);
			if (!dest.HasThing || (dest.Thing.def.size.x == 1 && dest.Thing.def.size.z == 1))
			{
				IntVec3 cell = dest.Cell;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = GenAdj.AdjacentCells[i] + cell;
					if (intVec.InBounds(map) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(intVec, bl, tl, tr, br, map))
					{
						Region region = intVec.GetRegion(map, RegionType.Set_Passable);
						if (region != null && region.Allows(traverseParams, true))
						{
							regions.Add(region);
						}
					}
				}
				return;
			}
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(dest);
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].InBounds(map) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(list[j], bl, tl, tr, br, map))
				{
					Region region2 = list[j].GetRegion(map, RegionType.Set_Passable);
					if (region2 != null && region2.Allows(traverseParams, true))
					{
						regions.Add(region2);
					}
				}
			}
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x0017D060 File Offset: 0x0017B260
		public static bool IsAdjacentOrInsideAndAllowedToTouch(IntVec3 root, LocalTargetInfo target, Map map)
		{
			IntVec3 bl;
			IntVec3 tl;
			IntVec3 tr;
			IntVec3 br;
			GenAdj.GetAdjacentCorners(target, out bl, out tl, out tr, out br);
			return root.AdjacentTo8WayOrInside(target) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(root, bl, tl, tr, br, map);
		}
	}
}
