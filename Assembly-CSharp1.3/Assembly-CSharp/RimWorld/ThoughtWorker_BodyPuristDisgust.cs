using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099A RID: 2458
	public class ThoughtWorker_BodyPuristDisgust : ThoughtWorker
	{
		// Token: 0x06003DBC RID: 15804 RVA: 0x001532D0 File Offset: 0x001514D0
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.BodyPurist))
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
