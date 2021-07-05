using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000996 RID: 2454
	public class ThoughtWorker_CreepyBreathing : ThoughtWorker
	{
		// Token: 0x06003DB4 RID: 15796 RVA: 0x001530C4 File Offset: 0x001512C4
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
			if (pawn.story.traits.HasTrait(TraitDefOf.Kind))
			{
				return false;
			}
			return true;
		}
	}
}
