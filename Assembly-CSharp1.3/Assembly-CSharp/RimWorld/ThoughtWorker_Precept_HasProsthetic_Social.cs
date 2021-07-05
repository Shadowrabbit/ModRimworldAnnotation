using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000951 RID: 2385
	public class ThoughtWorker_Precept_HasProsthetic_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003D00 RID: 15616 RVA: 0x00150DC1 File Offset: 0x0014EFC1
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_HasProsthetic.HasProsthetic(otherPawn);
		}
	}
}
