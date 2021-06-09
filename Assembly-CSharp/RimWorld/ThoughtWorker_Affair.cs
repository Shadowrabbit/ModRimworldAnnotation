using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E91 RID: 3729
	public class ThoughtWorker_Affair : ThoughtWorker
	{
		// Token: 0x06005371 RID: 21361 RVA: 0x001C0ABC File Offset: 0x001BECBC
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			if (!p.relations.DirectRelationExists(PawnRelationDefOf.Spouse, otherPawn))
			{
				return false;
			}
			List<DirectPawnRelation> directRelations = otherPawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].otherPawn != p && !directRelations[i].otherPawn.Dead && (directRelations[i].def == PawnRelationDefOf.Lover || directRelations[i].def == PawnRelationDefOf.Fiance))
				{
					return true;
				}
			}
			return false;
		}
	}
}
