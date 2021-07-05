using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000941 RID: 2369
	public class ThoughtWorker_Precept_AnyBodyPartCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003CD8 RID: 15576 RVA: 0x001506A6 File Offset: 0x0014E8A6
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_AnyBodyPartCovered.HasUnnecessarilyCoveredBodyParts(otherPawn);
		}
	}
}
