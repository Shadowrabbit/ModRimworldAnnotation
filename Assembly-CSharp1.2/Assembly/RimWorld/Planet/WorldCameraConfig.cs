using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002037 RID: 8247
	public abstract class WorldCameraConfig
	{
		// Token: 0x0600AEE0 RID: 44768 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
		}

		// Token: 0x0600AEE1 RID: 44769 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x04007814 RID: 30740
		public float dollyRateKeys = 170f;

		// Token: 0x04007815 RID: 30741
		public float dollyRateScreenEdge = 125f;

		// Token: 0x04007816 RID: 30742
		public float camRotationDecayFactor = 0.9f;

		// Token: 0x04007817 RID: 30743
		public float rotationSpeedScale = 0.3f;

		// Token: 0x04007818 RID: 30744
		public float zoomSpeed = 2.6f;

		// Token: 0x04007819 RID: 30745
		public float zoomPreserveFactor;

		// Token: 0x0400781A RID: 30746
		public bool smoothZoom;
	}
}
