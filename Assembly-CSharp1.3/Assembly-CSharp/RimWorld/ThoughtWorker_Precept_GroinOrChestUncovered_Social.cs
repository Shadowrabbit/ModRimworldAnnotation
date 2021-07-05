using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094B RID: 2379
	public class ThoughtWorker_Precept_GroinOrChestUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003CF1 RID: 15601 RVA: 0x00150ACF File Offset: 0x0014ECCF
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinOrChestUncovered.HasUncoveredGroinOrChest(otherPawn);
		}
	}
}
