using System;

namespace Verse
{
	// Token: 0x020002D2 RID: 722
	public static class RegionAndRoomQuery
	{
		// Token: 0x06001252 RID: 4690 RVA: 0x000C5F64 File Offset: 0x000C4164
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

		// Token: 0x06001253 RID: 4691 RVA: 0x0001339B File Offset: 0x0001159B
		public static Region GetRegion(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return RegionAndRoomQuery.RegionAt(thing.Position, thing.Map, allowedRegionTypes);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x000C5F9C File Offset: 0x000C419C
		public static Room RoomAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region region = RegionAndRoomQuery.RegionAt(c, map, allowedRegionTypes);
			if (region == null)
			{
				return null;
			}
			return region.Room;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x000C5FC0 File Offset: 0x000C41C0
		public static RoomGroup RoomGroupAt(IntVec3 c, Map map)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, RegionType.Set_All);
			if (room == null)
			{
				return null;
			}
			return room.Group;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x000133B9 File Offset: 0x000115B9
		public static Room GetRoom(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return RegionAndRoomQuery.RoomAt(thing.Position, thing.Map, allowedRegionTypes);
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x000C5FE4 File Offset: 0x000C41E4
		public static RoomGroup GetRoomGroup(this Thing thing)
		{
			Room room = thing.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return null;
			}
			return room.Group;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x000C6004 File Offset: 0x000C4204
		public static Room RoomAtFast(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt.Room;
			}
			return null;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x000C6034 File Offset: 0x000C4234
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
