using System;

namespace Verse
{
	// Token: 0x020001ED RID: 493
	public class MapGenFloatGrid
	{
		// Token: 0x170002AF RID: 687
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

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0004E661 File Offset: 0x0004C861
		public MapGenFloatGrid(Map map)
		{
			this.map = map;
			this.grid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x04000B5B RID: 2907
		private Map map;

		// Token: 0x04000B5C RID: 2908
		private float[] grid;
	}
}
