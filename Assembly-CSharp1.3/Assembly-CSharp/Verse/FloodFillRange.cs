using System;

namespace Verse
{
	// Token: 0x020001F4 RID: 500
	public struct FloodFillRange
	{
		// Token: 0x06000E1B RID: 3611 RVA: 0x0004F57A File Offset: 0x0004D77A
		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.z = y;
		}

		// Token: 0x04000B77 RID: 2935
		public int minX;

		// Token: 0x04000B78 RID: 2936
		public int maxX;

		// Token: 0x04000B79 RID: 2937
		public int z;
	}
}
