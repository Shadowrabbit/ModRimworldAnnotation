using System;

namespace Verse
{
	// Token: 0x020002B7 RID: 695
	public class HediffCompProperties_SkillDecay : HediffCompProperties
	{
		// Token: 0x060012D2 RID: 4818 RVA: 0x0006BAA5 File Offset: 0x00069CA5
		public HediffCompProperties_SkillDecay()
		{
			this.compClass = typeof(HediffComp_SkillDecay);
		}

		// Token: 0x04000E2D RID: 3629
		public SimpleCurve decayPerDayPercentageLevelCurve;
	}
}
