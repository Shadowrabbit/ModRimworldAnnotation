using System;

namespace Verse
{
	// Token: 0x020001B4 RID: 436
	public sealed class IntGrid
	{
		// Token: 0x17000255 RID: 597
		public int this[IntVec3 c]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(c, this.mapSizeX)];
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = value;
			}
		}

		// Token: 0x17000256 RID: 598
		public int this[int index]
		{
			get
			{
				return this.grid[index];
			}
			set
			{
				this.grid[index] = value;
			}
		}

		// Token: 0x17000257 RID: 599
		public int this[int x, int z]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)];
			}
			set
			{
				this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)] = value;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x0004234D File Offset: 0x0004054D
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x000033AC File Offset: 0x000015AC
		public IntGrid()
		{
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00042357 File Offset: 0x00040557
		public IntGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00042366 File Offset: 0x00040566
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00042390 File Offset: 0x00040590
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new int[this.mapSizeX * this.mapSizeZ];
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x000423F0 File Offset: 0x000405F0
		public void Clear(int value = 0)
		{
			if (value == 0)
			{
				Array.Clear(this.grid, 0, this.grid.Length);
				return;
			}
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = value;
			}
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00042434 File Offset: 0x00040634
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				int num = this.grid[i];
				if (num > 0)
				{
					CellRenderer.RenderCell(CellIndicesUtility.IndexToCell(i, this.mapSizeX), (float)(num % 100) / 100f * 0.5f);
				}
			}
		}

		// Token: 0x04000A02 RID: 2562
		private int[] grid;

		// Token: 0x04000A03 RID: 2563
		private int mapSizeX;

		// Token: 0x04000A04 RID: 2564
		private int mapSizeZ;
	}
}
