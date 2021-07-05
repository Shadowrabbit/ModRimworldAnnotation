using System;

namespace Verse
{
	// Token: 0x0200006E RID: 110
	public class CameraMapConfig_ContinuousPan : CameraMapConfig
	{
		// Token: 0x06000474 RID: 1140 RVA: 0x00017B65 File Offset: 0x00015D65
		public CameraMapConfig_ContinuousPan()
		{
			this.dollyRateKeys = 10f;
			this.dollyRateScreenEdge = 5f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
			this.minSize = 8.2f;
		}
	}
}
