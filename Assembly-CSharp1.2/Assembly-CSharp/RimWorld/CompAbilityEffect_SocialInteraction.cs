using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138C RID: 5004
	public class CompAbilityEffect_SocialInteraction : CompAbilityEffect
	{
		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x06006C9D RID: 27805 RVA: 0x00049DF7 File Offset: 0x00047FF7
		public new CompProperties_AbilitySocialInteraction Props
		{
			get
			{
				return (CompProperties_AbilitySocialInteraction)this.props;
			}
		}

		// Token: 0x06006C9E RID: 27806 RVA: 0x00215C30 File Offset: 0x00213E30
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null && this.parent.pawn != pawn)
			{
				Pawn_InteractionsTracker interactions = this.parent.pawn.interactions;
				if (interactions == null)
				{
					return;
				}
				interactions.TryInteractWith(pawn, this.Props.interactionDef);
			}
		}

		// Token: 0x06006C9F RID: 27807 RVA: 0x00049BFB File Offset: 0x00047DFB
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06006CA0 RID: 27808 RVA: 0x00215C88 File Offset: 0x00213E88
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!this.Props.canApplyToMentallyBroken && !AbilityUtility.ValidateNoMentalState(pawn, throwMessages))
				{
					return false;
				}
				if (!AbilityUtility.ValidateIsAwake(pawn, throwMessages))
				{
					return false;
				}
				if (!AbilityUtility.ValidateIsConscious(pawn, throwMessages))
				{
					return false;
				}
			}
			return true;
		}
	}
}
