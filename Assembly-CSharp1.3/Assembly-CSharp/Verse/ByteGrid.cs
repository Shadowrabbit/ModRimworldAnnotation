using System;

namespace Verse
{
	// Token: 0x020001AC RID: 428
	public sealed class ByteGrid : IExposable
	{
		// Token: 0x17000243 RID: 579
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

		// Token: 0x17000244 RID: 580
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

		// Token: 0x17000245 RID: 581
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

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00040F39 File Offset: 0x0003F139
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x000033AC File Offset: 0x000015AC
		public ByteGrid()
		{
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00040F43 File Offset: 0x0003F143
		public ByteGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00040F52 File Offset: 0x0003F152
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00040F7C File Offset: 0x0003F17C
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

		// Token: 0x06000C03 RID: 3075 RVA: 0x00040FDC File Offset: 0x0003F1DC
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.ByteArray(ref this.grid, "grid");
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00041014 File Offset: 0x0003F214
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

		// Token: 0x06000C05 RID: 3077 RVA: 0x00041058 File Offset: 0x0003F258
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

		// Token: 0x040009EC RID: 2540
		private byte[] grid;

		// Token: 0x040009ED RID: 2541
		private int mapSizeX;

		// Token: 0x040009EE RID: 2542
		private int mapSizeZ;
	}
}
