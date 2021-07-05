using System;

namespace Verse
{
	// Token: 0x020001AD RID: 429
	public class CellGrid
	{
		// Token: 0x17000247 RID: 583
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

		// Token: 0x17000248 RID: 584
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

		// Token: 0x17000249 RID: 585
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

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x00041190 File Offset: 0x0003F390
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000033AC File Offset: 0x000015AC
		public CellGrid()
		{
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0004119A File Offset: 0x0003F39A
		public CellGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000411A9 File Offset: 0x0003F3A9
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x000411D4 File Offset: 0x0003F3D4
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

		// Token: 0x06000C11 RID: 3089 RVA: 0x0004123C File Offset: 0x0003F43C
		public void Clear()
		{
			int num = CellIndicesUtility.CellToIndex(IntVec3.Invalid, this.mapSizeX);
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = num;
			}
		}

		// Token: 0x040009EF RID: 2543
		private int[] grid;

		// Token: 0x040009F0 RID: 2544
		private int mapSizeX;

		// Token: 0x040009F1 RID: 2545
		private int mapSizeZ;
	}
}
