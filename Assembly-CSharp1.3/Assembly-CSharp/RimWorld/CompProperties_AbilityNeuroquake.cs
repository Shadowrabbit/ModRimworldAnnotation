using System;

namespace RimWorld
{
	// Token: 0x02000D4A RID: 3402
	public class CompProperties_AbilityNeuroquake : CompProperties_AbilityEffect
	{
		// Token: 0x06004F67 RID: 20327 RVA: 0x001A9AF8 File Offset: 0x001A7CF8
		public CompProperties_AbilityNeuroquake()
		{
			this.compClass = typeof(CompAbilityEffect_Neuroquake);
		}

		// Token: 0x04002FA7 RID: 12199
		public int goodwillImpactForNeuroquake;

		// Token: 0x04002FA8 RID: 12200
		public int goodwillImpactForBerserk;

		// Token: 0x04002FA9 RID: 12201
		public int worldRangeTiles;

		// Token: 0x04002FAA RID: 12202
		public float mentalStateRadius;
	}
}
