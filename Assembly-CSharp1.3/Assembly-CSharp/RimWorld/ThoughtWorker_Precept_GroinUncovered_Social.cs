using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000949 RID: 2377
	public class ThoughtWorker_Precept_GroinUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003CEC RID: 15596 RVA: 0x001509F8 File Offset: 0x0014EBF8
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinUncovered.HasUncoveredGroin(otherPawn);
		}
	}
}
