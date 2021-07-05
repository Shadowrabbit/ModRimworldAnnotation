using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000994 RID: 2452
	public class ThoughtWorker_Ugly : ThoughtWorker
	{
		// Token: 0x06003DB0 RID: 15792 RVA: 0x00152FA0 File Offset: 0x001511A0
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
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
			float statValue = other.GetStatValue(StatDefOf.PawnBeauty, true);
			if (statValue <= -2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (statValue <= -1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return false;
		}
	}
}
