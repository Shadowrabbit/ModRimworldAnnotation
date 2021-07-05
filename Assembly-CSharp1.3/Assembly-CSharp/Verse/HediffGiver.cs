using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020002D0 RID: 720
	public abstract class HediffGiver
	{
		// Token: 0x06001381 RID: 4993 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0006EA58 File Offset: 0x0006CC58
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return (this.allowOnLodgers || !pawn.IsQuestLodger()) && (this.allowOnQuestRewardPawns || !pawn.IsWorldPawn() || !pawn.IsQuestReward()) && (this.allowOnBeggars || pawn.kindDef != PawnKindDefOf.Beggar) && HediffGiverUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0006EAC8 File Offset: 0x0006CCC8
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

		// Token: 0x06001385 RID: 4997 RVA: 0x0006EC04 File Offset: 0x0006CE04
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.hediff == null)
			{
				yield return "hediff is null";
			}
			yield break;
		}

		// Token: 0x04000E5C RID: 3676
		[TranslationHandle]
		public HediffDef hediff;

		// Token: 0x04000E5D RID: 3677
		public List<BodyPartDef> partsToAffect;

		// Token: 0x04000E5E RID: 3678
		public bool canAffectAnyLivePart;

		// Token: 0x04000E5F RID: 3679
		public bool allowOnLodgers = true;

		// Token: 0x04000E60 RID: 3680
		public bool allowOnQuestRewardPawns = true;

		// Token: 0x04000E61 RID: 3681
		public bool allowOnBeggars = true;

		// Token: 0x04000E62 RID: 3682
		public int countToAffect = 1;
	}
}
