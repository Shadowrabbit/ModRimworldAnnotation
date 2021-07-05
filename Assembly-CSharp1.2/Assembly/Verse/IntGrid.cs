using System;

namespace Verse
{
	// Token: 0x0200026B RID: 619
	public sealed class IntGrid
	{
		// Token: 0x170002E0 RID: 736
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

		// Token: 0x170002E1 RID: 737
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

		// Token: 0x170002E2 RID: 738
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

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x00011E35 File Offset: 0x00010035
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x00006B8B File Offset: 0x00004D8B
		public IntGrid()
		{
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00011E3F File Offset: 0x0001003F
		public IntGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00011E4E File Offset: 0x0001004E
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x000B7B70 File Offset: 0x000B5D70
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

		// Token: 0x06000FE9 RID: 4073 RVA: 0x000B7BD0 File Offset: 0x000B5DD0
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

		// Token: 0x06000FEA RID: 4074 RVA: 0x000B7C14 File Offset: 0x000B5E14
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

		// Token: 0x04000CC5 RID: 3269
		private int[] grid;

		// Token: 0x04000CC6 RID: 3270
		private int mapSizeX;

		// Token: 0x04000CC7 RID: 3271
		private int mapSizeZ;
	}
}
