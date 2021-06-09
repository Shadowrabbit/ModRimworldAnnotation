using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8F RID: 3727
	public class ThoughtWorker_WantToSleepWithSpouseOrLover : ThoughtWorker
	{
		// Token: 0x0600536C RID: 21356 RVA: 0x001C099C File Offset: 0x001BEB9C
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
			if (p.ownership.OwnedBed != null && p.ownership.OwnedBed == directPawnRelation.otherPawn.ownership.OwnedBed)
			{
				return false;
			}
			if (p.relations.OpinionOf(directPawnRelation.otherPawn) <= 0)
			{
				return false;
			}
			return true;
		}
	}
}
