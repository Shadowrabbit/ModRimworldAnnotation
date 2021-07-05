using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094D RID: 2381
	public class ThoughtWorker_Precept_GroinChestOrHairUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003CF6 RID: 15606 RVA: 0x00150BFE File Offset: 0x0014EDFE
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinChestOrHairUncovered.HasUncoveredGroinChestOrHair(otherPawn);
		}
	}
}
