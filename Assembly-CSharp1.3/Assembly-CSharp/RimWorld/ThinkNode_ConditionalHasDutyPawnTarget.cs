using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200091C RID: 2332
	public class ThinkNode_ConditionalHasDutyPawnTarget : ThinkNode_Conditional
	{
		// Token: 0x06003C6C RID: 15468 RVA: 0x0014F6B3 File Offset: 0x0014D8B3
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.Thing is Pawn;
		}
	}
}
