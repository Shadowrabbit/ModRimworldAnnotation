using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200091B RID: 2331
	public class ThinkNode_ConditionalHasDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x06003C6A RID: 15466 RVA: 0x0014F68D File Offset: 0x0014D88D
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid;
		}
	}
}
