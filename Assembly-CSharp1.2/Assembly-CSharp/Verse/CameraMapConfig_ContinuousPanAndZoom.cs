using System;

namespace Verse
{
	// Token: 0x020000BD RID: 189
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x0000B048 File Offset: 0x00009248
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
			this.minSize = 8.2f;
		}
	}
}
