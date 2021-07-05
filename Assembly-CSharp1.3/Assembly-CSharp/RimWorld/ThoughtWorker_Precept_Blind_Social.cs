using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096D RID: 2413
	public class ThoughtWorker_Precept_Blind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003D52 RID: 15698 RVA: 0x00151CD0 File Offset: 0x0014FED0
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_Blind.IsBlind(otherPawn);
		}
	}
}
