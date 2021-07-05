using System;

namespace RimWorld
{
	// Token: 0x02000D60 RID: 3424
	public class CompProperties_AbilitySocialInteraction : CompProperties_AbilityEffect
	{
		// Token: 0x06004FA1 RID: 20385 RVA: 0x001AA960 File Offset: 0x001A8B60
		public CompProperties_AbilitySocialInteraction()
		{
			this.compClass = typeof(CompAbilityEffect_SocialInteraction);
		}

		// Token: 0x04002FBE RID: 12222
		public InteractionDef interactionDef;

		// Token: 0x04002FBF RID: 12223
		public bool canApplyToMentallyBroken;

		// Token: 0x04002FC0 RID: 12224
		public bool canApplyToUnconscious;
	}
}
