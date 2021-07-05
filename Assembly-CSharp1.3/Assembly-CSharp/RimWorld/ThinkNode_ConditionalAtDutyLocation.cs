using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200091D RID: 2333
	public class ThinkNode_ConditionalAtDutyLocation : ThinkNode_Conditional
	{
		// Token: 0x06003C6E RID: 15470 RVA: 0x0014F6E1 File Offset: 0x0014D8E1
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.Position == pawn.mindState.duty.focus.Cell;
		}
	}
}
