using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F20 RID: 3872
	public class CompProperties_TempControl : CompProperties
	{
		// Token: 0x06005589 RID: 21897 RVA: 0x001C8790 File Offset: 0x001C6990
		public CompProperties_TempControl()
		{
			this.compClass = typeof(CompTempControl);
		}

		// Token: 0x040036AC RID: 13996
		public float energyPerSecond = 12f;

		// Token: 0x040036AD RID: 13997
		public float defaultTargetTemperature = 21f;

		// Token: 0x040036AE RID: 13998
		public float minTargetTemperature = -50f;

		// Token: 0x040036AF RID: 13999
		public float maxTargetTemperature = 50f;

		// Token: 0x040036B0 RID: 14000
		public float lowPowerConsumptionFactor = 0.1f;
	}
}
