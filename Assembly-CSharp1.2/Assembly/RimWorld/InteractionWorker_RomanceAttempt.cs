using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001486 RID: 5254
	public class InteractionWorker_RomanceAttempt : InteractionWorker
	{
		// Token: 0x06007154 RID: 29012 RVA: 0x0022AF98 File Offset: 0x00229198
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
			int num2 = initiator.relations.OpinionOf(recipient);
			if (num2 < 5)
			{
				return 0f;
			}
			if (recipient.relations.OpinionOf(initiator) < 5)
			{
				return 0f;
			}
			float num3 = 1f;
			Pawn pawn = LovePartnerRelationUtility.ExistingMostLikedLovePartner(initiator, false);
			if (pawn != null)
			{
				float value = (float)initiator.relations.OpinionOf(pawn);
				num3 = Mathf.InverseLerp(50f, -50f, value);
			}
			float num4 = initiator.story.traits.HasTrait(TraitDefOf.Gay) ? 1f : ((initiator.gender == Gender.Female) ? 0.15f : 1f);
			float num5 = Mathf.InverseLerp(0.15f, 1f, num);
			float num6 = Mathf.InverseLerp(5f, 100f, (float)num2);
			float num7;
			if (initiator.gender == recipient.gender)
			{
				if (initiator.story.traits.HasTrait(TraitDefOf.Gay) && recipient.story.traits.HasTrait(TraitDefOf.Gay))
				{
					num7 = 1f;
				}
				else
				{
					num7 = 0.15f;
				}
			}
			else if (!initiator.story.traits.HasTrait(TraitDefOf.Gay) && !recipient.story.traits.HasTrait(TraitDefOf.Gay))
			{
				num7 = 1f;
			}
			else
			{
				num7 = 0.15f;
			}
			return 1.15f * num4 * num5 * num6 * num3 * num7;
		}

		// Token: 0x06007155 RID: 29013 RVA: 0x0022B134 File Offset: 0x00229334
		public float SuccessChance(Pawn initiator, Pawn recipient)
		{
			float num = 0.6f * recipient.relations.SecondaryRomanceChanceFactor(initiator) * Mathf.InverseLerp(5f, 100f, (float)recipient.relations.OpinionOf(initiator));
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
			else if (recipient.GetSpouse() != null && !recipient.GetSpouse().Dead)
			{
				pawn = recipient.GetSpouse();
				num2 = 0.3f;
			}
			if (pawn != null)
			{
				num2 *= Mathf.InverseLerp(100f, 0f, (float)recipient.relations.OpinionOf(pawn));
				num2 *= Mathf.Clamp01(1f - recipient.relations.SecondaryRomanceChanceFactor(pawn));
			}
			return Mathf.Clamp01(num * num2);
		}

		// Token: 0x06007156 RID: 29014 RVA: 0x0022B274 File Offset: 0x00229474
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			if (Rand.Value < this.SuccessChance(initiator, recipient))
			{
				List<Pawn> list;
				this.BreakLoverAndFianceRelations(initiator, out list);
				List<Pawn> list2;
				this.BreakLoverAndFianceRelations(recipient, out list2);
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
				if (initiator.needs.mood != null)
				{
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.BrokeUpWithMe, recipient);
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMe, recipient);
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, recipient);
				}
				if (recipient.needs.mood != null)
				{
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.BrokeUpWithMe, initiator);
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMe, initiator);
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, initiator);
				}
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
				initiator.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RebuffedMyRomanceAttempt, recipient);
			}
			if (recipient.needs.mood != null)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedRomanceAttemptOnMe, initiator);
			}
			if (recipient.needs.mood != null && recipient.relations.OpinionOf(initiator) <= 0)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, initiator);
			}
			extraSentencePacks.Add(RulePackDefOf.Sentence_RomanceAttemptRejected);
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x06007157 RID: 29015 RVA: 0x0022B4F4 File Offset: 0x002296F4
		private void BreakLoverAndFianceRelations(Pawn pawn, out List<Pawn> oldLoversAndFiances)
		{
			oldLoversAndFiances = new List<Pawn>();
			for (;;)
			{
				Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null);
				if (firstDirectRelationPawn != null)
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, firstDirectRelationPawn);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, firstDirectRelationPawn);
					oldLoversAndFiances.Add(firstDirectRelationPawn);
				}
				else
				{
					Pawn firstDirectRelationPawn2 = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
					if (firstDirectRelationPawn2 == null)
					{
						break;
					}
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Fiance, firstDirectRelationPawn2);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, firstDirectRelationPawn2);
					oldLoversAndFiances.Add(firstDirectRelationPawn2);
				}
			}
		}

		// Token: 0x06007158 RID: 29016 RVA: 0x0004C46E File Offset: 0x0004A66E
		private void TryAddCheaterThought(Pawn pawn, Pawn cheater)
		{
			if (pawn.Dead || pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.CheatedOnMe, cheater);
		}

		// Token: 0x06007159 RID: 29017 RVA: 0x0022B590 File Offset: 0x00229790
		private void GetNewLoversLetter(Pawn initiator, Pawn recipient, List<Pawn> initiatorOldLoversAndFiances, List<Pawn> recipientOldLoversAndFiances, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			bool flag = false;
			if ((initiator.GetSpouse() != null && !initiator.GetSpouse().Dead) || (recipient.GetSpouse() != null && !recipient.GetSpouse().Dead))
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
			stringBuilder.AppendLine("LetterNewLovers".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2")));
			stringBuilder.AppendLine();
			if (flag)
			{
				if (initiator.GetSpouse() != null)
				{
					stringBuilder.AppendLine("LetterAffair".Translate(initiator.LabelShort, initiator.GetSpouse().LabelShort, recipient.LabelShort, initiator.Named("PAWN1"), recipient.Named("PAWN2"), initiator.GetSpouse().Named("SPOUSE")));
				}
				if (recipient.GetSpouse() != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLine("LetterAffair".Translate(recipient.LabelShort, recipient.GetSpouse().LabelShort, initiator.LabelShort, recipient.Named("PAWN1"), recipient.GetSpouse().Named("SPOUSE"), initiator.Named("PAWN2")));
				}
			}
			for (int i = 0; i < initiatorOldLoversAndFiances.Count; i++)
			{
				if (!initiatorOldLoversAndFiances[i].Dead)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLine("LetterNoLongerLovers".Translate(initiator.LabelShort, initiatorOldLoversAndFiances[i].LabelShort, initiator.Named("PAWN1"), initiatorOldLoversAndFiances[i].Named("PAWN2")));
				}
			}
			for (int j = 0; j < recipientOldLoversAndFiances.Count; j++)
			{
				if (!recipientOldLoversAndFiances[j].Dead)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLine("LetterNoLongerLovers".Translate(recipient.LabelShort, recipientOldLoversAndFiances[j].LabelShort, recipient.Named("PAWN1"), recipientOldLoversAndFiances[j].Named("PAWN2")));
				}
			}
			letterText = stringBuilder.ToString().TrimEndNewlines();
			lookTargets = new LookTargets(new TargetInfo[]
			{
				initiator,
				recipient
			});
		}

		// Token: 0x04004AD1 RID: 19153
		private const float MinRomanceChanceForRomanceAttempt = 0.15f;

		// Token: 0x04004AD2 RID: 19154
		private const int MinOpinionForRomanceAttempt = 5;

		// Token: 0x04004AD3 RID: 19155
		private const float BaseSelectionWeight = 1.15f;

		// Token: 0x04004AD4 RID: 19156
		private const float BaseSuccessChance = 0.6f;
	}
}
