using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D71 RID: 3441
	public class CompAbilityEffect_StopMentalState : CompAbilityEffect
	{
		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06004FD1 RID: 20433 RVA: 0x001AB354 File Offset: 0x001A9554
		public new CompProperties_AbilityStopMentalState Props
		{
			get
			{
				return (CompProperties_AbilityStopMentalState)this.props;
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06004FD2 RID: 20434 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x001AB361 File Offset: 0x001A9561
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

		// Token: 0x06004FD4 RID: 20436 RVA: 0x001A9452 File Offset: 0x001A7652
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x001AB384 File Offset: 0x001A9584
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

		// Token: 0x06004FD6 RID: 20438 RVA: 0x001AB4D8 File Offset: 0x001A96D8
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

		// Token: 0x06004FD7 RID: 20439 RVA: 0x001AB530 File Offset: 0x001A9730
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

		// Token: 0x06004FD8 RID: 20440 RVA: 0x001AB584 File Offset: 0x001A9784
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (target.Pawn != null && this.Valid(target, false))
			{
				return "AbilityPsyfocusCost".Translate() + ": " + this.PsyfocusCostForTarget(target).ToStringPercent("0.#");
			}
			return null;
		}
	}
}
