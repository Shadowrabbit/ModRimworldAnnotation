using System;

namespace RimWorld
{
	// Token: 0x02000991 RID: 2449
	public class Thought_ChemicalInterestVsTeetotaler : Thought_SituationalSocial
	{
		// Token: 0x06003DAA RID: 15786 RVA: 0x00152E1C File Offset: 0x0015101C
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			int num = this.pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			if (this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) >= 0)
			{
				return 0f;
			}
			if (num == 1)
			{
				return -20f;
			}
			return -30f;
		}
	}
}
