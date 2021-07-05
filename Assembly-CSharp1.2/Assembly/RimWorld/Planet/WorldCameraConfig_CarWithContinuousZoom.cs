using System;

namespace RimWorld.Planet
{
	// Token: 0x0200203C RID: 8252
	public class WorldCameraConfig_CarWithContinuousZoom : WorldCameraConfig_Car
	{
		// Token: 0x0600AEE8 RID: 44776 RVA: 0x00071E0C File Offset: 0x0007000C
		public WorldCameraConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
