using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001876 RID: 6262
	public class CompProperties_TemperatureRuinable : CompProperties
	{
		// Token: 0x06008AE3 RID: 35555 RVA: 0x0005D244 File Offset: 0x0005B444
		public CompProperties_TemperatureRuinable()
		{
			this.compClass = typeof(CompTemperatureRuinable);
		}

		// Token: 0x04005912 RID: 22802
		public float minSafeTemperature;

		// Token: 0x04005913 RID: 22803
		public float maxSafeTemperature = 100f;

		// Token: 0x04005914 RID: 22804
		public float progressPerDegreePerTick = 1E-05f;
	}
}
