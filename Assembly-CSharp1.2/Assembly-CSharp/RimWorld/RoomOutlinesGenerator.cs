using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BB RID: 4795
	public static class RoomOutlinesGenerator
	{
		// Token: 0x06006807 RID: 26631 RVA: 0x00201AF4 File Offset: 0x001FFCF4
		public static List<RoomOutline> GenerateRoomOutlines(CellRect initialRect, Map map, int divisionsCount, int finalRoomsCount, int maxRoomCells, int minTotalRoomsNonWallCellsCount)
		{
			int num = 0;
			List<RoomOutline> list;
			for (;;)
			{
				list = RoomOutlinesGenerator.GenerateRoomOutlines(initialRect, map, divisionsCount, finalRoomsCount, maxRoomCells);
				int num2 = 0;
				for (int i = 0; i < list.Count; i++)
				{
					num2 += list[i].CellsCountIgnoringWalls;
				}
				if (num2 >= minTotalRoomsNonWallCellsCount)
				{
					break;
				}
				num++;
				if (num > 15)
				{
					return list;
				}
			}
			return list;
		}

		// Token: 0x06006808 RID: 26632 RVA: 0x00201B44 File Offset: 0x001FFD44
		public static List<RoomOutline> GenerateRoomOutlines(CellRect initialRect, Map map, int divisionsCount, int finalRoomsCount, int maxRoomCells)
		{
			List<RoomOutline> list = new List<RoomOutline>();
			list.Add(new RoomOutline(initialRect));
			Func<RoomOutline, bool> <>9__3;
			for (int i = 0; i < divisionsCount; i++)
			{
				RoomOutline roomOutline;
				if (!(from x in list
				where x.CellsCountIgnoringWalls >= 32
				select x).TryRandomElementByWeight((RoomOutline x) => (float)Mathf.Max(x.rect.Width, x.rect.Height), out roomOutline))
				{
					IL_11F:
					while (list.Any((RoomOutline x) => x.CellsCountIgnoringWalls > maxRoomCells))
					{
						IEnumerable<RoomOutline> source = list;
						Func<RoomOutline, bool> predicate;
						if ((predicate = <>9__3) == null)
						{
							predicate = (<>9__3 = ((RoomOutline x) => x.CellsCountIgnoringWalls > maxRoomCells));
						}
						RoomOutline roomOutline2 = source.Where(predicate).RandomElement<RoomOutline>();
						bool horizontalWall = roomOutline2.rect.Height > roomOutline2.rect.Width;
						RoomOutlinesGenerator.Split(roomOutline2, list, horizontalWall);
					}
					while (list.Count > finalRoomsCount)
					{
						list.Remove(list.RandomElement<RoomOutline>());
					}
					return list;
				}
				bool flag = roomOutline.rect.Height > roomOutline.rect.Width;
				if ((!flag || roomOutline.rect.Height > 6) && (flag || roomOutline.rect.Width > 6))
				{
					RoomOutlinesGenerator.Split(roomOutline, list, flag);
				}
			}
			goto IL_11F;
		}

		// Token: 0x06006809 RID: 26633 RVA: 0x00201CA0 File Offset: 0x001FFEA0
		private static void Split(RoomOutline room, List<RoomOutline> allRooms, bool horizontalWall)
		{
			allRooms.Remove(room);
			if (horizontalWall)
			{
				int z = room.rect.CenterCell.z;
				allRooms.Add(new RoomOutline(new CellRect(room.rect.minX, room.rect.minZ, room.rect.Width, z - room.rect.minZ + 1)));
				allRooms.Add(new RoomOutline(new CellRect(room.rect.minX, z, room.rect.Width, room.rect.maxZ - z + 1)));
				return;
			}
			int x = room.rect.CenterCell.x;
			allRooms.Add(new RoomOutline(new CellRect(room.rect.minX, room.rect.minZ, x - room.rect.minX + 1, room.rect.Height)));
			allRooms.Add(new RoomOutline(new CellRect(x, room.rect.minZ, room.rect.maxX - x + 1, room.rect.Height)));
		}

		// Token: 0x0400454C RID: 17740
		private const int MinFreeRoomCellsToDivide = 32;

		// Token: 0x0400454D RID: 17741
		private const int MinAllowedRoomWidthAndHeight = 2;
	}
}
