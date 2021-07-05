using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E66 RID: 3686
	public class ThinkNode_ConditionalAtDutyLocation : ThinkNode_Conditional
	{
		// Token: 0x06005302 RID: 21250 RVA: 0x00039FDC File Offset: 0x000381DC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.Position == pawn.mindState.duty.focus.Cell;
		}
	}
}
