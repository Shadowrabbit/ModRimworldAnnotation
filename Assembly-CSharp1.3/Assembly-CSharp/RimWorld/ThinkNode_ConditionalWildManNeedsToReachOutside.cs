using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200092A RID: 2346
	public class ThinkNode_ConditionalWildManNeedsToReachOutside : ThinkNode_Conditional
	{
		// Token: 0x06003C8B RID: 15499 RVA: 0x0014F9FE File Offset: 0x0014DBFE
		protected override bool Satisfied(Pawn pawn)
		{
			return WildManUtility.WildManShouldReachOutsideNow(pawn);
		}
	}
}
