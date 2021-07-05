using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098A RID: 2442
	public class ThoughtWorker_HardWorkerVsLazy : ThoughtWorker
	{
		// Token: 0x06003D9C RID: 15772 RVA: 0x00152B0C File Offset: 0x00150D0C
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) <= 0)
			{
				return false;
			}
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (other.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) > 0)
			{
				return false;
			}
			return true;
		}
	}
}
