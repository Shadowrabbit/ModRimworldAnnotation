using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA2 RID: 3746
	public class ThoughtWorker_Man : ThoughtWorker
	{
		// Token: 0x06005393 RID: 21395 RVA: 0x001C11D0 File Offset: 0x001BF3D0
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
