using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001484 RID: 5252
	public class InteractionWorker_RecruitAttempt : InteractionWorker
	{
		// Token: 0x0600714D RID: 29005 RVA: 0x0022A4CC File Offset: 0x002286CC
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			bool flag = recipient.AnimalOrWildMan();
			float x = (float)((recipient.relations != null) ? recipient.relations.OpinionOf(initiator) : 0);
			bool flag2 = initiator.InspirationDef == InspirationDefOf.Inspired_Recruitment && !flag && recipient.guest.interactionMode != PrisonerInteractionModeDefOf.ReduceResistance;
			if (DebugSettings.instantRecruit)
			{
				recipient.guest.resistance = 0f;
			}
			float resistanceReduce = 0f;
			if (!flag && recipient.guest.resistance > 0f && !flag2)
			{
				float num = 1f;
				num *= initiator.GetStatValue(StatDefOf.NegotiationAbility, true);
				num *= InteractionWorker_RecruitAttempt.ResistanceImpactFactorCurve_Mood.Evaluate((recipient.needs.mood == null) ? 1f : recipient.needs.mood.CurInstantLevelPercentage);
				num *= InteractionWorker_RecruitAttempt.ResistanceImpactFactorCurve_Opinion.Evaluate(x);
				num = Mathf.Min(num, recipient.guest.resistance);
				float resistance = recipient.guest.resistance;
				recipient.guest.resistance = Mathf.Max(0f, recipient.guest.resistance - num);
				resistanceReduce = resistance - recipient.guest.resistance;
				string text = "TextMote_ResistanceReduced".Translate(resistance.ToString("F1"), recipient.guest.resistance.ToString("F1"));
				if (recipient.needs.mood != null && recipient.needs.mood.CurLevelPercentage < 0.4f)
				{
					text += "\n(" + "lowMood".Translate() + ")";
				}
				if (recipient.relations != null && (float)recipient.relations.OpinionOf(initiator) < -0.01f)
				{
					text += "\n(" + "lowOpinion".Translate() + ")";
				}
				MoteMaker.ThrowText((initiator.DrawPos + recipient.DrawPos) / 2f, initiator.Map, text, 8f);
				if (recipient.guest.resistance == 0f)
				{
					TaggedString taggedString = "MessagePrisonerResistanceBroken".Translate(recipient.LabelShort, initiator.LabelShort, initiator.Named("WARDEN"), recipient.Named("PRISONER"));
					if (recipient.guest.interactionMode == PrisonerInteractionModeDefOf.AttemptRecruit)
					{
						taggedString += " " + "MessagePrisonerResistanceBroken_RecruitAttempsWillBegin".Translate();
					}
					Messages.Message(taggedString, recipient, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			else
			{
				float num2;
				if (flag)
				{
					if (initiator.InspirationDef == InspirationDefOf.Inspired_Taming)
					{
						num2 = 1f;
						initiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Taming);
					}
					else
					{
						num2 = initiator.GetStatValue(StatDefOf.TameAnimalChance, true);
						float x2 = recipient.IsWildMan() ? 0.75f : recipient.RaceProps.wildness;
						num2 *= InteractionWorker_RecruitAttempt.TameChanceFactorCurve_Wildness.Evaluate(x2);
						if (recipient.IsPrisonerInPrisonCell())
						{
							num2 *= 0.6f;
						}
						if (initiator.relations.DirectRelationExists(PawnRelationDefOf.Bond, recipient))
						{
							num2 *= 4f;
						}
					}
				}
				else if (flag2 || DebugSettings.instantRecruit)
				{
					num2 = 1f;
				}
				else
				{
					num2 = recipient.RecruitChanceFinalByPawn(initiator);
				}
				if (Rand.Chance(num2))
				{
					if (!flag)
					{
						recipient.guest.ClearLastRecruiterData();
					}
					InteractionWorker_RecruitAttempt.DoRecruit(initiator, recipient, num2, out letterLabel, out letterText, true, false);
					if (!letterLabel.NullOrEmpty())
					{
						letterDef = LetterDefOf.PositiveEvent;
					}
					lookTargets = new LookTargets(new TargetInfo[]
					{
						recipient,
						initiator
					});
					if (flag2)
					{
						initiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Recruitment);
					}
					extraSentencePacks.Add(RulePackDefOf.Sentence_RecruitAttemptAccepted);
				}
				else
				{
					string text2 = flag ? "TextMote_TameFail".Translate(num2.ToStringPercent()) : "TextMote_RecruitFail".Translate(num2.ToStringPercent());
					if (!flag)
					{
						if (recipient.needs.mood != null && recipient.needs.mood.CurLevelPercentage < 0.4f)
						{
							text2 += "\n(" + "lowMood".Translate() + ")";
						}
						if (recipient.relations != null && (float)recipient.relations.OpinionOf(initiator) < -0.01f)
						{
							text2 += "\n(" + "lowOpinion".Translate() + ")";
						}
					}
					MoteMaker.ThrowText((initiator.DrawPos + recipient.DrawPos) / 2f, initiator.Map, text2, 8f);
					recipient.mindState.CheckStartMentalStateBecauseRecruitAttempted(initiator);
					extraSentencePacks.Add(RulePackDefOf.Sentence_RecruitAttemptRejected);
				}
			}
			if (!flag)
			{
				recipient.guest.SetLastRecruiterData(initiator, resistanceReduce);
			}
		}

		// Token: 0x0600714E RID: 29006 RVA: 0x0022AA28 File Offset: 0x00228C28
		public static void DoRecruit(Pawn recruiter, Pawn recruitee, float recruitChance, bool useAudiovisualEffects = true)
		{
			string text;
			string text2;
			InteractionWorker_RecruitAttempt.DoRecruit(recruiter, recruitee, recruitChance, out text, out text2, useAudiovisualEffects, true);
		}

		// Token: 0x0600714F RID: 29007 RVA: 0x0022AA44 File Offset: 0x00228C44
		public static void DoRecruit(Pawn recruiter, Pawn recruitee, float recruitChance, out string letterLabel, out string letter, bool useAudiovisualEffects = true, bool sendLetter = true)
		{
			letterLabel = null;
			letter = null;
			recruitChance = Mathf.Clamp01(recruitChance);
			string value = recruitee.LabelIndefinite();
			if (recruitee.apparel != null && recruitee.apparel.LockedApparel != null)
			{
				List<Apparel> lockedApparel = recruitee.apparel.LockedApparel;
				for (int i = lockedApparel.Count - 1; i >= 0; i--)
				{
					recruitee.apparel.Unlock(lockedApparel[i]);
				}
			}
			if (recruitee.royalty != null)
			{
				foreach (RoyalTitle royalTitle in recruitee.royalty.AllTitlesForReading)
				{
					if (royalTitle.def.replaceOnRecruited != null)
					{
						recruitee.royalty.SetTitle(royalTitle.faction, royalTitle.def.replaceOnRecruited, false, false, false);
					}
				}
			}
			if (recruitee.guest != null)
			{
				recruitee.guest.SetGuestStatus(null, false);
			}
			bool flag = recruitee.Name != null;
			if (recruitee.Faction != recruiter.Faction)
			{
				recruitee.SetFaction(recruiter.Faction, recruiter);
			}
			if (recruitee.RaceProps.Humanlike)
			{
				if (useAudiovisualEffects)
				{
					letterLabel = "LetterLabelMessageRecruitSuccess".Translate() + ": " + recruitee.LabelShortCap;
					if (sendLetter)
					{
						Find.LetterStack.ReceiveLetter(letterLabel, "MessageRecruitSuccess".Translate(recruiter, recruitee, recruitChance.ToStringPercent(), recruiter.Named("RECRUITER"), recruitee.Named("RECRUITEE")), LetterDefOf.PositiveEvent, recruitee, null, null, null, null);
					}
				}
				TaleRecorder.RecordTale(TaleDefOf.Recruited, new object[]
				{
					recruiter,
					recruitee
				});
				recruiter.records.Increment(RecordDefOf.PrisonersRecruited);
				if (recruitee.needs.mood != null)
				{
					recruitee.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RecruitedMe, recruiter);
				}
				QuestUtility.SendQuestTargetSignals(recruitee.questTags, "Recruited", recruitee.Named("SUBJECT"));
			}
			else
			{
				if (useAudiovisualEffects)
				{
					if (!flag)
					{
						Messages.Message("MessageTameAndNameSuccess".Translate(recruiter.LabelShort, value, recruitChance.ToStringPercent(), recruitee.Name.ToStringFull, recruiter.Named("RECRUITER"), recruitee.Named("RECRUITEE")).AdjustedFor(recruitee, "PAWN", true), recruitee, MessageTypeDefOf.PositiveEvent, true);
					}
					else
					{
						Messages.Message("MessageTameSuccess".Translate(recruiter.LabelShort, value, recruitChance.ToStringPercent(), recruiter.Named("RECRUITER")), recruitee, MessageTypeDefOf.PositiveEvent, true);
					}
					if (recruiter.Spawned && recruitee.Spawned)
					{
						MoteMaker.ThrowText((recruiter.DrawPos + recruitee.DrawPos) / 2f, recruiter.Map, "TextMote_TameSuccess".Translate(recruitChance.ToStringPercent()), 8f);
					}
				}
				recruiter.records.Increment(RecordDefOf.AnimalsTamed);
				RelationsUtility.TryDevelopBondRelation(recruiter, recruitee, 0.01f);
				if (Rand.Chance(Mathf.Lerp(0.02f, 1f, recruitee.RaceProps.wildness)) || recruitee.IsWildMan())
				{
					TaleRecorder.RecordTale(TaleDefOf.TamedAnimal, new object[]
					{
						recruiter,
						recruitee
					});
				}
				if (PawnsFinder.AllMapsWorldAndTemporary_Alive.Count((Pawn p) => p.playerSettings != null && p.playerSettings.Master == recruiter) >= 5)
				{
					TaleRecorder.RecordTale(TaleDefOf.IncreasedMenagerie, new object[]
					{
						recruiter,
						recruitee
					});
				}
			}
			if (recruitee.caller != null)
			{
				recruitee.caller.DoCall();
			}
		}

		// Token: 0x04004AC5 RID: 19141
		private const float BaseResistanceReductionPerInteraction = 1f;

		// Token: 0x04004AC6 RID: 19142
		private static readonly SimpleCurve ResistanceImpactFactorCurve_Mood = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.5f),
				true
			}
		};

		// Token: 0x04004AC7 RID: 19143
		private static readonly SimpleCurve ResistanceImpactFactorCurve_Opinion = new SimpleCurve
		{
			{
				new CurvePoint(-100f, 0.5f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 1.5f),
				true
			}
		};

		// Token: 0x04004AC8 RID: 19144
		private const float MaxMoodForWarning = 0.4f;

		// Token: 0x04004AC9 RID: 19145
		private const float MaxOpinionForWarning = -0.01f;

		// Token: 0x04004ACA RID: 19146
		public const float WildmanWildness = 0.75f;

		// Token: 0x04004ACB RID: 19147
		private const float WildmanPrisonerChanceFactor = 0.6f;

		// Token: 0x04004ACC RID: 19148
		private static readonly SimpleCurve TameChanceFactorCurve_Wildness = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(0f, 2f),
				true
			}
		};

		// Token: 0x04004ACD RID: 19149
		private const float TameChanceFactor_Bonded = 4f;

		// Token: 0x04004ACE RID: 19150
		private const float ChanceToDevelopBondRelationOnTamed = 0.01f;

		// Token: 0x04004ACF RID: 19151
		private const int MenagerieTaleThreshold = 5;
	}
}
