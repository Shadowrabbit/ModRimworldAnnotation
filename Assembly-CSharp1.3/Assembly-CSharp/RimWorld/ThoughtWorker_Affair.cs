using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000986 RID: 2438
	public class ThoughtWorker_Affair : ThoughtWorker
	{
		// Token: 0x06003D94 RID: 15764 RVA: 0x00152910 File Offset: 0x00150B10
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			if (!p.relations.DirectRelationExists(PawnRelationDefOf.Spouse, otherPawn))
			{
				return false;
			}
			List<DirectPawnRelation> directRelations = otherPawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].otherPawn != p && !directRelations[i].otherPawn.Dead && (directRelations[i].def == PawnRelationDefOf.Lover || directRelations[i].def == PawnRelationDefOf.Fiance) && !new HistoryEvent(otherPawn.GetHistoryEventLoveRelationCount(), otherPawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
				{
					return false;
				}
			}
			return false;
		}
	}
}
