using System;

namespace Verse
{
	// Token: 0x020003DC RID: 988
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		// Token: 0x0600184F RID: 6223 RVA: 0x000DEECC File Offset: 0x000DD0CC
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}

		// Token: 0x0400126C RID: 4716
		public float severityPerDayGrowing;

		// Token: 0x0400126D RID: 4717
		public float severityPerDayRemission;

		// Token: 0x0400126E RID: 4718
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x0400126F RID: 4719
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);
	}
}
