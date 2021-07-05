using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000985 RID: 2437
	public class ThoughtWorker_WantToSleepWithSpouseOrLover : ThoughtWorker
	{
		// Token: 0x06003D92 RID: 15762 RVA: 0x00152810 File Offset: 0x00150A10
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<DirectPawnRelation> list = LovePartnerRelationUtility.ExistingLovePartners(p, false);
			if (list.NullOrEmpty<DirectPawnRelation>())
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].otherPawn.IsColonist && !list[i].otherPawn.IsWorldPawn() && list[i].otherPawn.relations.everSeenByPlayer)
				{
					if (p.ownership.OwnedBed != null && p.ownership.OwnedBed == list[i].otherPawn.ownership.OwnedBed)
					{
						return false;
					}
					HistoryEventDef def = (list[i].def == PawnRelationDefOf.Spouse) ? HistoryEventDefOf.SharedBed_Spouse : HistoryEventDefOf.SharedBed_NonSpouse;
					if (new HistoryEvent(def, p.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
					{
						num++;
					}
				}
			}
			return num > 0;
		}
	}
}
