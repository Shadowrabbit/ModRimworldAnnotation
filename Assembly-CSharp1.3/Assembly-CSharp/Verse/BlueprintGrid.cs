using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001AA RID: 426
	public sealed class BlueprintGrid
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00040AE8 File Offset: 0x0003ECE8
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00040AF0 File Offset: 0x0003ECF0
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00040B18 File Offset: 0x0003ED18
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

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00040B98 File Offset: 0x0003ED98
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

		// Token: 0x040009E4 RID: 2532
		private Map map;

		// Token: 0x040009E5 RID: 2533
		private List<Blueprint>[] innerArray;
	}
}
