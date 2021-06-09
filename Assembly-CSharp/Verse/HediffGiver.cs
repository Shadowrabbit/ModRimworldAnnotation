using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000403 RID: 1027
	public abstract class HediffGiver
	{
		// Token: 0x06001909 RID: 6409 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x000E0F60 File Offset: 0x000DF160
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return (this.allowOnLodgers || !pawn.IsQuestLodger()) && (this.allowOnQuestRewardPawns || !pawn.IsWorldPawn() || !pawn.IsQuestReward()) && HediffGiverUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x000E0FB8 File Offset: 0x000DF1B8
		protected void SendLetter(Pawn pawn, Hediff cause)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				if (cause == null)
				{
					Find.LetterStack.ReceiveLetter("LetterHediffFromRandomHediffGiverLabel".Translate(pawn.LabelShortCap, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHediffFromRandomHediffGiver".Translate(pawn.LabelShortCap, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
					return;
				}
				Find.LetterStack.ReceiveLetter("LetterHealthComplicationsLabel".Translate(pawn.LabelShort, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHealthComplications".Translate(pawn.LabelShortCap, this.hediff.LabelCap, cause.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
			}
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00017B6D File Offset: 0x00015D6D
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.hediff == null)
			{
				yield return "hediff is null";
			}
			yield break;
		}

		// Token: 0x040012C1 RID: 4801
		[TranslationHandle]
		public HediffDef hediff;

		// Token: 0x040012C2 RID: 4802
		public List<BodyPartDef> partsToAffect;

		// Token: 0x040012C3 RID: 4803
		public bool canAffectAnyLivePart;

		// Token: 0x040012C4 RID: 4804
		public bool allowOnLodgers = true;

		// Token: 0x040012C5 RID: 4805
		public bool allowOnQuestRewardPawns = true;

		// Token: 0x040012C6 RID: 4806
		public int countToAffect = 1;
	}
}
