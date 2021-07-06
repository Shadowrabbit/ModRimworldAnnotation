using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200040C RID: 1036
	public class HediffGiver_BrainInjury : HediffGiver
	{
		// Token: 0x06001935 RID: 6453 RVA: 0x000E177C File Offset: 0x000DF97C
		public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			if (!(hediff is Hediff_Injury))
			{
				return false;
			}
			if (hediff.Part != pawn.health.hediffSet.GetBrain())
			{
				return false;
			}
			float num = hediff.Severity / hediff.Part.def.GetMaxHealth(pawn);
			if (Rand.Value < num * this.chancePerDamagePct && base.TryApply(pawn, null))
			{
				if ((pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony) && !this.letter.NullOrEmpty())
				{
					Find.LetterStack.ReceiveLetter(this.letterLabel, this.letter.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
				}
				return true;
			}
			return false;
		}

		// Token: 0x040012DE RID: 4830
		public float chancePerDamagePct;

		// Token: 0x040012DF RID: 4831
		public string letterLabel;

		// Token: 0x040012E0 RID: 4832
		public string letter;
	}
}
