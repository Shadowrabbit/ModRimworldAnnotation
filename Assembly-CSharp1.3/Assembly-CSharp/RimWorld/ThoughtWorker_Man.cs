using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000997 RID: 2455
	public class ThoughtWorker_Man : ThoughtWorker
	{
		// Token: 0x06003DB6 RID: 15798 RVA: 0x00153150 File Offset: 0x00151350
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.DislikesMen))
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
			if (other.gender != Gender.Male)
			{
				return false;
			}
			return true;
		}
	}
}
