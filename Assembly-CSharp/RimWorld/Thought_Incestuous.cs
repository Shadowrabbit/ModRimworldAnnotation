using System;

namespace RimWorld
{
	// Token: 0x02000E93 RID: 3731
	public class Thought_Incestuous : Thought_SituationalSocial
	{
		// Token: 0x06005375 RID: 21365 RVA: 0x0003A384 File Offset: 0x00038584
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
