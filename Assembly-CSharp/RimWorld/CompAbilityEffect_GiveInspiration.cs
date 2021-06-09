using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137D RID: 4989
	public class CompAbilityEffect_GiveInspiration : CompAbilityEffect
	{
		// Token: 0x06006C6D RID: 27757 RVA: 0x002151F4 File Offset: 0x002133F4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				InspirationDef randomAvailableInspirationDef = pawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
				if (randomAvailableInspirationDef != null)
				{
					base.Apply(target, dest);
					pawn.mindState.inspirationHandler.TryStartInspiration_NewTemp(randomAvailableInspirationDef, "LetterPsychicInspiration".Translate(pawn.Named("PAWN"), this.parent.pawn.Named("CASTER")));
				}
			}
		}

		// Token: 0x06006C6E RID: 27758 RVA: 0x00049BFB File Offset: 0x00047DFB
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06006C6F RID: 27759 RVA: 0x0021526C File Offset: 0x0021346C
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!AbilityUtility.ValidateNoInspiration(pawn, throwMessages))
				{
					return false;
				}
				if (!AbilityUtility.ValidateCanGetInspiration(pawn, throwMessages))
				{
					return false;
				}
			}
			return base.Valid(target, throwMessages);
		}
	}
}
