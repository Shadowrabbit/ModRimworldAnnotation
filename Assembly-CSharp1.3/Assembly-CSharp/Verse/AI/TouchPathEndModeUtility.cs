using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000609 RID: 1545
	public static class TouchPathEndModeUtility
	{
		// Token: 0x06002C81 RID: 11393 RVA: 0x00109B6C File Offset: 0x00107D6C
		public static bool IsCornerTouchAllowed(int cornerX, int cornerZ, int adjCardinal1X, int adjCardinal1Z, int adjCardinal2X, int adjCardinal2Z, PathingContext pc)
		{
			Building building = pc.map.edificeGrid[new IntVec3(cornerX, 0, cornerZ)];
			if (building != null && TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(building.def))
			{
				return true;
			}
			IntVec3 intVec = new IntVec3(adjCardinal1X, 0, adjCardinal1Z);
			IntVec3 intVec2 = new IntVec3(adjCardinal2X, 0, adjCardinal2Z);
			return (pc.pathGrid.Walkable(intVec) && intVec.GetDoor(pc.map) == null) || (pc.pathGrid.Walkable(intVec2) && intVec2.GetDoor(pc.map) == null);
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x00109BFC File Offset: 0x00107DFC
		public static bool MakesOccupiedCellsAlwaysReachableDiagonally(ThingDef def)
		{
			ThingDef thingDef = def.IsFrame ? (def.entityDefToBuild as ThingDef) : def;
			return thingDef != null && thingDef.CanInteractThroughCorners;
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x00109C30 File Offset: 0x00107E30
		public static bool IsAdjacentCornerAndNotAllowed(IntVec3 cell, IntVec3 BL, IntVec3 TL, IntVec3 TR, IntVec3 BR, PathingContext pc)
		{
			return (cell == BL && !TouchPathEndModeUtility.IsCornerTouchAllowed(BL.x + 1, BL.z + 1, BL.x + 1, BL.z, BL.x, BL.z + 1, pc)) || (cell == TL && !TouchPathEndModeUtility.IsCornerTouchAllowed(TL.x + 1, TL.z - 1, TL.x + 1, TL.z, TL.x, TL.z - 1, pc)) || (cell == TR && !TouchPathEndModeUtility.IsCornerTouchAllowed(TR.x - 1, TR.z - 1, TR.x - 1, TR.z, TR.x, TR.z - 1, pc)) || (cell == BR && !TouchPathEndModeUtility.IsCornerTouchAllowed(BR.x - 1, BR.z + 1, BR.x - 1, BR.z, BR.x, BR.z + 1, pc));
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x00109D48 File Offset: 0x00107F48
		public static void AddAllowedAdjacentRegions(LocalTargetInfo dest, TraverseParms traverseParams, Map map, List<Region> regions)
		{
			IntVec3 bl;
			IntVec3 tl;
			IntVec3 tr;
			IntVec3 br;
			GenAdj.GetAdjacentCorners(dest, out bl, out tl, out tr, out br);
			PathingContext pc = map.pathing.For(traverseParams);
			if (!dest.HasThing || (dest.Thing.def.size.x == 1 && dest.Thing.def.size.z == 1))
			{
				IntVec3 cell = dest.Cell;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = GenAdj.AdjacentCells[i] + cell;
					if (intVec.InBounds(map) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(intVec, bl, tl, tr, br, pc))
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
				if (list[j].InBounds(map) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(list[j], bl, tl, tr, br, pc))
				{
					Region region2 = list[j].GetRegion(map, RegionType.Set_Passable);
					if (region2 != null && region2.Allows(traverseParams, true))
					{
						regions.Add(region2);
					}
				}
			}
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x00109E8C File Offset: 0x0010808C
		public static bool IsAdjacentOrInsideAndAllowedToTouch(IntVec3 root, LocalTargetInfo target, PathingContext pc)
		{
			IntVec3 bl;
			IntVec3 tl;
			IntVec3 tr;
			IntVec3 br;
			GenAdj.GetAdjacentCorners(target, out bl, out tl, out tr, out br);
			return root.AdjacentTo8WayOrInside(target) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(root, bl, tl, tr, br, pc);
		}
	}
}
