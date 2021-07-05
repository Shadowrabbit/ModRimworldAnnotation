using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000989 RID: 2441
	public class ThoughtWorker_Disfigured : ThoughtWorker
	{
		// Token: 0x06003D9A RID: 15770 RVA: 0x00152A48 File Offset: 0x00150C48
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || other.Dead)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!RelationsUtility.IsDisfigured(other, pawn, false))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			if (pawn.story.traits.HasTrait(TraitDefOf.Kind))
			{
				return false;
			}
			if (pawn.Ideo != null && pawn.Ideo.IdeoApprovesOfBlindness() && !RelationsUtility.IsDisfigured(other, pawn, true) && (ThoughtWorker_Precept_Blind.IsBlind(other) || ThoughtWorker_Precept_HalfBlind.IsHalfBlind(other)))
			{
				return false;
			}
			return true;
		}
	}
}
