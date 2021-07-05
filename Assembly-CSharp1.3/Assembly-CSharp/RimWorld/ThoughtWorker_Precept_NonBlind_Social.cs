using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000970 RID: 2416
	public class ThoughtWorker_Precept_NonBlind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003D58 RID: 15704 RVA: 0x00151CF7 File Offset: 0x0014FEF7
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_NonBlind.IsNotBlind(otherPawn, true);
		}
	}
}
