using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CCC RID: 3276
	public class JobGiver_TakeWoundedGuest : ThinkNode_JobGiver
	{
		// Token: 0x06004BBD RID: 19389 RVA: 0x001A6B88 File Offset: 0x001A4D88
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
