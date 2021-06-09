using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA1 RID: 3745
	public class ThoughtWorker_CreepyBreathing : ThoughtWorker
	{
		// Token: 0x06005391 RID: 21393 RVA: 0x001C1164 File Offset: 0x001BF364
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!other.story.traits.HasTrait(TraitDefOf.CreepyBreathing))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
			{
				return false;
			}
			return true;
		}
	}
}
