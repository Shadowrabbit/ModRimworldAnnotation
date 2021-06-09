using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E92 RID: 3730
	public class ThoughtWorker_Incestuous : ThoughtWorker
	{
		// Token: 0x06005373 RID: 21363 RVA: 0x001C0B58 File Offset: 0x001BED58
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
