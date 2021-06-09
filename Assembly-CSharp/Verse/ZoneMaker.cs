using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200032A RID: 810
	public static class ZoneMaker
	{
		// Token: 0x060014A7 RID: 5287 RVA: 0x000CF66C File Offset: 0x000CD86C
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
