using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F8 RID: 2552
	public class CompProperties_CausesGameCondition_ClimateAdjuster : CompProperties_CausesGameCondition
	{
		// Token: 0x06003ECC RID: 16076 RVA: 0x001574B1 File Offset: 0x001556B1
		public CompProperties_CausesGameCondition_ClimateAdjuster()
		{
			this.compClass = typeof(CompCauseGameCondition_TemperatureOffset);
		}

		// Token: 0x0400219E RID: 8606
		public FloatRange temperatureOffsetRange = new FloatRange(-10f, 10f);
	}
}
