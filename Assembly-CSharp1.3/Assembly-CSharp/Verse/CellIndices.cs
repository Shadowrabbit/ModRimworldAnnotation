using System;

namespace Verse
{
	// Token: 0x02000468 RID: 1128
	public class CellIndices
	{
		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x0600223C RID: 8764 RVA: 0x000D90B0 File Offset: 0x000D72B0
		public int NumGridCells
		{
			get
			{
				return this.mapSizeX * this.mapSizeZ;
			}
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000D90BF File Offset: 0x000D72BF
		public CellIndices(Map map)
		{
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x000D90E9 File Offset: 0x000D72E9
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.mapSizeX);
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x000D90F7 File Offset: 0x000D72F7
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000D9106 File Offset: 0x000D7306
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.mapSizeX);
		}

		// Token: 0x04001557 RID: 5463
		private int mapSizeX;

		// Token: 0x04001558 RID: 5464
		private int mapSizeZ;
	}
}
