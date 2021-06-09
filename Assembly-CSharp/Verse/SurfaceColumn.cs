using System;

namespace Verse
{
	// Token: 0x020007E5 RID: 2021
	public struct SurfaceColumn
	{
		// Token: 0x06003308 RID: 13064 RVA: 0x00027FD2 File Offset: 0x000261D2
		public SurfaceColumn(float x, SimpleCurve y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x0400232B RID: 9003
		public float x;

		// Token: 0x0400232C RID: 9004
		public SimpleCurve y;
	}
}
