using System;

namespace RimWorld
{
	// Token: 0x0200098B RID: 2443
	public class Thought_HardWorkerVsLazy : Thought_SituationalSocial
	{
		// Token: 0x06003D9E RID: 15774 RVA: 0x00152B98 File Offset: 0x00150D98
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
