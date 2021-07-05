using System;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D27 RID: 3367
	public class CompAbilityEffect_Convert : CompAbilityEffect
	{
		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06004EFC RID: 20220 RVA: 0x001A73B0 File Offset: 0x001A55B0
		public new CompProperties_AbilityConvert Props
		{
			get
			{
				return (CompProperties_AbilityConvert)this.props;
			}
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06004EFD RID: 20221 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x001A73C0 File Offset: 0x001A55C0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckIdeology("Ideoligion conversion"))
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			float certaintyReduction = InteractionWorker_ConvertIdeoAttempt.CertaintyReduction(pawn, pawn2) * this.Props.convertPowerFactor;
			float certainty = pawn2.ideo.Certainty;
			if (pawn2.ideo.IdeoConversionAttempt(certaintyReduction, pawn.Ideo))
			{
				pawn2.ideo.SetIdeo(this.parent.pawn.Ideo);
				Messages.Message(this.Props.successMessage.Formatted(pawn.Named("INITIATOR"), pawn2.Named("RECIPIENT"), pawn.Ideo.name.Named("IDEO")), new LookTargets(new Pawn[]
				{
					pawn,
					pawn2
				}), MessageTypeDefOf.PositiveEvent, true);
				PlayLogEntry_Interaction entry = new PlayLogEntry_Interaction(InteractionDefOf.Convert_Success, this.parent.pawn, pawn2, null);
				Find.PlayLog.Add(entry);
			}
			else
			{
				pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.failedThoughtInitiator, pawn2, null);
				pawn2.needs.mood.thoughts.memories.TryGainMemory(this.Props.failedThoughtRecipient, pawn, null);
				Messages.Message(this.Props.failMessage.Formatted(pawn.Named("INITIATOR"), pawn2.Named("RECIPIENT"), pawn.Ideo.name.Named("IDEO"), certainty.ToStringPercent().Named("CERTAINTYBEFORE"), pawn2.ideo.Certainty.ToStringPercent().Named("CERTAINTYAFTER")), new LookTargets(new Pawn[]
				{
					pawn,
					pawn2
				}), MessageTypeDefOf.NeutralEvent, true);
				PlayLogEntry_Interaction entry2 = new PlayLogEntry_Interaction(InteractionDefOf.Convert_Failure, this.parent.pawn, pawn2, null);
				Find.PlayLog.Add(entry2);
			}
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x001A7604 File Offset: 0x001A5804
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeHuman(pawn, throwMessages) && AbilityUtility.ValidateNoMentalState(pawn, throwMessages) && AbilityUtility.ValidateIsAwake(pawn, throwMessages) && AbilityUtility.ValidateNotSameIdeo(this.parent.pawn, pawn, throwMessages);
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x001A7658 File Offset: 0x001A5858
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (target.Pawn == null || !this.Valid(target, false))
			{
				return null;
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("AbilityIdeoConvertBreakdownLabel".Translate().CapitalizeFirst() + ": " + (InteractionWorker_ConvertIdeoAttempt.CertaintyReduction(pawn, pawn2) * this.Props.convertPowerFactor).ToStringPercent());
			stringBuilder.AppendLine();
			stringBuilder.AppendInNewLine("Factors".Translate() + ":");
			stringBuilder.AppendInNewLine(" -  " + "Base".Translate().CapitalizeFirst() + ": " + 0.06f.ToStringPercent());
			stringBuilder.AppendInNewLine(" -  " + "AbilityIdeoConvertBreakdownUsingAbility".Translate(this.parent.def.LabelCap.Named("ABILITY")) + ": " + this.Props.convertPowerFactor.ToStringPercent());
			float statValue = pawn.GetStatValue(StatDefOf.ConversionPower, true);
			if (statValue != 1f)
			{
				stringBuilder.AppendInNewLine(" -  " + "AbilityIdeoConvertBreakdownConversionPower".Translate(pawn.Named("PAWN")) + ": " + statValue.ToStringPercent());
			}
			stringBuilder.Append(ConversionUtility.GetCertaintyReductionFactorsDescription(pawn2));
			Precept_Role role = pawn2.Ideo.GetRole(pawn2);
			if (role != null && role.def.certaintyLossFactor != 1f)
			{
				stringBuilder.AppendInNewLine(" -  " + "AbilityIdeoConvertBreakdownRole".Translate(pawn2.Named("PAWN"), role.Named("ROLE")) + ": " + role.def.certaintyLossFactor.ToStringPercent());
			}
			ReliquaryUtility.GetRelicConvertPowerFactorForPawn(pawn, stringBuilder);
			return stringBuilder.ToString();
		}
	}
}
