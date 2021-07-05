using System;

namespace Verse
{
	// Token: 0x020002A0 RID: 672
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		// Token: 0x06001287 RID: 4743 RVA: 0x0006AA9C File Offset: 0x00068C9C
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}

		// Token: 0x04000E04 RID: 3588
		public float severityPerDayGrowing;

		// Token: 0x04000E05 RID: 3589
		public float severityPerDayRemission;

		// Token: 0x04000E06 RID: 3590
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x04000E07 RID: 3591
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);
	}
}
