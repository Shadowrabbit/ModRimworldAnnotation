using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F7 RID: 2039
	public class JobGiver_InduceSlaveToRebel : ThinkNode_JobGiver
	{
		// Token: 0x0600368A RID: 13962 RVA: 0x00135360 File Offset: 0x00133560
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = SlaveRebellionUtility.FindSlaveForRebellion(pawn);
			if (pawn2 == null || !pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InduceSlaveToRebel, pawn2);
			job.interaction = InteractionDefOf.SparkSlaveRebellion;
			return job;
		}
	}
}
