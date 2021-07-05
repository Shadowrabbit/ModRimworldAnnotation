using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000947 RID: 2375
	public class ThoughtWorker_Precept_FaceCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003CE7 RID: 15591 RVA: 0x00150960 File Offset: 0x0014EB60
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_FaceCovered.HasCoveredFace(otherPawn);
		}
	}
}
