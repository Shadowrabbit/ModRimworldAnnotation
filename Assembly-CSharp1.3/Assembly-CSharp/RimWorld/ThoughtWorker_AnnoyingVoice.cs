using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000995 RID: 2453
	public class ThoughtWorker_AnnoyingVoice : ThoughtWorker
	{
		// Token: 0x06003DB2 RID: 15794 RVA: 0x00153038 File Offset: 0x00151238
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!other.story.traits.HasTrait(TraitDefOf.AnnoyingVoice))
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
