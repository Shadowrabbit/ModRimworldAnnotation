using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094F RID: 2383
	public class ThoughtWorker_Precept_GroinChestHairOrFaceUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003CFB RID: 15611 RVA: 0x00150D60 File Offset: 0x0014EF60
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinChestHairOrFaceUncovered.HasUncoveredGroinChestHairOrFace(otherPawn);
		}
	}
}
