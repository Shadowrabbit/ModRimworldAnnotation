using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001DE RID: 478
	public class ShadowData
	{
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x0004D126 File Offset: 0x0004B326
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x0004D133 File Offset: 0x0004B333
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x0004D140 File Offset: 0x0004B340
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}

		// Token: 0x04000B2D RID: 2861
		public Vector3 volume = Vector3.one;

		// Token: 0x04000B2E RID: 2862
		public Vector3 offset = Vector3.zero;
	}
}
