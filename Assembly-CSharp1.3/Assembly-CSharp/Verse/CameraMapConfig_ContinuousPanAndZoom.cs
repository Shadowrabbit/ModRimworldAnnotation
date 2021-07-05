using System;

namespace Verse
{
	// Token: 0x0200006F RID: 111
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x00017BA4 File Offset: 0x00015DA4
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
			this.minSize = 8.2f;
		}
	}
}
