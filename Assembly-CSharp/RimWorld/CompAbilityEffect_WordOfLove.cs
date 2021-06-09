using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A8 RID: 5032
	public class CompAbilityEffect_WordOfLove : CompAbilityEffect_WithDest
	{
		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x06006D1E RID: 27934 RVA: 0x0004A36E File Offset: 0x0004856E
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

		// Token: 0x06006D1F RID: 27935 RVA: 0x00217410 File Offset: 0x00215610
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
				float num = this.parent.def.EffectDuration;
				num *= pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);
				hediffComp_Disappears.ticksToDisappear = num.SecondsToTicks();
			}
			pawn.health.AddHediff(hediff_PsychicLove, null, null, null);
		}

		// Token: 0x06006D20 RID: 27936 RVA: 0x00049BFB File Offset: 0x00047DFB
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06006D21 RID: 27937 RVA: 0x002174D4 File Offset: 0x002156D4
		public override bool ValidateTarget(LocalTargetInfo target)
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
			return base.ValidateTarget(target);
		}

		// Token: 0x06006D22 RID: 27938 RVA: 0x00217580 File Offset: 0x00215780
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

		// Token: 0x06006D23 RID: 27939 RVA: 0x0004A391 File Offset: 0x00048591
		public override string ExtraLabel(LocalTargetInfo target)
		{
			if (this.selectedTarget.IsValid)
			{
				return "PsychicLoveFor".Translate();
			}
			return "PsychicLoveInduceIn".Translate();
		}
	}
}
