using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002D5 RID: 725
	public class HediffGiver_BrainInjury : HediffGiver
	{
		// Token: 0x06001395 RID: 5013 RVA: 0x0006F0F4 File Offset: 0x0006D2F4
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

		// Token: 0x04000E69 RID: 3689
		public float chancePerDamagePct;

		// Token: 0x04000E6A RID: 3690
		public string letterLabel;

		// Token: 0x04000E6B RID: 3691
		public string letter;
	}
}
