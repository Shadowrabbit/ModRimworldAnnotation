using System;

namespace Verse
{
	// Token: 0x02000263 RID: 611
	public sealed class ByteGrid : IExposable
	{
		// Token: 0x170002CE RID: 718
		public byte this[IntVec3 c]
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

		// Token: 0x170002CF RID: 719
		public byte this[int index]
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

		// Token: 0x170002D0 RID: 720
		public byte this[int x, int z]
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

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x00011A05 File Offset: 0x0000FC05
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00006B8B File Offset: 0x00004D8B
		public ByteGrid()
		{
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00011A0F File Offset: 0x0000FC0F
		public ByteGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00011A1E File Offset: 0x0000FC1E
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x000B6BC0 File Offset: 0x000B4DC0
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new byte[this.mapSizeX * this.mapSizeZ];
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00011A48 File Offset: 0x0000FC48
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.ByteArray(ref this.grid, "grid");
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000B6C20 File Offset: 0x000B4E20
		public void Clear(byte value = 0)
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

		// Token: 0x06000F83 RID: 3971 RVA: 0x000B6C64 File Offset: 0x000B4E64
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				byte b = this.grid[i];
				if (b > 0)
				{
					CellRenderer.RenderCell(CellIndicesUtility.IndexToCell(i, this.mapSizeX), (float)b / 255f * 0.5f);
				}
			}
		}

		// Token: 0x04000CAF RID: 3247
		private byte[] grid;

		// Token: 0x04000CB0 RID: 3248
		private int mapSizeX;

		// Token: 0x04000CB1 RID: 3249
		private int mapSizeZ;
	}
}
