using System;

namespace Verse
{
	// Token: 0x02000267 RID: 615
	public sealed class EdificeGrid
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x00011C0E File Offset: 0x0000FE0E
		public Building[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x170002DA RID: 730
		public Building this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x170002DB RID: 731
		public Building this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00011C3A File Offset: 0x0000FE3A
		public EdificeGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Building[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x000B7334 File Offset: 0x000B5534
		public void Register(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.innerArray[cellIndices.CellToIndex(c)] = ed;
				}
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x000B739C File Offset: 0x000B559C
		public void DeRegister(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.innerArray[cellIndices.CellToIndex(j, i)] = null;
				}
			}
		}

		// Token: 0x04000CBE RID: 3262
		private Map map;

		// Token: 0x04000CBF RID: 3263
		private Building[] innerArray;
	}
}
