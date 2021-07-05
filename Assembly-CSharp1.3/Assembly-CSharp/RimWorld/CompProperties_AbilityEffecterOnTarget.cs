using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2F RID: 3375
	public class CompProperties_AbilityEffecterOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x06004F16 RID: 20246 RVA: 0x001A82B6 File Offset: 0x001A64B6
		public CompProperties_AbilityEffecterOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_EffecterOnTarget);
		}

		// Token: 0x04002F88 RID: 12168
		public EffecterDef effecterDef;

		// Token: 0x04002F89 RID: 12169
		public int maintainForTicks = -1;

		// Token: 0x04002F8A RID: 12170
		public float scale = 1f;
	}
}
