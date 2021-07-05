using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A10 RID: 2576
	public class CompProperties_TempControl : CompProperties
	{
		// Token: 0x06003F09 RID: 16137 RVA: 0x00157F28 File Offset: 0x00156128
		public CompProperties_TempControl()
		{
			this.compClass = typeof(CompTempControl);
		}

		// Token: 0x0400220B RID: 8715
		public float energyPerSecond = 12f;

		// Token: 0x0400220C RID: 8716
		public float defaultTargetTemperature = 21f;

		// Token: 0x0400220D RID: 8717
		public float minTargetTemperature = -50f;

		// Token: 0x0400220E RID: 8718
		public float maxTargetTemperature = 50f;

		// Token: 0x0400220F RID: 8719
		public float lowPowerConsumptionFactor = 0.1f;
	}
}
