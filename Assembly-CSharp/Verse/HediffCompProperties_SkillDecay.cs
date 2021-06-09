using System;

namespace Verse
{
	// Token: 0x020003F2 RID: 1010
	public class HediffCompProperties_SkillDecay : HediffCompProperties
	{
		// Token: 0x06001897 RID: 6295 RVA: 0x0001756D File Offset: 0x0001576D
		public HediffCompProperties_SkillDecay()
		{
			this.compClass = typeof(HediffComp_SkillDecay);
		}

		// Token: 0x04001294 RID: 4756
		public SimpleCurve decayPerDayPercentageLevelCurve;
	}
}
