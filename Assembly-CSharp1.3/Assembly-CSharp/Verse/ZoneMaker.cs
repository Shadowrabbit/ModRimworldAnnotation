using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000230 RID: 560
	public static class ZoneMaker
	{
		// Token: 0x06000FEC RID: 4076 RVA: 0x0005A588 File Offset: 0x00058788
		public static Zone MakeZoneWithCells(Zone z, IEnumerable<IntVec3> cells)
		{
			if (cells != null)
			{
				foreach (IntVec3 c in cells)
				{
					z.AddCell(c);
				}
			}
			return z;
		}
	}
}
