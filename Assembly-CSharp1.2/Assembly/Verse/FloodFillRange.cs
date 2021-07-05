using System;

namespace Verse
{
	// Token: 0x020002C1 RID: 705
	public struct FloodFillRange
	{
		// Token: 0x060011D9 RID: 4569 RVA: 0x00012E38 File Offset: 0x00011038
		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.z = y;
		}

		// Token: 0x04000E66 RID: 3686
		public int minX;

		// Token: 0x04000E67 RID: 3687
		public int maxX;

		// Token: 0x04000E68 RID: 3688
		public int z;
	}
}
