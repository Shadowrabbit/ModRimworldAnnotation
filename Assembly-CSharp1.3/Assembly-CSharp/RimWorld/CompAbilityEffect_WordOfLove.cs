using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D83 RID: 3459
	public class CompAbilityEffect_WordOfLove : CompAbilityEffect_WithDest
	{
		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x0600501D RID: 20509 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x0600501E RID: 20510 RVA: 0x001AC701 File Offset: 0x001AA901
		public override TargetingParameters targetParams
		{
			get
			{
				return new TargetingParameters
				{
					canTargetSelf = true,
					canTargetBuildings = false,
					canTargetAnimals = false,
					canTargetMechs = false
				};
			}
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x001AC724 File Offset: 0x001AA924
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff_PsychicLove hediff_PsychicLove = (Hediff_PsychicLove)HediffMaker.MakeHediff(HediffDefOf.PsychicLove, pawn, pawn.health.hediffSet.GetBrain());
			hediff_PsychicLove.target = dest.Thing;
			HediffComp_Disappears hediffComp_Disappears = hediff_PsychicLove.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null)
			{
				float num = this.parent.def.EffectDuration(this.parent.pawn);
				num *= pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);
				hediffComp_Disappears.ticksToDisappear = num.SecondsToTicks();
			}
			pawn.health.AddHediff(hediff_PsychicLove, null, null, null);
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x001A9452 File Offset: 0x001A7652
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x001AC7F4 File Offset: 0x001AA9F4
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = this.selectedTarget.Pawn;
			Pawn pawn2 = target.Pawn;
			if (pawn == pawn2)
			{
				return false;
			}
			if (pawn != null && pawn2 != null && !pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
			{
				Gender gender = pawn.gender;
				Gender gender2 = pawn.story.traits.HasTrait(TraitDefOf.Gay) ? gender : gender.Opposite();
				if (pawn2.gender != gender2)
				{
					Messages.Message("AbilityCantApplyWrongAttractionGender".Translate(pawn, pawn2), pawn, MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			return base.ValidateTarget(target, true);
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x001AC8A0 File Offset: 0x001AAAA0
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.story.traits.HasTrait(TraitDefOf.Asexual))
				{
					if (throwMessages)
					{
						Messages.Message("AbilityCantApplyOnAsexual".Translate(this.parent.def.label), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
				if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x001AC915 File Offset: 0x001AAB15
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (this.selectedTarget.IsValid)
			{
				return "PsychicLoveFor".Translate();
			}
			return "PsychicLoveInduceIn".Translate();
		}
	}
}
