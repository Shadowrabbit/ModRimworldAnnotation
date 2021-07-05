using System;

namespace RimWorld
{
	// Token: 0x02000D5E RID: 3422
	public class CompProperties_AbilitySmokepop : CompProperties_AbilityEffect
	{
		// Token: 0x06004F9C RID: 20380 RVA: 0x001AA8B0 File Offset: 0x001A8AB0
		public CompProperties_AbilitySmokepop()
		{
			this.compClass = typeof(CompAbilityEffect_Smokepop);
		}

		// Token: 0x04002FBD RID: 12221
		public float smokeRadius;
	}
}
