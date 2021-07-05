using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200049A RID: 1178
	public static class DijkstraUtility
	{
		// Token: 0x060023E6 RID: 9190 RVA: 0x000DFA58 File Offset: 0x000DDC58
		public static IEnumerable<IntVec3> AdjacentCellsNeighborsGetter(IntVec3 cell, Map map)
		{
			DijkstraUtility.adjacentCells.Clear();
			IntVec3[] array = GenAdj.AdjacentCells;
			for (int i = 0; i < array.Length; i++)
			{
				IntVec3 intVec = cell + array[i];
				if (intVec.InBounds(map))
				{
					DijkstraUtility.adjacentCells.Add(intVec);
				}
			}
			return DijkstraUtility.adjacentCells;
		}

		// Token: 0x040016A6 RID: 5798
		private static List<IntVec3> adjacentCells = new List<IntVec3>();
	}
}
