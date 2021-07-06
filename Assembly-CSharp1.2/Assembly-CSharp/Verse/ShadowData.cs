using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A3 RID: 675
	public class ShadowData
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x00012A81 File Offset: 0x00010C81
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x00012A8E File Offset: 0x00010C8E
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x00012A9B File Offset: 0x00010C9B
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}

		// Token: 0x04000E16 RID: 3606
		public Vector3 volume = Vector3.one;

		// Token: 0x04000E17 RID: 3607
		public Vector3 offset = Vector3.zero;
	}
}
