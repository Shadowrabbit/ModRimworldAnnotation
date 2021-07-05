using System;

namespace Verse
{
	// Token: 0x020007C2 RID: 1986
	public class CellIndices
	{
		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x00027446 File Offset: 0x00025646
		public int NumGridCells
		{
			get
			{
				return this.mapSizeX * this.mapSizeZ;
			}
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x00027455 File Offset: 0x00025655
		public CellIndices(Map map)
		{
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x0002747F File Offset: 0x0002567F
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.mapSizeX);
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x0002748D File Offset: 0x0002568D
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x0002749C File Offset: 0x0002569C
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.mapSizeX);
		}

		// Token: 0x0400227F RID: 8831
		private int mapSizeX;

		// Token: 0x04002280 RID: 8832
		private int mapSizeZ;
	}
}
