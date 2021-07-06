using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000260 RID: 608
	public sealed class BlueprintGrid
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x00011844 File Offset: 0x0000FA44
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0001184C File Offset: 0x0000FA4C
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x000B67A8 File Offset: 0x000B49A8
		public void Register(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					if (this.innerArray[num] == null)
					{
						this.innerArray[num] = new List<Blueprint>();
					}
					this.innerArray[num].Add(ed);
				}
			}
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x000B6828 File Offset: 0x000B4A28
		public void DeRegister(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					this.innerArray[num].Remove(ed);
					if (this.innerArray[num].Count == 0)
					{
						this.innerArray[num] = null;
					}
				}
			}
		}

		// Token: 0x04000CA0 RID: 3232
		private Map map;

		// Token: 0x04000CA1 RID: 3233
		private List<Blueprint>[] innerArray;
	}
}
