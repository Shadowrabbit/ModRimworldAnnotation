using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000D42 RID: 3394
	public class CompProperties_AbilityGiveRandomHediff : CompProperties_AbilityEffect
	{
		// Token: 0x06004F50 RID: 20304 RVA: 0x001A95D7 File Offset: 0x001A77D7
		public CompProperties_AbilityGiveRandomHediff()
		{
			this.compClass = typeof(CompAbilityEffect_GiveRandomHediff);
		}

		// Token: 0x04002F9F RID: 12191
		public List<HediffOption> options;

		// Token: 0x04002FA0 RID: 12192
		public bool allowDuplicates;
	}
}
