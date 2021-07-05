using System;

namespace RimWorld
{
	// Token: 0x0200098F RID: 2447
	public class Thought_TeetotalerVsChemicalInterest : Thought_SituationalSocial
	{
		// Token: 0x06003DA6 RID: 15782 RVA: 0x00152D34 File Offset: 0x00150F34
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			int num = this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			if (num <= 0)
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
