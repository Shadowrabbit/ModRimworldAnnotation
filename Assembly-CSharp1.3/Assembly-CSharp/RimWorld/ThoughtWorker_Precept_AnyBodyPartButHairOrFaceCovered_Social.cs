using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000945 RID: 2373
	public class ThoughtWorker_Precept_AnyBodyPartButHairOrFaceCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003CE2 RID: 15586 RVA: 0x001508C9 File Offset: 0x0014EAC9
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_AnyBodyPartButHairOrFaceCovered.HasCoveredBodyPartsButHairOrFace(otherPawn);
		}
	}
}
