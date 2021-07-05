using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097E RID: 2430
	public class ThoughtWorker_OpinionOfMyLover : ThoughtWorker
	{
		// Token: 0x06003D7F RID: 15743 RVA: 0x001524E8 File Offset: 0x001506E8
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
