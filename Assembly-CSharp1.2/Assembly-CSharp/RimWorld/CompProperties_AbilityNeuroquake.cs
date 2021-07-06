using System;

namespace RimWorld
{
	// Token: 0x02001383 RID: 4995
	public class CompProperties_AbilityNeuroquake : CompProperties_AbilityEffect
	{
		// Token: 0x06006C87 RID: 27783 RVA: 0x00049CD1 File Offset: 0x00047ED1
		public CompProperties_AbilityNeuroquake()
		{
			this.compClass = typeof(CompAbilityEffect_Neuroquake);
		}

		// Token: 0x040047FA RID: 18426
		public int goodwillImpactForNeuroquake;

		// Token: 0x040047FB RID: 18427
		public int goodwillImpactForBerserk;

		// Token: 0x040047FC RID: 18428
		public int worldRangeTiles;
	}
}
