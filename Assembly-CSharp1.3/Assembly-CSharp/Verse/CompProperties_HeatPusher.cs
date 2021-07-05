using System;

namespace Verse
{
	// Token: 0x02000099 RID: 153
	public class CompProperties_HeatPusher : CompProperties
	{
		// Token: 0x0600052D RID: 1325 RVA: 0x0001A8AF File Offset: 0x00018AAF
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}

		// Token: 0x04000260 RID: 608
		public float heatPerSecond;

		// Token: 0x04000261 RID: 609
		public float heatPushMaxTemperature = 99999f;

		// Token: 0x04000262 RID: 610
		public float heatPushMinTemperature = -99999f;
	}
}
