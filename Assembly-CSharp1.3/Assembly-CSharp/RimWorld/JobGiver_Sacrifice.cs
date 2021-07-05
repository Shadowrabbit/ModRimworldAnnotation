using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007A3 RID: 1955
	public class JobGiver_Sacrifice : ThinkNode_JobGiver
	{
		// Token: 0x0600354C RID: 13644 RVA: 0x0012D878 File Offset: 0x0012BA78
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if (!pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Sacrifice, pawn2, pawn.mindState.duty.focus);
		}
	}
}
