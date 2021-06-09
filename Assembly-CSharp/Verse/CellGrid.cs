using System;

namespace Verse
{
	// Token: 0x02000264 RID: 612
	public class CellGrid
	{
		// Token: 0x170002D2 RID: 722
		public IntVec3 this[IntVec3 c]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x170002D3 RID: 723
		public IntVec3 this[int index]
		{
			get
			{
				return CellIndicesUtility.IndexToCell(this.grid[index], this.mapSizeX);
			}
			set
			{
				this.grid[index] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x170002D4 RID: 724
		public IntVec3 this[int x, int z]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x00011AA9 File Offset: 0x0000FCA9
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00006B8B File Offset: 0x00004D8B
		public CellGrid()
		{
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x00011AB3 File Offset: 0x0000FCB3
		public CellGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00011AC2 File Offset: 0x0000FCC2
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x000B6D70 File Offset: 0x000B4F70
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear();
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new int[this.mapSizeX * this.mapSizeZ];
			this.Clear();
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x000B6DD8 File Offset: 0x000B4FD8
		public void Clear()
		{
			int num = CellIndicesUtility.CellToIndex(IntVec3.Invalid, this.mapSizeX);
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = num;
			}
		}

		// Token: 0x04000CB2 RID: 3250
		private int[] grid;

		// Token: 0x04000CB3 RID: 3251
		private int mapSizeX;

		// Token: 0x04000CB4 RID: 3252
		private int mapSizeZ;
	}
}
