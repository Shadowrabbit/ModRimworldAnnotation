using System;

namespace RimWorld
{
	// Token: 0x02000988 RID: 2440
	public class Thought_Incestuous : Thought_SituationalSocial
	{
		// Token: 0x06003D98 RID: 15768 RVA: 0x00152A14 File Offset: 0x00150C14
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			return LovePartnerRelationUtility.IncestOpinionOffsetFor(this.otherPawn, this.pawn);
		}
	}
}
