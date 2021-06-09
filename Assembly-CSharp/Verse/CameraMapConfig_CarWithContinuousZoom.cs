using System;

namespace Verse
{
	// Token: 0x020000BF RID: 191
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x060005E6 RID: 1510 RVA: 0x0000B0AC File Offset: 0x000092AC
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
