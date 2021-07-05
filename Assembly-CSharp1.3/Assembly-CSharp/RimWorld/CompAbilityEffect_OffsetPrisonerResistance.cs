using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D4D RID: 3405
	public class CompAbilityEffect_OffsetPrisonerResistance : CompAbilityEffect
	{
		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06004F71 RID: 20337 RVA: 0x001AA1DC File Offset: 0x001A83DC
		public new CompProperties_AbilityOffsetPrisonerResistance Props
		{
			get
			{
				return (CompProperties_AbilityOffsetPrisonerResistance)this.props;
			}
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x001AA1EC File Offset: 0x001A83EC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				pawn.guest.resistance = Mathf.Max(pawn.guest.resistance + this.Props.offset, 0f);
			}
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x001AA238 File Offset: 0x001A8438
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

		// Token: 0x06004F74 RID: 20340 RVA: 0x001AA284 File Offset: 0x001A8484
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn == null || AbilityUtility.ValidateHasResistance(pawn, throwMessages);
		}
	}
}
