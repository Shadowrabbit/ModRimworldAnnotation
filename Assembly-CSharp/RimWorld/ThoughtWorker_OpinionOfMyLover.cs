using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E88 RID: 3720
	public class ThoughtWorker_OpinionOfMyLover : ThoughtWorker
	{
		// Token: 0x06005359 RID: 21337 RVA: 0x001C0750 File Offset: 0x001BE950
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, false);
			if (directPawnRelation == null)
			{
				return false;
			}
			if (!directPawnRelation.otherPawn.IsColonist || directPawnRelation.otherPawn.IsWorldPawn() || !directPawnRelation.otherPawn.relations.everSeenByPlayer)
			{
				return false;
			}
			return p.relations.OpinionOf(directPawnRelation.otherPawn) != 0;
		}
	}
}
