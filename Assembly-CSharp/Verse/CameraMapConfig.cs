using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000BA RID: 186
	public abstract class CameraMapConfig
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x040002F5 RID: 757
		public float dollyRateKeys = 50f;

		// Token: 0x040002F6 RID: 758
		public float dollyRateScreenEdge = 35f;

		// Token: 0x040002F7 RID: 759
		public float camSpeedDecayFactor = 0.85f;

		// Token: 0x040002F8 RID: 760
		public float moveSpeedScale = 2f;

		// Token: 0x040002F9 RID: 761
		public float zoomSpeed = 2.6f;

		// Token: 0x040002FA RID: 762
		public float minSize = 11f;

		// Token: 0x040002FB RID: 763
		public float zoomPreserveFactor;

		// Token: 0x040002FC RID: 764
		public bool smoothZoom;
	}
}
