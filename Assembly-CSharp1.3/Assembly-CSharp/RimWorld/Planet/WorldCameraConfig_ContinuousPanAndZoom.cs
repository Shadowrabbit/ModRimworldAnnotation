using System;

namespace RimWorld.Planet
{
	// Token: 0x0200174D RID: 5965
	public class WorldCameraConfig_ContinuousPanAndZoom : WorldCameraConfig_ContinuousPan
	{
		// Token: 0x060089BC RID: 35260 RVA: 0x0031755E File Offset: 0x0031575E
		public WorldCameraConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
