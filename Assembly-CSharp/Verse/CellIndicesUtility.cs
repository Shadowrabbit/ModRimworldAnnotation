using System;

namespace Verse
{
	// Token: 0x020007C3 RID: 1987
	public static class CellIndicesUtility
	{
		// Token: 0x06003208 RID: 12808 RVA: 0x000274AA File Offset: 0x000256AA
		public static int CellToIndex(IntVec3 c, int mapSizeX)
		{
			return c.z * mapSizeX + c.x;
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x000274BB File Offset: 0x000256BB
		public static int CellToIndex(int x, int z, int mapSizeX)
		{
			return z * mapSizeX + x;
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x0014BA40 File Offset: 0x00149C40
		public static IntVec3 IndexToCell(int ind, int mapSizeX)
		{
			int newX = ind % mapSizeX;
			int newZ = ind / mapSizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}
