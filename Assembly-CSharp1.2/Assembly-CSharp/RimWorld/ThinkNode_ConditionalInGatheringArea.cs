using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E6C RID: 3692
	public class ThinkNode_ConditionalInGatheringArea : ThinkNode_Conditional
	{
		// Token: 0x06005310 RID: 21264 RVA: 0x001BFD6C File Offset: 0x001BDF6C
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.mindState.duty == null)
			{
				return false;
			}
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return GatheringsUtility.InGatheringArea(pawn.Position, cell, pawn.Map);
		}
	}
}
