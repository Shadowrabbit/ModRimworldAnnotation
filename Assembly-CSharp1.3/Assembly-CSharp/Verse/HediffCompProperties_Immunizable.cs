using System;

namespace Verse
{
	// Token: 0x020002BD RID: 701
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		// Token: 0x060012F8 RID: 4856 RVA: 0x0006C473 File Offset: 0x0006A673
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}

		// Token: 0x04000E42 RID: 3650
		public float immunityPerDayNotSick;

		// Token: 0x04000E43 RID: 3651
		public float immunityPerDaySick;

		// Token: 0x04000E44 RID: 3652
		public float severityPerDayNotImmune;

		// Token: 0x04000E45 RID: 3653
		public float severityPerDayImmune;

		// Token: 0x04000E46 RID: 3654
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);
	}
}
