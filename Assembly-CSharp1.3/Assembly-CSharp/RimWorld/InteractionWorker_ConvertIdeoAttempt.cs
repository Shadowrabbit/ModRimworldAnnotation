using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF3 RID: 3571
	public class InteractionWorker_ConvertIdeoAttempt : InteractionWorker
	{
		// Token: 0x060052C1 RID: 21185 RVA: 0x001BEB6C File Offset: 0x001BCD6C
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return 0f;
			}
			if (initiator.Ideo == null || !recipient.RaceProps.Humanlike || initiator.Ideo == recipient.Ideo)
			{
				return 0f;
			}
			if (initiator.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				return 0f;
			}
			return 0.04f * initiator.GetStatValue(StatDefOf.SocialIdeoSpreadFrequencyFactor, true) * this.ConversionSelectionFactor(initiator, recipient);
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x001BEBE8 File Offset: 0x001BCDE8
		private float ConversionSelectionFactor(Pawn initiator, Pawn recipient)
		{
			InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory pawnConversionCategory = InteractionWorker_ConvertIdeoAttempt.<ConversionSelectionFactor>g__GetConversionCategory|2_0(initiator);
			InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory pawnConversionCategory2 = InteractionWorker_ConvertIdeoAttempt.<ConversionSelectionFactor>g__GetConversionCategory|2_0(recipient);
			switch (pawnConversionCategory)
			{
			case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Free:
				switch (pawnConversionCategory2)
				{
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Colonist:
					return 0.5f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Free:
					return 0.25f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Prisoner:
					return 0.25f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Slave:
					return 0.5f;
				}
				break;
			case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Prisoner:
				switch (pawnConversionCategory2)
				{
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Colonist:
					return 0.25f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Free:
					return 0.25f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Prisoner:
					return 0.5f;
				case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Slave:
					return 0.5f;
				}
				break;
			case InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Slave:
				return 0.5f;
			}
			return 1f;
		}

		// Token: 0x060052C3 RID: 21187 RVA: 0x001BEC84 File Offset: 0x001BCE84
		public static float CertaintyReduction(Pawn initiator, Pawn recipient)
		{
			float num = 0.06f * initiator.GetStatValue(StatDefOf.ConversionPower, true) * recipient.GetStatValue(StatDefOf.CertaintyLossFactor, true) * ReliquaryUtility.GetRelicConvertPowerFactorForPawn(initiator, null);
			Precept_Role role = recipient.Ideo.GetRole(recipient);
			if (role != null)
			{
				num *= role.def.certaintyLossFactor;
			}
			return num;
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x001BECD8 File Offset: 0x001BCED8
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterLabel = null;
			letterText = null;
			letterDef = null;
			lookTargets = null;
			Ideo ideo = recipient.Ideo;
			Precept_Role role = ideo.GetRole(recipient);
			float certainty = recipient.ideo.Certainty;
			if (recipient.ideo.IdeoConversionAttempt(InteractionWorker_ConvertIdeoAttempt.CertaintyReduction(initiator, recipient), initiator.Ideo))
			{
				if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(recipient))
				{
					letterLabel = "LetterLabelConvertIdeoAttempt_Success".Translate();
					letterText = "LetterConvertIdeoAttempt_Success".Translate(initiator.Named("INITIATOR"), recipient.Named("RECIPIENT"), initiator.Ideo.Named("IDEO"), ideo.Named("OLDIDEO")).Resolve();
					letterDef = LetterDefOf.PositiveEvent;
					lookTargets = new LookTargets(new TargetInfo[]
					{
						initiator,
						recipient
					});
					if (role != null)
					{
						letterText = letterText + "\n\n" + "LetterRoleLostLetterIdeoChangedPostfix".Translate(recipient.Named("PAWN"), role.Named("ROLE"), ideo.Named("OLDIDEO")).Resolve();
					}
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_ConvertIdeoAttemptSuccess);
				return;
			}
			float num = recipient.interactions.SocialFightPossible(initiator) ? 0.04f : 0f;
			float num2 = Rand.Value * (0.96000004f + num);
			if (num2 < 0.6f || recipient.IsPrisoner)
			{
				extraSentencePacks.Add(RulePackDefOf.Sentence_ConvertIdeoAttemptFail);
				return;
			}
			if (num2 < 0.96000004f)
			{
				if (recipient.needs.mood != null)
				{
					if (PawnUtility.ShouldSendNotificationAbout(recipient))
					{
						Messages.Message("MessageFailedConvertIdeoAttempt".Translate(initiator.Named("INITIATOR"), recipient.Named("RECIPIENT"), certainty.ToStringPercent().Named("CERTAINTYBEFORE"), recipient.ideo.Certainty.ToStringPercent().Named("CERTAINTYAFTER")), recipient, MessageTypeDefOf.NeutralEvent, true);
					}
					recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedConvertIdeoAttemptResentment, initiator, null);
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_ConvertIdeoAttemptFailResentment);
				return;
			}
			recipient.interactions.StartSocialFight(initiator, "MessageFailedConvertIdeoAttemptSocialFight");
			extraSentencePacks.Add(RulePackDefOf.Sentence_ConvertIdeoAttemptFailSocialFight);
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x001BEF30 File Offset: 0x001BD130
		[CompilerGenerated]
		internal static InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory <ConversionSelectionFactor>g__GetConversionCategory|2_0(Pawn pawn)
		{
			if (pawn.IsSlave)
			{
				return InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Slave;
			}
			if (pawn.IsColonist || pawn.IsPrisonerOfColony)
			{
				return InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.Colonist;
			}
			if (pawn.IsPrisoner)
			{
				return InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Prisoner;
			}
			return InteractionWorker_ConvertIdeoAttempt.PawnConversionCategory.NPC_Free;
		}

		// Token: 0x0200228A RID: 8842
		private enum PawnConversionCategory
		{
			// Token: 0x040083DD RID: 33757
			Colonist,
			// Token: 0x040083DE RID: 33758
			NPC_Free,
			// Token: 0x040083DF RID: 33759
			NPC_Prisoner,
			// Token: 0x040083E0 RID: 33760
			Slave
		}
	}
}
