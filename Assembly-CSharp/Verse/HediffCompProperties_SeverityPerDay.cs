using System;

namespace Verse
{
	// Token: 0x020003FC RID: 1020
	public class HediffCompProperties_SeverityPerDay : HediffCompProperties
	{
		// Token: 0x060018D7 RID: 6359 RVA: 0x00017A69 File Offset: 0x00015C69
		public HediffCompProperties_SeverityPerDay()
		{
			this.compClass = typeof(HediffComp_SeverityPerDay);
		}

		// Token: 0x040012B9 RID: 4793
		public float severityPerDay;

		// Token: 0x040012BA RID: 4794
		public bool showDaysToRecover;

		// Token: 0x040012BB RID: 4795
		public bool showHoursToRecover;
	}
}
