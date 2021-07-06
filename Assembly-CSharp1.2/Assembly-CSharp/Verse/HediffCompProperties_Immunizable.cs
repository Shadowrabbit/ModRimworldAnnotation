using System;

namespace Verse
{
	// Token: 0x020003FA RID: 1018
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		// Token: 0x060018CA RID: 6346 RVA: 0x00017941 File Offset: 0x00015B41
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}

		// Token: 0x040012B2 RID: 4786
		public float immunityPerDayNotSick;

		// Token: 0x040012B3 RID: 4787
		public float immunityPerDaySick;

		// Token: 0x040012B4 RID: 4788
		public float severityPerDayNotImmune;

		// Token: 0x040012B5 RID: 4789
		public float severityPerDayImmune;

		// Token: 0x040012B6 RID: 4790
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);
	}
}
