using System;

namespace Verse
{
	// Token: 0x020001FE RID: 510
	public static class RegionAndRoomQuery
	{
		// Token: 0x06000E69 RID: 3689 RVA: 0x0005196C File Offset: 0x0004FB6C
		public static Region RegionAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!c.InBounds(map))
			{
				return null;
			}
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt;
			}
			return null;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x000519A1 File Offset: 0x0004FBA1
		public static Region GetRegion(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return RegionAndRoomQuery.RegionAt(thing.Position, thing.Map, allowedRegionTypes);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x000519C0 File Offset: 0x0004FBC0
		public static District DistrictAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region region = RegionAndRoomQuery.RegionAt(c, map, allowedRegionTypes);
			if (region == null)
			{
				return null;
			}
			return region.District;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000519E4 File Offset: 0x0004FBE4
		public static Room RoomAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_All)
		{
			District district = RegionAndRoomQuery.DistrictAt(c, map, allowedRegionTypes);
			if (district == null)
			{
				return null;
			}
			return district.Room;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00051A05 File Offset: 0x0004FC05
		public static District GetDistrict(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return RegionAndRoomQuery.DistrictAt(thing.Position, thing.Map, allowedRegionTypes);
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00051A24 File Offset: 0x0004FC24
		public static Room GetRoom(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_All)
		{
			District district = thing.GetDistrict(allowedRegionTypes);
			if (district == null)
			{
				return null;
			}
			return district.Room;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00051A44 File Offset: 0x0004FC44
		public static District DistirctAtFast(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt.District;
			}
			return null;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00051A74 File Offset: 0x0004FC74
		public static Room RoomAtOrAdjacent(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, allowedRegionTypes);
			if (room != null)
			{
				return room;
			}
			for (int i = 0; i < 8; i++)
			{
				room = RegionAndRoomQuery.RoomAt(c + GenAdj.AdjacentCells[i], map, allowedRegionTypes);
				if (room != null)
				{
					return room;
				}
			}
			return room;
		}
	}
}
