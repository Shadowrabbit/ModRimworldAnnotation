using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E98 RID: 3736
	public class ThoughtWorker_TeetotalerVsChemicalInterest : ThoughtWorker
	{
		// Token: 0x0600537F RID: 21375 RVA: 0x001C0D00 File Offset: 0x001BEF00
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.IsTeetotaler())
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
			if (other.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) <= 0)
			{
				return false;
			}
			return true;
		}
	}
}
