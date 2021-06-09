using System;

namespace Verse
{
	// Token: 0x020002B8 RID: 696
	public class MapGenFloatGrid
	{
		// Token: 0x17000345 RID: 837
		public float this[IntVec3 c]
		{
			get
			{
				return this.grid[this.map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.grid[this.map.cellIndices.CellToIndex(c)] = value;
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00012D8A File Offset: 0x00010F8A
		public MapGenFloatGrid(Map map)
		{
			this.map = map;
			this.grid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x04000E58 RID: 3672
		private Map map;

		// Token: 0x04000E59 RID: 3673
		private float[] grid;
	}
}
