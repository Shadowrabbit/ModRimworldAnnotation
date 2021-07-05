using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C66 RID: 3174
	public static class ComplexLayoutGenerator
	{
		// Token: 0x06004A29 RID: 18985 RVA: 0x00187C94 File Offset: 0x00185E94
		public static ComplexLayout GenerateRandomLayout(CellRect container, int minRoomWidth = 6, int minRoomHeight = 6, float areaPrunePercent = 0.2f, IntRange? mergeRoomsRange = null, int entranceCount = 1)
		{
			return ComplexLayoutGenerator.GenerateRoomLayout(new ComplexLayoutParams
			{
				container = container,
				areaPrunePercent = areaPrunePercent,
				minRoomHeight = Mathf.Max(minRoomWidth, 4),
				minRoomWidth = Mathf.Max(minRoomHeight, 4),
				mergeRoomsRange = (mergeRoomsRange ?? ComplexLayoutGenerator.DefaultMaxMergedRoomsRange),
				entranceCount = entranceCount
			});
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x00187D08 File Offset: 0x00185F08
		private static ComplexLayout GenerateRoomLayout(ComplexLayoutParams parms)
		{
			ComplexLayout complexLayout = new ComplexLayout();
			complexLayout.Init(parms.container);
			ComplexLayoutGenerator.SplitRandom(parms.container, ComplexLayoutGenerator.tmpRoomRects, parms.minRoomWidth, parms.minRoomHeight);
			ComplexLayoutGenerator.MergeRandom(ComplexLayoutGenerator.tmpRoomRects, ComplexLayoutGenerator.tmpMergedRoomRects, ComplexLayoutGenerator.DefaultMaxMergedRoomsRange, 5);
			foreach (List<CellRect> rects in ComplexLayoutGenerator.tmpMergedRoomRects)
			{
				complexLayout.AddRoom(rects);
			}
			float num = (float)complexLayout.Area - parms.areaPrunePercent * (float)complexLayout.Area;
			int num2 = 100;
			while ((float)complexLayout.Area > num && complexLayout.TryMinimizeLayoutWithoutDisconnection() && num2 > 0)
			{
				num2--;
			}
			complexLayout.Finish();
			ComplexLayoutGenerator.CreateDoors(complexLayout, parms.entranceCount);
			return complexLayout;
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x00187DE8 File Offset: 0x00185FE8
		private static void CreateDoors(ComplexLayout layout, int entranceCount)
		{
			ComplexLayoutGenerator.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.layout = layout;
			HashSet<int> hashSet = new HashSet<int>();
			ComplexLayoutGenerator.tmpCells.Clear();
			ComplexLayoutGenerator.tmpCells.AddRange(CS$<>8__locals1.layout.container.Cells.InRandomOrder(null));
			for (int i = 0; i < ComplexLayoutGenerator.tmpCells.Count; i++)
			{
				IntVec3 intVec = ComplexLayoutGenerator.tmpCells[i];
				if (CS$<>8__locals1.layout.IsWallAt(intVec))
				{
					if (ComplexLayoutGenerator.<CreateDoors>g__IsGoodForHorizontalDoor|6_0(intVec, ref CS$<>8__locals1))
					{
						int roomIdAt = CS$<>8__locals1.layout.GetRoomIdAt(intVec + IntVec3.North);
						int roomIdAt2 = CS$<>8__locals1.layout.GetRoomIdAt(intVec + IntVec3.South);
						bool flag = CS$<>8__locals1.layout.IsOutside(intVec + IntVec3.North) || CS$<>8__locals1.layout.IsOutside(intVec + IntVec3.South);
						int item = Gen.HashOrderless(roomIdAt, roomIdAt2);
						if (hashSet.Contains(item) || (flag && entranceCount <= 0))
						{
							goto IL_1B2;
						}
						CS$<>8__locals1.layout.Add(intVec, RoomLayoutCellType.Door);
						hashSet.Add(item);
						if (flag)
						{
							entranceCount--;
						}
					}
					if (ComplexLayoutGenerator.<CreateDoors>g__IsGoodForVerticalDoor|6_1(intVec, ref CS$<>8__locals1))
					{
						int roomIdAt3 = CS$<>8__locals1.layout.GetRoomIdAt(intVec + IntVec3.East);
						int roomIdAt4 = CS$<>8__locals1.layout.GetRoomIdAt(intVec + IntVec3.West);
						bool flag2 = CS$<>8__locals1.layout.IsOutside(intVec + IntVec3.East) || CS$<>8__locals1.layout.IsOutside(intVec + IntVec3.West);
						int item2 = Gen.HashOrderless(roomIdAt3, roomIdAt4);
						if (!hashSet.Contains(item2) && (!flag2 || entranceCount > 0))
						{
							CS$<>8__locals1.layout.Add(intVec, RoomLayoutCellType.Door);
							hashSet.Add(item2);
							if (flag2)
							{
								entranceCount--;
							}
						}
					}
				}
				IL_1B2:;
			}
			ComplexLayoutGenerator.tmpCells.Clear();
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x00187FC8 File Offset: 0x001861C8
		private static void SplitRandom(CellRect rectToSplit, List<CellRect> rooms, int minWidth, int minHeight)
		{
			ComplexLayoutGenerator.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.minHeight = minHeight;
			CS$<>8__locals1.minWidth = minWidth;
			rooms.Clear();
			Queue<CellRect> queue = new Queue<CellRect>();
			queue.Enqueue(rectToSplit);
			while (queue.Count > 0)
			{
				CellRect cellRect = queue.Dequeue();
				if (!ComplexLayoutGenerator.<SplitRandom>g__CanSplit|7_0(cellRect, ref CS$<>8__locals1))
				{
					rooms.Add(cellRect);
				}
				else if (cellRect.Width > cellRect.Height)
				{
					int num = Rand.Range(CS$<>8__locals1.minWidth, cellRect.Width - CS$<>8__locals1.minWidth);
					CellRect item = new CellRect(cellRect.minX, cellRect.minZ, num, cellRect.Height);
					CellRect item2 = new CellRect(cellRect.minX + num, cellRect.minZ, cellRect.Width - num, cellRect.Height);
					queue.Enqueue(item);
					queue.Enqueue(item2);
				}
				else
				{
					int num2 = Rand.Range(CS$<>8__locals1.minHeight, cellRect.Height - CS$<>8__locals1.minHeight);
					CellRect item3 = new CellRect(cellRect.minX, cellRect.minZ + num2, cellRect.Width, cellRect.Height - num2);
					CellRect item4 = new CellRect(cellRect.minX, cellRect.minZ, cellRect.Width, num2);
					queue.Enqueue(item3);
					queue.Enqueue(item4);
				}
			}
		}

		// Token: 0x06004A2D RID: 18989 RVA: 0x00188110 File Offset: 0x00186310
		private static void MergeRandom(List<CellRect> rects, List<List<CellRect>> mergedRects, IntRange maxMergedRooms, int minAdjacenyScore = 5)
		{
			ComplexLayoutGenerator.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.mergedRects = mergedRects;
			CS$<>8__locals1.mergedRects.Clear();
			rects.Shuffle<CellRect>();
			for (int i = 0; i < rects.Count; i++)
			{
				CellRect cellRect = rects[i];
				if (!ComplexLayoutGenerator.<MergeRandom>g__Used|8_0(cellRect, ref CS$<>8__locals1))
				{
					List<CellRect> list = new List<CellRect>
					{
						cellRect
					};
					int num = Math.Max(1, maxMergedRooms.RandomInRange);
					int num2 = 0;
					while (num2 < rects.Count && list.Count < num)
					{
						CellRect cellRect2 = rects[num2];
						if (!(cellRect == cellRect2) && !ComplexLayoutGenerator.<MergeRandom>g__Used|8_0(cellRect2, ref CS$<>8__locals1) && cellRect.GetAdjacencyScore(cellRect2) >= minAdjacenyScore)
						{
							list.Add(cellRect2);
						}
						num2++;
					}
					CS$<>8__locals1.mergedRects.Add(list);
				}
			}
		}

		// Token: 0x06004A2F RID: 18991 RVA: 0x00188204 File Offset: 0x00186404
		[CompilerGenerated]
		internal static bool <CreateDoors>g__IsGoodForHorizontalDoor|6_0(IntVec3 p, ref ComplexLayoutGenerator.<>c__DisplayClass6_0 A_1)
		{
			return A_1.layout.IsWallAt(p + IntVec3.West) && A_1.layout.IsWallAt(p + IntVec3.East) && !A_1.layout.IsWallAt(p + IntVec3.North) && !A_1.layout.IsWallAt(p + IntVec3.South);
		}

		// Token: 0x06004A30 RID: 18992 RVA: 0x00188274 File Offset: 0x00186474
		[CompilerGenerated]
		internal static bool <CreateDoors>g__IsGoodForVerticalDoor|6_1(IntVec3 p, ref ComplexLayoutGenerator.<>c__DisplayClass6_0 A_1)
		{
			return A_1.layout.IsWallAt(p + IntVec3.North) && A_1.layout.IsWallAt(p + IntVec3.South) && !A_1.layout.IsWallAt(p + IntVec3.East) && !A_1.layout.IsWallAt(p + IntVec3.West);
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x001882E4 File Offset: 0x001864E4
		[CompilerGenerated]
		internal static bool <SplitRandom>g__CanSplit|7_0(CellRect r, ref ComplexLayoutGenerator.<>c__DisplayClass7_0 A_1)
		{
			return r.Height > 2 * A_1.minHeight || r.Width > 2 * A_1.minWidth;
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x0018830C File Offset: 0x0018650C
		[CompilerGenerated]
		internal static bool <MergeRandom>g__Used|8_0(CellRect rect, ref ComplexLayoutGenerator.<>c__DisplayClass8_0 A_1)
		{
			for (int i = 0; i < A_1.mergedRects.Count; i++)
			{
				for (int j = 0; j < A_1.mergedRects[i].Count; j++)
				{
					if (A_1.mergedRects[i][j] == rect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04002D0B RID: 11531
		private static IntRange DefaultMaxMergedRoomsRange = new IntRange(1, 3);

		// Token: 0x04002D0C RID: 11532
		private static List<CellRect> tmpRoomRects = new List<CellRect>();

		// Token: 0x04002D0D RID: 11533
		private static List<List<CellRect>> tmpMergedRoomRects = new List<List<CellRect>>();

		// Token: 0x04002D0E RID: 11534
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
