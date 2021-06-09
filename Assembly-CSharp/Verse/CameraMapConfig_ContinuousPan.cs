using System;

namespace Verse
{
	// Token: 0x020000BC RID: 188
	public class CameraMapConfig_ContinuousPan : CameraMapConfig
	{
		// Token: 0x060005E2 RID: 1506 RVA: 0x0000B009 File Offset: 0x00009209
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
