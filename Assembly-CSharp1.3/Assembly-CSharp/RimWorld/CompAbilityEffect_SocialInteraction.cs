using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D61 RID: 3425
	public class CompAbilityEffect_SocialInteraction : CompAbilityEffect
	{
		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06004FA2 RID: 20386 RVA: 0x001AA978 File Offset: 0x001A8B78
		public new CompProperties_AbilitySocialInteraction Props
		{
			get
			{
				return (CompProperties_AbilitySocialInteraction)this.props;
			}
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x001AA988 File Offset: 0x001A8B88
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

		// Token: 0x06004FA4 RID: 20388 RVA: 0x001A9452 File Offset: 0x001A7652
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x001AA9E0 File Offset: 0x001A8BE0
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
				if (!this.Props.canApplyToUnconscious && !AbilityUtility.ValidateIsConscious(pawn, throwMessages))
				{
					return false;
				}
			}
			return true;
		}
	}
}
