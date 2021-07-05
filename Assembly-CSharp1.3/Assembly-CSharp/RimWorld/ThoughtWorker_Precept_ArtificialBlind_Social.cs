using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096F RID: 2415
	public class ThoughtWorker_Precept_ArtificialBlind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003D56 RID: 15702 RVA: 0x00151CEA File Offset: 0x0014FEEA
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_ArtificialBlind.IsArtificiallyBlind(otherPawn);
		}
	}
}
