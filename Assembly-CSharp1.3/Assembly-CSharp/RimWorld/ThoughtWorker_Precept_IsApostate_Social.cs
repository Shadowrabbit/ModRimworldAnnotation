using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000961 RID: 2401
	public class ThoughtWorker_Precept_IsApostate_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003D28 RID: 15656 RVA: 0x00151576 File Offset: 0x0014F776
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return otherPawn.ideo != null && otherPawn.Ideo != p.Ideo && otherPawn.ideo.PreviousIdeos.Contains(p.Ideo);
		}
	}
}
