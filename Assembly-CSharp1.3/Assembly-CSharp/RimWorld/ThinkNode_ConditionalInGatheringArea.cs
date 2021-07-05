using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000923 RID: 2339
	public class ThinkNode_ConditionalInGatheringArea : ThinkNode_Conditional
	{
		// Token: 0x06003C7C RID: 15484 RVA: 0x0014F87C File Offset: 0x0014DA7C
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
