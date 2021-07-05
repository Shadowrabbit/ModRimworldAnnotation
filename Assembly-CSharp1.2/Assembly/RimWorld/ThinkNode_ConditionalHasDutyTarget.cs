using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E64 RID: 3684
	public class ThinkNode_ConditionalHasDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x060052FE RID: 21246 RVA: 0x00039F88 File Offset: 0x00038188
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid;
		}
	}
}
