using System;

namespace Verse
{
	// Token: 0x020001B0 RID: 432
	public sealed class EdificeGrid
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x000418C2 File Offset: 0x0003FAC2
		public Building[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x1700024F RID: 591
		public Building this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x17000250 RID: 592
		public Building this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x000418EE File Offset: 0x0003FAEE
		public EdificeGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Building[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00041914 File Offset: 0x0003FB14
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

		// Token: 0x06000C2F RID: 3119 RVA: 0x0004197C File Offset: 0x0003FB7C
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

		// Token: 0x040009FB RID: 2555
		private Map map;

		// Token: 0x040009FC RID: 2556
		private Building[] innerArray;
	}
}
