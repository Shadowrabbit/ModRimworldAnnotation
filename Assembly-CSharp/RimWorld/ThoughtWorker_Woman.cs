using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA3 RID: 3747
	public class ThoughtWorker_Woman : ThoughtWorker
	{
		// Token: 0x06005395 RID: 21397 RVA: 0x001C124C File Offset: 0x001BF44C
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
