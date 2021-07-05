using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B3 RID: 4531
	public class CompProperties_TemperatureRuinable : CompProperties
	{
		// Token: 0x06006D23 RID: 27939 RVA: 0x00249AA3 File Offset: 0x00247CA3
		public CompProperties_TemperatureRuinable()
		{
			this.compClass = typeof(CompTemperatureRuinable);
		}

		// Token: 0x04003CA8 RID: 15528
		public float minSafeTemperature;

		// Token: 0x04003CA9 RID: 15529
		public float maxSafeTemperature = 100f;

		// Token: 0x04003CAA RID: 15530
		public float progressPerDegreePerTick = 1E-05f;
	}
}
