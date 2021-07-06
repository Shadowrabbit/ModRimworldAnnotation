using System;

namespace RimWorld
{
	// Token: 0x0200138B RID: 5003
	public class CompProperties_AbilitySocialInteraction : CompProperties_AbilityEffect
	{
		// Token: 0x06006C9C RID: 27804 RVA: 0x00049DDF File Offset: 0x00047FDF
		public CompProperties_AbilitySocialInteraction()
		{
			this.compClass = typeof(CompAbilityEffect_SocialInteraction);
		}

		// Token: 0x04004801 RID: 18433
		public InteractionDef interactionDef;

		// Token: 0x04004802 RID: 18434
		public bool canApplyToMentallyBroken;
	}
}
