using System;

namespace RimWorld
{
	// Token: 0x02000E96 RID: 3734
	public class Thought_HardWorkerVsLazy : Thought_SituationalSocial
	{
		// Token: 0x0600537B RID: 21371 RVA: 0x001C0C9C File Offset: 0x001BEE9C
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			int num = this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.Industriousness);
			if (num > 0)
			{
				return 0f;
			}
			if (num == 0)
			{
				return -5f;
			}
			if (num == -1)
			{
				return -20f;
			}
			return -30f;
		}
	}
}
