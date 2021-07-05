using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D3E RID: 3390
	public class CompAbilityEffect_GiveInspiration : CompAbilityEffect
	{
		// Token: 0x06004F45 RID: 20293 RVA: 0x001A93DC File Offset: 0x001A75DC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				InspirationDef randomAvailableInspirationDef = pawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
				if (randomAvailableInspirationDef != null)
				{
					base.Apply(target, dest);
					pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, "LetterPsychicInspiration".Translate(pawn.Named("PAWN"), this.parent.pawn.Named("CASTER")), true);
				}
			}
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x001A9452 File Offset: 0x001A7652
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x001A945C File Offset: 0x001A765C
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
