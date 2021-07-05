using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D2A RID: 3370
	public class CompAbilityEffect_Counsel : CompAbilityEffect
	{
		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06004F04 RID: 20228 RVA: 0x001A7A33 File Offset: 0x001A5C33
		public new CompProperties_AbilityCounsel Props
		{
			get
			{
				return (CompProperties_AbilityCounsel)this.props;
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06004F05 RID: 20229 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x001A7A40 File Offset: 0x001A5C40
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckIdeology("Ideoligion councel"))
			{
				return;
			}
			Pawn pawn = target.Pawn;
			if (Rand.Chance(this.ChanceForPawn(pawn)))
			{
				List<Thought> list = new List<Thought>();
				pawn.needs.mood.thoughts.GetAllMoodThoughts(list);
				Thought_Memory thought_Memory = (Thought_Memory)(from t in list
				where t is Thought_Memory && t.MoodOffset() <= this.Props.minMoodOffset
				select t).MaxByWithFallback((Thought t) => -t.MoodOffset(), null);
				if (thought_Memory != null)
				{
					Thought_Counselled thought_Counselled = (Thought_Counselled)ThoughtMaker.MakeThought(ThoughtDefOf.Counselled);
					thought_Counselled.durationTicksOverride = thought_Memory.DurationTicks - thought_Memory.age;
					thought_Counselled.moodOffsetOverride = -thought_Memory.MoodOffset();
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Counselled, this.parent.pawn);
					Messages.Message(this.Props.successMessage.Formatted(this.parent.pawn.Named("INITIATOR"), pawn.Named("RECIPIENT"), thought_Memory.MoodOffset()), new LookTargets(new Pawn[]
					{
						this.parent.pawn,
						pawn
					}), MessageTypeDefOf.PositiveEvent, false);
				}
				else
				{
					Thought_Memory thought_Memory2 = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.Counselled_MoodBoost);
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory2, this.parent.pawn);
					Messages.Message(this.Props.successMessageNoNegativeThought.Formatted(this.parent.pawn.Named("INITIATOR"), pawn.Named("RECIPIENT"), thought_Memory2.def.stages[0].baseMoodEffect.Named("MOODBONUS")), new LookTargets(new Pawn[]
					{
						this.parent.pawn,
						pawn
					}), MessageTypeDefOf.PositiveEvent, false);
				}
				PlayLogEntry_Interaction entry = new PlayLogEntry_Interaction(InteractionDefOf.Counsel_Success, this.parent.pawn, pawn, null);
				Find.PlayLog.Add(entry);
			}
			else
			{
				pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.failedThoughtRecipient, this.parent.pawn, null);
				PlayLogEntry_Interaction entry2 = new PlayLogEntry_Interaction(InteractionDefOf.Counsel_Failure, this.parent.pawn, pawn, null);
				Find.PlayLog.Add(entry2);
				Messages.Message(this.Props.failMessage.Formatted(this.parent.pawn.Named("INITIATOR"), pawn.Named("RECIPIENT")), new LookTargets(new Pawn[]
				{
					this.parent.pawn,
					pawn
				}), MessageTypeDefOf.NegativeEvent, false);
			}
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x001A7D70 File Offset: 0x001A5F70
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!AbilityUtility.ValidateMustBeHuman(pawn, throwMessages))
			{
				return false;
			}
			if (this.parent.pawn.Ideo != pawn.Ideo)
			{
				if (throwMessages)
				{
					Precept_Role role = this.parent.pawn.Ideo.GetRole(this.parent.pawn);
					Messages.Message("AbilityMustBeSameIdeoCounsel".Translate(role.LabelForPawn(this.parent.pawn), this.parent.pawn.Ideo.memberName), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages))
			{
				return false;
			}
			if (!AbilityUtility.ValidateIsAwake(pawn, throwMessages))
			{
				return false;
			}
			List<Thought> list = new List<Thought>();
			pawn.needs.mood.thoughts.GetAllMoodThoughts(list);
			if (list.Any((Thought t) => t.def == ThoughtDefOf.Counselled))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityCantApplyAlreadyCounselled".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x001A7EB0 File Offset: 0x001A60B0
		public float ChanceForPawn(Pawn pawn)
		{
			return CompAbilityEffect_Counsel.SuccessChanceBySocialSkill.Evaluate((float)this.parent.pawn.skills.GetSkill(SkillDefOf.Social).Level) * CompAbilityEffect_Counsel.SuccessChanceFactorByOpinion.Evaluate((float)pawn.relations.OpinionOf(this.parent.pawn));
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x001A7F0C File Offset: 0x001A610C
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return null;
			}
			if (!this.Valid(target, false))
			{
				return null;
			}
			Pawn pawn2 = this.parent.pawn;
			Pawn pawn3 = pawn;
			string t = this.ChanceForPawn(pawn).ToStringPercent();
			string t2 = CompAbilityEffect_Counsel.SuccessChanceBySocialSkill.Evaluate((float)pawn2.skills.GetSkill(SkillDefOf.Social).Level).ToStringPercent();
			string t3 = CompAbilityEffect_Counsel.SuccessChanceFactorByOpinion.Evaluate((float)pawn3.relations.OpinionOf(pawn2)).ToStringPercent();
			return "chance".Translate().CapitalizeFirst() + ": " + t + "\n\n" + "Factors".Translate() + ":\n" + " -  " + "AbilityIdeoConvertBreakdownSocialSkill".Translate(pawn2.Named("PAWN")) + " " + t2 + "\n" + " -  " + "AbilityIdeoConvertBreakdownOpinion".Translate(pawn2.Named("INITIATOR"), pawn3.Named("RECIPIENT")) + " " + t3;
		}

		// Token: 0x04002F82 RID: 12162
		public static readonly SimpleCurve SuccessChanceBySocialSkill = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.02f),
				true
			},
			{
				new CurvePoint(5f, 0.4f),
				true
			},
			{
				new CurvePoint(10f, 0.7f),
				true
			},
			{
				new CurvePoint(20f, 0.9f),
				true
			}
		};

		// Token: 0x04002F83 RID: 12163
		public static readonly SimpleCurve SuccessChanceFactorByOpinion = new SimpleCurve
		{
			{
				new CurvePoint(-100f, 0.7f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 1.3f),
				true
			}
		};
	}
}
