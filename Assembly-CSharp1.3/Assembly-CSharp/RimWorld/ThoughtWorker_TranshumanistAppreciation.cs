using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000999 RID: 2457
	public class ThoughtWorker_TranshumanistAppreciation : ThoughtWorker
	{
		// Token: 0x06003DBA RID: 15802 RVA: 0x00153248 File Offset: 0x00151448
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.Transhumanist))
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (other.def != p.def)
			{
				return false;
			}
			int num = other.health.hediffSet.CountAddedAndImplantedParts();
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}
	}
}
