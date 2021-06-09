using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA0 RID: 3744
	public class ThoughtWorker_AnnoyingVoice : ThoughtWorker
	{
		// Token: 0x0600538F RID: 21391 RVA: 0x001C10F8 File Offset: 0x001BF2F8
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
			return true;
		}
	}
}
