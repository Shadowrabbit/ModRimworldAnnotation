using System;

namespace Verse
{
	// Token: 0x02000469 RID: 1129
	public static class CellIndicesUtility
	{
		// Token: 0x06002241 RID: 8769 RVA: 0x000D9114 File Offset: 0x000D7314
		public static int CellToIndex(IntVec3 c, int mapSizeX)
		{
			return c.z * mapSizeX + c.x;
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x000D9125 File Offset: 0x000D7325
		public static int CellToIndex(int x, int z, int mapSizeX)
		{
			return z * mapSizeX + x;
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000D912C File Offset: 0x000D732C
		public static IntVec3 IndexToCell(int ind, int mapSizeX)
		{
			int newX = ind % mapSizeX;
			int newZ = ind / mapSizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}
