using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200174A RID: 5962
	public abstract class WorldCameraConfig
	{
		// Token: 0x060089B7 RID: 35255 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
		}

		// Token: 0x060089B8 RID: 35256 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x04005767 RID: 22375
		public float dollyRateKeys = 170f;

		// Token: 0x04005768 RID: 22376
		public float dollyRateScreenEdge = 125f;

		// Token: 0x04005769 RID: 22377
		public float camRotationDecayFactor = 0.9f;

		// Token: 0x0400576A RID: 22378
		public float rotationSpeedScale = 0.3f;

		// Token: 0x0400576B RID: 22379
		public float zoomSpeed = 2.6f;

		// Token: 0x0400576C RID: 22380
		public float zoomPreserveFactor;

		// Token: 0x0400576D RID: 22381
		public bool smoothZoom;
	}
}
