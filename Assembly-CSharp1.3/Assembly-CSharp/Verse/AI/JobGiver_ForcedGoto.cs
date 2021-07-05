using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000633 RID: 1587
	public class JobGiver_ForcedGoto : ThinkNode_JobGiver
	{
		// Token: 0x06002D5E RID: 11614 RVA: 0x0010FCB4 File Offset: 0x0010DEB4
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 forcedGotoPosition = pawn.mindState.forcedGotoPosition;
			if (!forcedGotoPosition.IsValid)
			{
				return null;
			}
			if (!pawn.CanReach(forcedGotoPosition, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Goto, forcedGotoPosition);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			return job;
		}
	}
}
