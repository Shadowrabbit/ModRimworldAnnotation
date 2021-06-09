using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E65 RID: 3685
	public class ThinkNode_ConditionalHasDutyPawnTarget : ThinkNode_Conditional
	{
		// Token: 0x06005300 RID: 21248 RVA: 0x00039FAE File Offset: 0x000381AE
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.Thing is Pawn;
		}
	}
}
