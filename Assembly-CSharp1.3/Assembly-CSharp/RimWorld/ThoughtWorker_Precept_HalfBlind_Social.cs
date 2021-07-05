using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096E RID: 2414
	public class ThoughtWorker_Precept_HalfBlind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003D54 RID: 15700 RVA: 0x00151CDD File Offset: 0x0014FEDD
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_HalfBlind.IsHalfBlind(otherPawn);
		}
	}
}
