using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200136E RID: 4974
	public class CompProperties_AbilityEffecterOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x06006C34 RID: 27700 RVA: 0x000499D6 File Offset: 0x00047BD6
		public CompProperties_AbilityEffecterOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_EffecterOnTarget);
		}

		// Token: 0x040047D3 RID: 18387
		public EffecterDef effecterDef;

		// Token: 0x040047D4 RID: 18388
		public int maintainForTicks = -1;

		// Token: 0x040047D5 RID: 18389
		public float scale = 1f;
	}
}
