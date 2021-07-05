using System;

namespace RimWorld.Planet
{
	// Token: 0x0200203A RID: 8250
	public class WorldCameraConfig_ContinuousPanAndZoom : WorldCameraConfig_ContinuousPan
	{
		// Token: 0x0600AEE5 RID: 44773 RVA: 0x00071DB3 File Offset: 0x0006FFB3
		public WorldCameraConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
