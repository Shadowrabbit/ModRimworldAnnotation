using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E5A RID: 3674
	public class ThinkNode_ConditionalExitTimedOut : ThinkNode_Conditional
	{
		// Token: 0x060052E9 RID: 21225 RVA: 0x00039E7A File Offset: 0x0003807A
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.exitMapAfterTick >= 0 && Find.TickManager.TicksGame > pawn.mindState.exitMapAfterTick;
		}
	}
}
