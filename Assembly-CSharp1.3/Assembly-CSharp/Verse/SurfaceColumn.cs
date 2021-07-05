using System;

namespace Verse
{
	// Token: 0x02000484 RID: 1156
	public struct SurfaceColumn
	{
		// Token: 0x0600233D RID: 9021 RVA: 0x000DD307 File Offset: 0x000DB507
		public SurfaceColumn(float x, SimpleCurve y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x04001601 RID: 5633
		public float x;

		// Token: 0x04001602 RID: 5634
		public SimpleCurve y;
	}
}
