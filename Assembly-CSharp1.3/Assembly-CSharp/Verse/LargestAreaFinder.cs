using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x020004CB RID: 1227
	public static class LargestAreaFinder
	{
		// Token: 0x06002556 RID: 9558 RVA: 0x000E8EF0 File Offset: 0x000E70F0
		public static CellRect FindLargestRect(Map map, Predicate<IntVec3> predicate, int breakEarlyOn = -1)
		{
			LargestAreaFinder.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.breakEarlyOn = breakEarlyOn;
			if (LargestAreaFinder.visited == null)
			{
				LargestAreaFinder.visited = new BoolGrid(map);
			}
			LargestAreaFinder.visited.ClearAndResizeTo(map);
			Rand.PushState(map.uniqueID ^ 484111219);
			CS$<>8__locals1.largestRect = CellRect.Empty;
			for (int i = 0; i < 3; i++)
			{
				LargestAreaFinder.tmpProcessed.Clear();
				foreach (IntVec3 c in map.cellsInRandomOrder.GetAll().InRandomOrder(LargestAreaFinder.randomOrderWorkingList))
				{
					CellRect largestRect = LargestAreaFinder.FindLargestRectAt(c, map, LargestAreaFinder.tmpProcessed, predicate);
					if (largestRect.Area > CS$<>8__locals1.largestRect.Area)
					{
						CS$<>8__locals1.largestRect = largestRect;
						if (LargestAreaFinder.<FindLargestRect>g__ShouldBreakEarly|3_0(ref CS$<>8__locals1))
						{
							break;
						}
					}
				}
				if (LargestAreaFinder.<FindLargestRect>g__ShouldBreakEarly|3_0(ref CS$<>8__locals1))
				{
					break;
				}
			}
			Rand.PopState();
			return CS$<>8__locals1.largestRect;
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000E8FEC File Offset: 0x000E71EC
		private static CellRect FindLargestRectAt(IntVec3 c, Map map, HashSet<IntVec3> processed, Predicate<IntVec3> predicate)
		{
			if (processed.Contains(c) || !predicate(c))
			{
				return CellRect.Empty;
			}
			return LargestAreaFinder.ExpandRect(CellRect.SingleCell(c), map, processed, predicate, true);
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000E9018 File Offset: 0x000E7218
		public static CellRect ExpandRect(CellRect rect, Map map, HashSet<IntVec3> processed, Predicate<IntVec3> predicate, bool keepSquare = false)
		{
			LargestAreaFinder.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.rect = rect;
			CS$<>8__locals1.processed = processed;
			CS$<>8__locals1.predicate = predicate;
			bool flag;
			do
			{
				flag = false;
				if (!keepSquare || CS$<>8__locals1.rect.Width <= CS$<>8__locals1.rect.Height)
				{
					if (CS$<>8__locals1.rect.maxX + 1 < map.Size.x && LargestAreaFinder.<ExpandRect>g__CanExpand|5_0(Rot4.East, ref CS$<>8__locals1))
					{
						CS$<>8__locals1.rect.maxX = CS$<>8__locals1.rect.maxX + 1;
						flag = true;
					}
					if (CS$<>8__locals1.rect.minX > 0 && LargestAreaFinder.<ExpandRect>g__CanExpand|5_0(Rot4.West, ref CS$<>8__locals1))
					{
						CS$<>8__locals1.rect.minX = CS$<>8__locals1.rect.minX - 1;
						flag = true;
					}
				}
				if (!keepSquare || CS$<>8__locals1.rect.Height <= CS$<>8__locals1.rect.Width)
				{
					if (CS$<>8__locals1.rect.maxZ + 1 < map.Size.z && LargestAreaFinder.<ExpandRect>g__CanExpand|5_0(Rot4.North, ref CS$<>8__locals1))
					{
						CS$<>8__locals1.rect.maxZ = CS$<>8__locals1.rect.maxZ + 1;
						flag = true;
					}
					if (CS$<>8__locals1.rect.minZ > 0 && LargestAreaFinder.<ExpandRect>g__CanExpand|5_0(Rot4.South, ref CS$<>8__locals1))
					{
						CS$<>8__locals1.rect.minZ = CS$<>8__locals1.rect.minZ - 1;
						flag = true;
					}
				}
			}
			while (flag);
			foreach (IntVec3 item in CS$<>8__locals1.rect)
			{
				CS$<>8__locals1.processed.Add(item);
			}
			return CS$<>8__locals1.rect;
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000E91BE File Offset: 0x000E73BE
		[CompilerGenerated]
		internal static bool <FindLargestRect>g__ShouldBreakEarly|3_0(ref LargestAreaFinder.<>c__DisplayClass3_0 A_0)
		{
			return A_0.breakEarlyOn >= 0 && A_0.largestRect.Width >= A_0.breakEarlyOn && A_0.largestRect.Height >= A_0.breakEarlyOn;
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000E91F4 File Offset: 0x000E73F4
		[CompilerGenerated]
		internal static bool <ExpandRect>g__CanExpand|5_0(Rot4 dir, ref LargestAreaFinder.<>c__DisplayClass5_0 A_1)
		{
			foreach (IntVec3 a in A_1.rect.GetEdgeCells(dir))
			{
				IntVec3 intVec = a + dir.FacingCell;
				if (A_1.processed.Contains(intVec) || !A_1.predicate(intVec))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400173D RID: 5949
		private static BoolGrid visited;

		// Token: 0x0400173E RID: 5950
		private static List<IntVec3> randomOrderWorkingList = new List<IntVec3>();

		// Token: 0x0400173F RID: 5951
		private static HashSet<IntVec3> tmpProcessed = new HashSet<IntVec3>();
	}
}
