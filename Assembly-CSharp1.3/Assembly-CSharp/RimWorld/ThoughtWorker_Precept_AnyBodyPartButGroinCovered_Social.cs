using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000943 RID: 2371
	public class ThoughtWorker_Precept_AnyBodyPartButGroinCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003CDD RID: 15581 RVA: 0x001507C9 File Offset: 0x0014E9C9
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_AnyBodyPartButGroinCovered.HasCoveredBodyPartsButGroin(otherPawn);
		}
	}
}
