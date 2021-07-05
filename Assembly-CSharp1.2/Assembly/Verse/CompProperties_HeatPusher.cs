using System;

namespace Verse
{
	// Token: 0x020000FC RID: 252
	public class CompProperties_HeatPusher : CompProperties
	{
		// Token: 0x06000732 RID: 1842 RVA: 0x0000BD44 File Offset: 0x00009F44
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}

		// Token: 0x04000434 RID: 1076
		public float heatPerSecond;

		// Token: 0x04000435 RID: 1077
		public float heatPushMaxTemperature = 99999f;

		// Token: 0x04000436 RID: 1078
		public float heatPushMinTemperature = -99999f;
	}
}
