using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001393 RID: 5011
	public class CompAbilityEffect_StopMentalState : CompAbilityEffect
	{
		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x06006CBA RID: 27834 RVA: 0x00049F6B File Offset: 0x0004816B
		public new CompProperties_AbilityStopMentalState Props
		{
			get
			{
				return (CompProperties_AbilityStopMentalState)this.props;
			}
		}

		// Token: 0x06006CBB RID: 27835 RVA: 0x00049F78 File Offset: 0x00048178
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			pawn.MentalState.RecoverFromState();
		}

		// Token: 0x06006CBC RID: 27836 RVA: 0x00049BFB File Offset: 0x00047DFB
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06006CBD RID: 27837 RVA: 0x00216174 File Offset: 0x00214374
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!AbilityUtility.ValidateHasMentalState(pawn, throwMessages))
				{
					return false;
				}
				if (this.Props.exceptions.Contains(pawn.MentalStateDef))
				{
					if (throwMessages)
					{
						Messages.Message("AbilityDoesntWorkOnMentalState".Translate(this.parent.def.label, pawn.MentalStateDef.label), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
				float num = this.PsyfocusCostForTarget(target);
				if (num > this.parent.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
				{
					Pawn pawn2 = this.parent.pawn;
					if (throwMessages)
					{
						TaggedString value = ("MentalBreakIntensity" + this.TargetMentalBreakIntensity(target)).Translate();
						Messages.Message("CommandPsycastNotEnoughPsyfocusForMentalBreak".Translate(num.ToStringPercent(), value, pawn2.psychicEntropy.CurrentPsyfocus.ToStringPercent("0.#"), this.parent.def.label.Named("PSYCASTNAME"), pawn2.Named("CASTERNAME")), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006CBE RID: 27838 RVA: 0x002162C8 File Offset: 0x002144C8
		public MentalBreakIntensity TargetMentalBreakIntensity(LocalTargetInfo target)
		{
			Pawn pawn = target.Pawn;
			MentalStateDef mentalStateDef = (pawn != null) ? pawn.MentalStateDef : null;
			if (mentalStateDef != null)
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].mentalState == mentalStateDef)
					{
						return allDefsListForReading[i].intensity;
					}
				}
			}
			return MentalBreakIntensity.Minor;
		}

		// Token: 0x06006CBF RID: 27839 RVA: 0x00216320 File Offset: 0x00214520
		public override float PsyfocusCostForTarget(LocalTargetInfo target)
		{
			switch (this.TargetMentalBreakIntensity(target))
			{
			case MentalBreakIntensity.Minor:
				return this.Props.psyfocusCostForMinor;
			case MentalBreakIntensity.Major:
				return this.Props.psyfocusCostForMajor;
			case MentalBreakIntensity.Extreme:
				return this.Props.psyfocusCostForExtreme;
			default:
				return 0f;
			}
		}

		// Token: 0x06006CC0 RID: 27840 RVA: 0x00216374 File Offset: 0x00214574
		public override string ExtraLabel(LocalTargetInfo target)
		{
			if (target.Pawn != null && this.Valid(target, false))
			{
				return "AbilityPsyfocusCost".Translate() + ": " + this.PsyfocusCostForTarget(target).ToStringPercent("0.#");
			}
			return null;
		}
	}
}
