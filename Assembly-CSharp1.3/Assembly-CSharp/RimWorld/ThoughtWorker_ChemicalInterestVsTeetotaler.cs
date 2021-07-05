using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000990 RID: 2448
	public class ThoughtWorker_ChemicalInterestVsTeetotaler : ThoughtWorker
	{
		// Token: 0x06003DA8 RID: 15784 RVA: 0x00152D90 File Offset: 0x00150F90
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) <= 0)
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
			if (other.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) >= 0)
			{
				return false;
			}
			return true;
		}
	}
}
