using System;

namespace Verse
{
	// Token: 0x020002BF RID: 703
	public class HediffCompProperties_SeverityPerDay : HediffCompProperties
	{
		// Token: 0x06001306 RID: 4870 RVA: 0x0006C701 File Offset: 0x0006A901
		public HediffCompProperties_SeverityPerDay()
		{
			this.compClass = typeof(HediffComp_SeverityPerDay);
		}

		// Token: 0x04000E49 RID: 3657
		public float severityPerDay;

		// Token: 0x04000E4A RID: 3658
		public bool showDaysToRecover;

		// Token: 0x04000E4B RID: 3659
		public bool showHoursToRecover;
	}
}
