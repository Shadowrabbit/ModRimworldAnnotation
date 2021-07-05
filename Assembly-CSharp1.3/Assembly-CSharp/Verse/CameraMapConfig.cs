using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200006C RID: 108
	public abstract class CameraMapConfig
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x0400017B RID: 379
		public float dollyRateKeys = 50f;

		// Token: 0x0400017C RID: 380
		public float dollyRateScreenEdge = 35f;

		// Token: 0x0400017D RID: 381
		public float camSpeedDecayFactor = 0.85f;

		// Token: 0x0400017E RID: 382
		public float moveSpeedScale = 2f;

		// Token: 0x0400017F RID: 383
		public float zoomSpeed = 2.6f;

		// Token: 0x04000180 RID: 384
		public float minSize = 11f;

		// Token: 0x04000181 RID: 385
		public float zoomPreserveFactor;

		// Token: 0x04000182 RID: 386
		public bool smoothZoom;
	}
}
