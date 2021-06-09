using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200080D RID: 2061
	public static class DijkstraUtility
	{
		// Token: 0x060033E4 RID: 13284 RVA: 0x001512C4 File Offset: 0x0014F4C4
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

		// Token: 0x040023F9 RID: 9209
		private static List<IntVec3> adjacentCells = new List<IntVec3>();
	}
}
