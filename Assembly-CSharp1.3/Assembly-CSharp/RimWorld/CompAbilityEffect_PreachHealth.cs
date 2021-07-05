using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D51 RID: 3409
	public class CompAbilityEffect_PreachHealth : CompAbilityEffect
	{
		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06004F7B RID: 20347 RVA: 0x001AA333 File Offset: 0x001A8533
		public new CompProperties_PreachHealth Props
		{
			get
			{
				return (CompProperties_PreachHealth)this.props;
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06004F7C RID: 20348 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x001AA340 File Offset: 0x001A8540
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeHuman(pawn, throwMessages) && AbilityUtility.ValidateNoMentalState(pawn, throwMessages) && AbilityUtility.ValidateSameIdeo(this.parent.pawn, pawn, throwMessages) && AbilityUtility.ValidateSickOrInjured(pawn, throwMessages);
		}
	}
}
