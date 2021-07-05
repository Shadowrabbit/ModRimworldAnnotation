using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000987 RID: 2439
	public class ThoughtWorker_Incestuous : ThoughtWorker
	{
		// Token: 0x06003D96 RID: 15766 RVA: 0x001529C8 File Offset: 0x00150BC8
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (LovePartnerRelationUtility.IncestOpinionOffsetFor(other, pawn) == 0f)
			{
				return false;
			}
			return true;
		}
	}
}
