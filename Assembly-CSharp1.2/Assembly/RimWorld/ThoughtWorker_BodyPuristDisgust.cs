using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA5 RID: 3749
	public class ThoughtWorker_BodyPuristDisgust : ThoughtWorker
	{
		// Token: 0x06005399 RID: 21401 RVA: 0x001C1350 File Offset: 0x001BF550
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
