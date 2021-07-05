using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007B9 RID: 1977
	public class JobGiver_TakeWoundedGuest : ThinkNode_JobGiver
	{
		// Token: 0x0600358E RID: 13710 RVA: 0x0012EBF0 File Offset: 0x0012CDF0
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				return null;
			}
			Pawn pawn2 = KidnapAIUtility.ReachableWoundedGuest(pawn);
			if (pawn2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Kidnap);
			job.targetA = pawn2;
			job.targetB = c;
			job.count = 1;
			return job;
		}
	}
}
