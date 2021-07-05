using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000998 RID: 2456
	public class ThoughtWorker_Woman : ThoughtWorker
	{
		// Token: 0x06003DB8 RID: 15800 RVA: 0x001531CC File Offset: 0x001513CC
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.DislikesWomen))
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
			if (other.gender != Gender.Female)
			{
				return false;
			}
			return true;
		}
	}
}
