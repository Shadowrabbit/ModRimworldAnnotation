using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001386 RID: 4998
	public class CompAbilityEffect_OffsetPrisonerResistance : CompAbilityEffect
	{
		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x06006C8F RID: 27791 RVA: 0x00049D7C File Offset: 0x00047F7C
		public new CompProperties_AbilityOffsetPrisonerResistance Props
		{
			get
			{
				return (CompProperties_AbilityOffsetPrisonerResistance)this.props;
			}
		}

		// Token: 0x06006C90 RID: 27792 RVA: 0x00215A94 File Offset: 0x00213C94
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				pawn.guest.resistance = Mathf.Max(pawn.guest.resistance + this.Props.offset, 0f);
			}
		}

		// Token: 0x06006C91 RID: 27793 RVA: 0x00215AE0 File Offset: 0x00213CE0
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!pawn.IsPrisonerOfColony)
				{
					return false;
				}
				if (pawn != null && pawn.guest.resistance < 1E-45f)
				{
					return false;
				}
				if (pawn.Downed)
				{
					return false;
				}
			}
			return this.Valid(target, false);
		}

		// Token: 0x06006C92 RID: 27794 RVA: 0x00215B2C File Offset: 0x00213D2C
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn == null || AbilityUtility.ValidateHasResistance(pawn, throwMessages);
		}
	}
}
