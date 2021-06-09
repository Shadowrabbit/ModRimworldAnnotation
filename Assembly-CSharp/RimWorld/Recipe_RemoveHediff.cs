using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D4 RID: 5076
	public class Recipe_RemoveHediff : Recipe_Surgery
	{
		// Token: 0x06006DE5 RID: 28133 RVA: 0x0021A558 File Offset: 0x00218758
		public override bool AvailableOnNow(Thing thing)
		{
			Pawn pawn;
			if ((pawn = (thing as Pawn)) == null)
			{
				return false;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if ((!this.recipe.targetsBodyPart || hediffs[i].Part != null) && hediffs[i].def == this.recipe.removesHediff && hediffs[i].Visible)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006DE6 RID: 28134 RVA: 0x0004A9AA File Offset: 0x00048BAA
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			List<Hediff> allHediffs = pawn.health.hediffSet.hediffs;
			int num;
			for (int i = 0; i < allHediffs.Count; i = num + 1)
			{
				if (allHediffs[i].Part != null && allHediffs[i].def == recipe.removesHediff && allHediffs[i].Visible)
				{
					yield return allHediffs[i].Part;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06006DE7 RID: 28135 RVA: 0x0021A5D8 File Offset: 0x002187D8
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
				{
					string text;
					if (!this.recipe.successfullyRemovedHediffMessage.NullOrEmpty())
					{
						text = this.recipe.successfullyRemovedHediffMessage.Formatted(billDoer.LabelShort, pawn.LabelShort);
					}
					else
					{
						text = "MessageSuccessfullyRemovedHediff".Translate(billDoer.LabelShort, pawn.LabelShort, this.recipe.removesHediff.label.Named("HEDIFF"), billDoer.Named("SURGEON"), pawn.Named("PATIENT"));
					}
					Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == this.recipe.removesHediff && x.Part == part && x.Visible);
			if (hediff != null)
			{
				pawn.health.RemoveHediff(hediff);
			}
		}
	}
}
