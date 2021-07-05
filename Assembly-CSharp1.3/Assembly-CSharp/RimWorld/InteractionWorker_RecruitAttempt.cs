﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFA RID: 3578
	public class InteractionWorker_RecruitAttempt : InteractionWorker
	{
		// Token: 0x060052D6 RID: 21206 RVA: 0x001BFA38 File Offset: 0x001BDC38
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
			if (!flag)
			{
				if (recipient.guest.resistance > 0f && !flag2)
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
					recipient.guest.ClearLastRecruiterData();
					InteractionWorker_RecruitAttempt.DoRecruit(initiator, recipient, out letterLabel, out letterText, true, false);
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
				recipient.guest.SetLastRecruiterData(initiator, resistanceReduce);
				return;
			}
			float num2;
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
				if (initiator.Ideo != null && initiator.Ideo.IsVeneratedAnimal(recipient))
				{
					num2 *= 2f;
				}
			}
			if (Rand.Chance(num2))
			{
				InteractionWorker_RecruitAttempt.DoRecruit(initiator, recipient, out letterLabel, out letterText, true, false);
				if (!letterLabel.NullOrEmpty())
				{
					letterDef = LetterDefOf.PositiveEvent;
				}
				lookTargets = new LookTargets(new TargetInfo[]
				{
					recipient,
					initiator
				});
				extraSentencePacks.Add(RulePackDefOf.Sentence_RecruitAttemptAccepted);
				return;
			}
			TaggedString taggedString2 = "TextMote_TameFail".Translate(num2.ToStringPercent());
			MoteMaker.ThrowText((initiator.DrawPos + recipient.DrawPos) / 2f, initiator.Map, taggedString2, 8f);
			recipient.mindState.CheckStartMentalStateBecauseRecruitAttempted(initiator);
			extraSentencePacks.Add(RulePackDefOf.Sentence_RecruitAttemptRejected);
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x001BFF1C File Offset: 0x001BE11C
		public static void DoRecruit(Pawn recruiter, Pawn recruitee, bool useAudiovisualEffects = true)
		{
			string text;
			string text2;
			InteractionWorker_RecruitAttempt.DoRecruit(recruiter, recruitee, out text, out text2, useAudiovisualEffects, true);
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x001BFF38 File Offset: 0x001BE138
		public static void DoRecruit(Pawn recruiter, Pawn recruitee, out string letterLabel, out string letter, bool useAudiovisualEffects = true, bool sendLetter = true)
		{
			letterLabel = null;
			letter = null;
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
				recruitee.guest.SetGuestStatus(null, GuestStatus.Guest);
			}
			bool flag = recruitee.Name != null;
			if (recruitee.Faction != recruiter.Faction)
			{
				Faction faction = recruitee.Faction;
				recruitee.SetFaction(recruiter.Faction, recruiter);
				if (faction != null)
				{
					Find.FactionManager.Notify_PawnRecruited(faction);
				}
			}
			if (recruitee.guest != null)
			{
				recruitee.guest.Notify_PawnRecruited();
			}
			if (recruitee.RaceProps.Humanlike)
			{
				if (useAudiovisualEffects)
				{
					letterLabel = "LetterLabelMessageRecruitSuccess".Translate() + ": " + recruitee.LabelShortCap;
					if (sendLetter)
					{
						Find.LetterStack.ReceiveLetter(letterLabel, "MessageRecruitSuccess".Translate(recruiter, recruitee, recruiter.Named("RECRUITER"), recruitee.Named("RECRUITEE")), LetterDefOf.PositiveEvent, recruitee, null, null, null, null);
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
					recruitee.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RecruitedMe, recruiter, null);
				}
				QuestUtility.SendQuestTargetSignals(recruitee.questTags, "Recruited", recruitee.Named("SUBJECT"));
			}
			else
			{
				if (useAudiovisualEffects)
				{
					if (!flag)
					{
						Messages.Message("MessageTameAndNameSuccess".Translate(recruiter.LabelShort, value, recruitee.Name.ToStringFull, recruiter.Named("RECRUITER"), recruitee.Named("RECRUITEE")).AdjustedFor(recruitee, "PAWN", true), recruitee, MessageTypeDefOf.PositiveEvent, true);
					}
					else
					{
						Messages.Message("MessageTameSuccess".Translate(recruiter.LabelShort, value, recruiter.Named("RECRUITER")), recruitee, MessageTypeDefOf.PositiveEvent, true);
					}
					if (recruiter.Spawned && recruitee.Spawned)
					{
						MoteMaker.ThrowText((recruiter.DrawPos + recruitee.DrawPos) / 2f, recruiter.Map, "TextMote_TameSuccess".Translate(), 8f);
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

		// Token: 0x040030DE RID: 12510
		private const float BaseResistanceReductionPerInteraction = 1f;

		// Token: 0x040030DF RID: 12511
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

		// Token: 0x040030E0 RID: 12512
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

		// Token: 0x040030E1 RID: 12513
		private const float MaxMoodForWarning = 0.4f;

		// Token: 0x040030E2 RID: 12514
		private const float MaxOpinionForWarning = -0.01f;

		// Token: 0x040030E3 RID: 12515
		public const float WildmanWildness = 0.75f;

		// Token: 0x040030E4 RID: 12516
		private const float WildmanPrisonerChanceFactor = 0.6f;

		// Token: 0x040030E5 RID: 12517
		private const float VeneratedAnimalChanceFactor = 2f;

		// Token: 0x040030E6 RID: 12518
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

		// Token: 0x040030E7 RID: 12519
		private const float TameChanceFactor_Bonded = 4f;

		// Token: 0x040030E8 RID: 12520
		private const float ChanceToDevelopBondRelationOnTamed = 0.01f;

		// Token: 0x040030E9 RID: 12521
		private const int MenagerieTaleThreshold = 5;
	}
}
