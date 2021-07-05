using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000955 RID: 2389
	public class ThoughtWorker_Precept_IdeoDiversity_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003D08 RID: 15624 RVA: 0x00150F5C File Offset: 0x0014F15C
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return p.Faction == otherPawn.Faction && p.Ideo != otherPawn.Ideo;
		}
	}
}
