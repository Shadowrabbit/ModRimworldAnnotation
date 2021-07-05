using System;

namespace RimWorld
{
	// Token: 0x02001389 RID: 5001
	public class CompProperties_AbilitySmokepop : CompProperties_AbilityEffect
	{
		// Token: 0x06006C97 RID: 27799 RVA: 0x00049DA1 File Offset: 0x00047FA1
		public CompProperties_AbilitySmokepop()
		{
			this.compClass = typeof(CompAbilityEffect_Smokepop);
		}

		// Token: 0x04004800 RID: 18432
		public float smokeRadius;
	}
}
