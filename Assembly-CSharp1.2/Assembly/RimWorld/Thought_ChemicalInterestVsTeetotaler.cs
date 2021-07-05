using System;

namespace RimWorld
{
	// Token: 0x02000E9C RID: 3740
	public class Thought_ChemicalInterestVsTeetotaler : Thought_SituationalSocial
	{
		// Token: 0x06005387 RID: 21383 RVA: 0x001C0EFC File Offset: 0x001BF0FC
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
