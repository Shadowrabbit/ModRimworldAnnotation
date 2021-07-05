using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000953 RID: 2387
	public class ThoughtWorker_Precept_HasNoProsthetic_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003D04 RID: 15620 RVA: 0x00150DDE File Offset: 0x0014EFDE
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return !ThoughtWorker_Precept_HasProsthetic.HasProsthetic(otherPawn);
		}
	}
}
