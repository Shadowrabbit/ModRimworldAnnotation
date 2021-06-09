using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F03 RID: 3843
	public class CompProperties_CausesGameCondition_ClimateAdjuster : CompProperties_CausesGameCondition
	{
		// Token: 0x0600550A RID: 21770 RVA: 0x0003AFA5 File Offset: 0x000391A5
		public CompProperties_CausesGameCondition_ClimateAdjuster()
		{
			this.compClass = typeof(CompCauseGameCondition_TemperatureOffset);
		}

		// Token: 0x04003611 RID: 13841
		public FloatRange temperatureOffsetRange = new FloatRange(-10f, 10f);
	}
}
