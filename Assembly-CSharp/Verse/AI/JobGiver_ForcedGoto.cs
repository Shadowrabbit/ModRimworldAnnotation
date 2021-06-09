using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A91 RID: 2705
	public class JobGiver_ForcedGoto : ThinkNode_JobGiver
	{
		// Token: 0x0600403E RID: 16446 RVA: 0x00182524 File Offset: 0x00180724
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 forcedGotoPosition = pawn.mindState.forcedGotoPosition;
			if (!forcedGotoPosition.IsValid)
			{
				return null;
			}
			if (!pawn.CanReach(forcedGotoPosition, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
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
