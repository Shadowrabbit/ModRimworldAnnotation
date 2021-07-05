using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004B9 RID: 1209
	public static class GenMorphology
	{
		// Token: 0x06002506 RID: 9478 RVA: 0x000E66F8 File Offset: 0x000E48F8
		public static void Erode(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			GenMorphology.cellsSet.Clear();
			GenMorphology.cellsSet.AddRange(cells);
			GenMorphology.tmpEdgeCells.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					IntVec3 item = cells[i] + cardinalDirections[j];
					if (!GenMorphology.cellsSet.Contains(item))
					{
						GenMorphology.tmpEdgeCells.Add(cells[i]);
						break;
					}
				}
			}
			if (!GenMorphology.tmpEdgeCells.Any<IntVec3>())
			{
				return;
			}
			GenMorphology.tmpOutput.Clear();
			Predicate<IntVec3> passCheck;
			if (extraPredicate != null)
			{
				passCheck = ((IntVec3 x) => GenMorphology.cellsSet.Contains(x) && extraPredicate(x));
			}
			else
			{
				passCheck = ((IntVec3 x) => GenMorphology.cellsSet.Contains(x));
			}
			map.floodFiller.FloodFill(IntVec3.Invalid, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				if (traversalDist >= count)
				{
					GenMorphology.tmpOutput.Add(cell);
				}
				return false;
			}, int.MaxValue, false, GenMorphology.tmpEdgeCells);
			cells.Clear();
			cells.AddRange(GenMorphology.tmpOutput);
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x000E6828 File Offset: 0x000E4A28
		public static void Dilate(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			FloodFiller floodFiller = map.floodFiller;
			IntVec3 invalid = IntVec3.Invalid;
			Predicate<IntVec3> passCheck = extraPredicate;
			if (extraPredicate == null && (passCheck = GenMorphology.<>c.<>9__4_0) == null)
			{
				passCheck = (GenMorphology.<>c.<>9__4_0 = ((IntVec3 x) => true));
			}
			floodFiller.FloodFill(invalid, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				if (traversalDist > count)
				{
					return true;
				}
				if (traversalDist != 0)
				{
					cells.Add(cell);
				}
				return false;
			}, int.MaxValue, false, cells);
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x000E689F File Offset: 0x000E4A9F
		public static void Open(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Erode(cells, count, map, null);
			GenMorphology.Dilate(cells, count, map, null);
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000E68B3 File Offset: 0x000E4AB3
		public static void Close(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Dilate(cells, count, map, null);
			GenMorphology.Erode(cells, count, map, null);
		}

		// Token: 0x04001718 RID: 5912
		private static HashSet<IntVec3> tmpOutput = new HashSet<IntVec3>();

		// Token: 0x04001719 RID: 5913
		private static HashSet<IntVec3> cellsSet = new HashSet<IntVec3>();

		// Token: 0x0400171A RID: 5914
		private static List<IntVec3> tmpEdgeCells = new List<IntVec3>();
	}
}
