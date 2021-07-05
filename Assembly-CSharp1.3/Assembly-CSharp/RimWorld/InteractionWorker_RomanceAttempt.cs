using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFB RID: 3579
	public class InteractionWorker_RomanceAttempt : InteractionWorker
	{
		// Token: 0x060052DB RID: 21211 RVA: 0x001C0480 File Offset: 0x001BE680
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (TutorSystem.TutorialMode)
			{
				return 0f;
			}
			if (LovePartnerRelationUtility.LovePartnerRelationExists(initiator, recipient))
			{
				return 0f;
			}
			float num = initiator.relations.SecondaryRomanceChanceFactor(recipient);
			if (num < 0.15f)
			{
				return 0f;
			}
			float num2 = 5f;
			int num3 = initiator.relations.OpinionOf(recipient);
			if ((float)num3 < num2)
			{
				return 0f;
			}
			if ((float)recipient.relations.OpinionOf(initiator) < num2)
			{
				return 0f;
			}
			float num4 = 1f;
			if (!new HistoryEvent(initiator.GetHistoryEventForLoveRelationCountPlusOne(), initiator.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				Pawn pawn = LovePartnerRelationUtility.ExistingMostLikedLovePartner(initiator, false);
				if (pawn != null)
				{
					float value = (float)initiator.relations.OpinionOf(pawn);
					num4 = Mathf.InverseLerp(50f, -50f, value);
				}
			}
			float num5 = initiator.story.traits.HasTrait(TraitDefOf.Gay) ? 1f : ((initiator.gender == Gender.Female) ? 0.15f : 1f);
			float num6 = Mathf.InverseLerp(0.15f, 1f, num);
			float num7 = Mathf.InverseLerp(num2, 100f, (float)num3);
			float num8;
			if (initiator.gender == recipient.gender)
			{
				if (initiator.story.traits.HasTrait(TraitDefOf.Gay) && recipient.story.traits.HasTrait(TraitDefOf.Gay))
				{
					num8 = 1f;
				}
				else
				{
					num8 = 0.15f;
				}
			}
			else if (!initiator.story.traits.HasTrait(TraitDefOf.Gay) && !recipient.story.traits.HasTrait(TraitDefOf.Gay))
			{
				num8 = 1f;
			}
			else
			{
				num8 = 0.15f;
			}
			return 1.15f * num5 * num6 * num7 * num4 * num8;
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x001C0640 File Offset: 0x001BE840
		public float SuccessChance(Pawn initiator, Pawn recipient)
		{
			float num = 0.6f;
			num *= recipient.relations.SecondaryRomanceChanceFactor(initiator);
			num *= Mathf.InverseLerp(5f, 100f, (float)recipient.relations.OpinionOf(initiator));
			if (!new HistoryEvent(recipient.GetHistoryEventForLoveRelationCountPlusOne(), recipient.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				float num2 = 1f;
				Pawn pawn = null;
				if (recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, (Pawn x) => !x.Dead) != null)
				{
					pawn = recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null);
					num2 = 0.6f;
				}
				else if (recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => !x.Dead) != null)
				{
					pawn = recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
					num2 = 0.1f;
				}
				else if (recipient.GetSpouseCount(false) > 0)
				{
					pawn = recipient.GetMostLikedSpouseRelation().otherPawn;
					num2 = 0.3f;
				}
				if (pawn != null)
				{
					num2 *= Mathf.InverseLerp(100f, 0f, (float)recipient.relations.OpinionOf(pawn));
					num2 *= Mathf.Clamp01(1f - recipient.relations.SecondaryRomanceChanceFactor(pawn));
				}
				num *= num2;
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x001C07A4 File Offset: 0x001BE9A4
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			if (Rand.Value < this.SuccessChance(initiator, recipient))
			{
				List<Pawn> list;
				this.BreakLoverAndFianceRelations(initiator, out list);
				List<Pawn> list2;
				this.BreakLoverAndFianceRelations(recipient, out list2);
				this.RemoveBrokeUpAndFailedRomanceThoughts(initiator, recipient);
				this.RemoveBrokeUpAndFailedRomanceThoughts(recipient, initiator);
				for (int i = 0; i < list.Count; i++)
				{
					this.TryAddCheaterThought(list[i], initiator);
				}
				for (int j = 0; j < list2.Count; j++)
				{
					this.TryAddCheaterThought(list2[j], recipient);
				}
				initiator.relations.TryRemoveDirectRelation(PawnRelationDefOf.ExLover, recipient);
				initiator.relations.AddDirectRelation(PawnRelationDefOf.Lover, recipient);
				TaleRecorder.RecordTale(TaleDefOf.BecameLover, new object[]
				{
					initiator,
					recipient
				});
				if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(recipient))
				{
					this.GetNewLoversLetter(initiator, recipient, list, list2, out letterText, out letterLabel, out letterDef, out lookTargets);
				}
				else
				{
					letterText = null;
					letterLabel = null;
					letterDef = null;
					lookTargets = null;
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_RomanceAttemptAccepted);
				LovePartnerRelationUtility.TryToShareBed(initiator, recipient);
				return;
			}
			if (initiator.needs.mood != null)
			{
				initiator.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RebuffedMyRomanceAttempt, recipient, null);
			}
			if (recipient.needs.mood != null)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedRomanceAttemptOnMe, initiator, null);
			}
			if (recipient.needs.mood != null && recipient.relations.OpinionOf(initiator) <= 0)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, initiator, null);
			}
			extraSentencePacks.Add(RulePackDefOf.Sentence_RomanceAttemptRejected);
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x001C095C File Offset: 0x001BEB5C
		private void RemoveBrokeUpAndFailedRomanceThoughts(Pawn pawn, Pawn otherPawn)
		{
			if (pawn.needs.mood != null)
			{
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.BrokeUpWithMe, otherPawn);
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMe, otherPawn);
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, otherPawn);
			}
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x001C09D8 File Offset: 0x001BEBD8
		private void BreakLoverAndFianceRelations(Pawn pawn, out List<Pawn> oldLoversAndFiances)
		{
			oldLoversAndFiances = new List<Pawn>();
			int num = 200;
			while (num > 0 && !new HistoryEvent(pawn.GetHistoryEventForLoveRelationCountPlusOne(), pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				Pawn pawn2 = LovePartnerRelationUtility.ExistingLeastLikedPawnWithRelation(pawn, (DirectPawnRelation r) => r.def == PawnRelationDefOf.Lover);
				if (pawn2 != null)
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, pawn2);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, pawn2);
					oldLoversAndFiances.Add(pawn2);
					num--;
				}
				else
				{
					Pawn pawn3 = LovePartnerRelationUtility.ExistingLeastLikedPawnWithRelation(pawn, (DirectPawnRelation r) => r.def == PawnRelationDefOf.Fiance);
					if (pawn3 == null)
					{
						break;
					}
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Fiance, pawn3);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, pawn3);
					oldLoversAndFiances.Add(pawn3);
					num--;
				}
			}
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x001C0ACE File Offset: 0x001BECCE
		private void TryAddCheaterThought(Pawn pawn, Pawn cheater)
		{
			if (pawn.Dead || pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.CheatedOnMe, cheater, null);
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x001C0B08 File Offset: 0x001BED08
		private void GetNewLoversLetter(Pawn initiator, Pawn recipient, List<Pawn> initiatorOldLoversAndFiances, List<Pawn> recipientOldLoversAndFiances, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			bool flag = false;
			HistoryEvent ev = new HistoryEvent(initiator.GetHistoryEventLoveRelationCount(), initiator.Named(HistoryEventArgsNames.Doer));
			HistoryEvent ev2 = new HistoryEvent(recipient.GetHistoryEventLoveRelationCount(), recipient.Named(HistoryEventArgsNames.Doer));
			if (!ev.DoerWillingToDo() || !ev2.DoerWillingToDo())
			{
				letterLabel = "LetterLabelAffair".Translate();
				letterDef = LetterDefOf.NegativeEvent;
				flag = true;
			}
			else
			{
				letterLabel = "LetterLabelNewLovers".Translate();
				letterDef = LetterDefOf.PositiveEvent;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (BedUtility.WillingToShareBed(initiator, recipient))
			{
				stringBuilder.AppendLineTagged("LetterNewLovers".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2")));
			}
			if (flag)
			{
				Pawn firstSpouse = initiator.GetFirstSpouse();
				if (firstSpouse != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLineTagged("LetterAffair".Translate(initiator.LabelShort, firstSpouse.LabelShort, recipient.LabelShort, initiator.Named("PAWN1"), recipient.Named("PAWN2"), firstSpouse.Named("SPOUSE")));
				}
				Pawn firstSpouse2 = recipient.GetFirstSpouse();
				if (firstSpouse2 != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLineTagged("LetterAffair".Translate(recipient.LabelShort, firstSpouse2.LabelShort, initiator.LabelShort, recipient.Named("PAWN1"), firstSpouse2.Named("SPOUSE"), initiator.Named("PAWN2")));
				}
			}
			for (int i = 0; i < initiatorOldLoversAndFiances.Count; i++)
			{
				if (!initiatorOldLoversAndFiances[i].Dead)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLineTagged("LetterNoLongerLovers".Translate(initiator.LabelShort, initiatorOldLoversAndFiances[i].LabelShort, initiator.Named("PAWN1"), initiatorOldLoversAndFiances[i].Named("PAWN2")));
				}
			}
			for (int j = 0; j < recipientOldLoversAndFiances.Count; j++)
			{
				if (!recipientOldLoversAndFiances[j].Dead)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLineTagged("LetterNoLongerLovers".Translate(recipient.LabelShort, recipientOldLoversAndFiances[j].LabelShort, recipient.Named("PAWN1"), recipientOldLoversAndFiances[j].Named("PAWN2")));
				}
			}
			letterText = stringBuilder.ToString().TrimEndNewlines();
			lookTargets = new LookTargets(new TargetInfo[]
			{
				initiator,
				recipient
			});
		}

		// Token: 0x040030EA RID: 12522
		private const float MinRomanceChanceForRomanceAttempt = 0.15f;

		// Token: 0x040030EB RID: 12523
		private const int MinOpinionForRomanceAttempt = 5;

		// Token: 0x040030EC RID: 12524
		private const float BaseSelectionWeight = 1.15f;

		// Token: 0x040030ED RID: 12525
		private const float BaseSuccessChance = 0.6f;

		// Token: 0x040030EE RID: 12526
		private const float PsychicMateSelectionWeightFactor = 3f;
	}
}
