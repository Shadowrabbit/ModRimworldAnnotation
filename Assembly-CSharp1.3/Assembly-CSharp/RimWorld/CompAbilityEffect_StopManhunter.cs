using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D6F RID: 3439
	public class CompAbilityEffect_StopManhunter : CompAbilityEffect
	{
		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06004FC6 RID: 20422 RVA: 0x001AB0F8 File Offset: 0x001A92F8
		public new CompProperties_StopManhunter Props
		{
			get
			{
				return (CompProperties_StopManhunter)this.props;
			}
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x001AB108 File Offset: 0x001A9308
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckIdeology("Animal calm"))
			{
				return;
			}
			Pawn pawn = target.Pawn;
			pawn.MentalState.RecoverFromState();
			Messages.Message(this.Props.successMessage.Formatted(this.parent.pawn.Named("INITIATOR"), pawn.Named("RECIPIENT")), new LookTargets(new Pawn[]
			{
				this.parent.pawn,
				pawn
			}), MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x001AB194 File Offset: 0x001A9394
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeAnimal(pawn, throwMessages) && AbilityUtility.ValidateIsMaddened(pawn, throwMessages) && AbilityUtility.ValidateIsAwake(pawn, throwMessages);
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x001AB1D0 File Offset: 0x001A93D0
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return AbilityUtility.ValidateIsMaddened(target.Pawn, false) && base.CanApplyOn(target, dest);
		}
	}
}
