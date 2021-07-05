using System;

namespace RimWorld.Planet
{
	// Token: 0x0200174F RID: 5967
	public class WorldCameraConfig_CarWithContinuousZoom : WorldCameraConfig_Car
	{
		// Token: 0x060089BF RID: 35263 RVA: 0x003176B6 File Offset: 0x003158B6
		public WorldCameraConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
