using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D5A RID: 3418
	public class CompProperties_AbilityRemoveHediff : CompProperties_AbilityEffect
	{
		// Token: 0x06004F93 RID: 20371 RVA: 0x001AA75F File Offset: 0x001A895F
		public CompProperties_AbilityRemoveHediff()
		{
			this.compClass = typeof(CompAbilityEffect_RemoveHediff);
		}

		// Token: 0x04002FBA RID: 12218
		public HediffDef hediffDef;

		// Token: 0x04002FBB RID: 12219
		public bool applyToSelf;

		// Token: 0x04002FBC RID: 12220
		public bool applyToTarget;
	}
}
