using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000911 RID: 2321
	public class ThinkNode_ConditionalExitTimedOut : ThinkNode_Conditional
	{
		// Token: 0x06003C55 RID: 15445 RVA: 0x0014F546 File Offset: 0x0014D746
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.exitMapAfterTick >= 0 && Find.TickManager.TicksGame > pawn.mindState.exitMapAfterTick;
		}
	}
}
