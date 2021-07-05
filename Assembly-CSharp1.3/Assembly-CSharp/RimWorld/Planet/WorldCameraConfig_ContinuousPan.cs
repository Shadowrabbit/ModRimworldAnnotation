using System;

namespace RimWorld.Planet
{
	// Token: 0x0200174C RID: 5964
	public class WorldCameraConfig_ContinuousPan : WorldCameraConfig
	{
		// Token: 0x060089BB RID: 35259 RVA: 0x0031752A File Offset: 0x0031572A
		public WorldCameraConfig_ContinuousPan()
		{
			this.dollyRateKeys = 34f;
			this.dollyRateScreenEdge = 17.85f;
			this.camRotationDecayFactor = 1f;
			this.rotationSpeedScale = 0.15f;
		}
	}
}
