using System;

namespace RimWorld
{
	// Token: 0x02000D68 RID: 3432
	public class CompProperties_AbilityStartRitual : CompProperties_AbilityEffect
	{
		// Token: 0x06004FB4 RID: 20404 RVA: 0x001AAD6F File Offset: 0x001A8F6F
		public CompProperties_AbilityStartRitual()
		{
			this.compClass = typeof(CompAbilityEffect_StartRitual);
		}

		// Token: 0x04002FC3 RID: 12227
		public PreceptDef ritualDef;
	}
}
