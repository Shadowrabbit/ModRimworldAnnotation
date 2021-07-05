using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007B0 RID: 1968
	public class JobGiver_LayDownResting : ThinkNode_JobGiver
	{
		// Token: 0x0600356F RID: 13679 RVA: 0x0012E2A8 File Offset: 0x0012C4A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c2;
			if (!GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, (IntVec3 c) => pawn.CanReserveAndReach(c, PathEndMode.Touch, Danger.None, 1, -1, null, false), out c2))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.LayDownResting, c2);
			job.locomotionUrgency = LocomotionUrgency.Amble;
			return job;
		}
	}
}
