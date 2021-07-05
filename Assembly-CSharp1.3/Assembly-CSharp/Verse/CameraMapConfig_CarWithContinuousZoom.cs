using System;

namespace Verse
{
	// Token: 0x02000071 RID: 113
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x06000478 RID: 1144 RVA: 0x00017D06 File Offset: 0x00015F06
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
