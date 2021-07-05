using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D58 RID: 3416
	public class CompProperties_AbilityReassure : CompProperties_AbilityEffect
	{
		// Token: 0x06004F8C RID: 20364 RVA: 0x001AA586 File Offset: 0x001A8786
		public CompProperties_AbilityReassure()
		{
			this.compClass = typeof(CompAbilityEffect_Reassure);
		}

		// Token: 0x04002FB8 RID: 12216
		[MustTranslate]
		public string successMessage;

		// Token: 0x04002FB9 RID: 12217
		public float baseCertaintyGain = 0.075f;
	}
}
