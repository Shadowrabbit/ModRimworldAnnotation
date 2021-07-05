using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007BF RID: 1983
	public class JobGiver_FollowRoper : ThinkNode_JobGiver
	{
		// Token: 0x0600359B RID: 13723 RVA: 0x0012EF88 File Offset: 0x0012D188
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2;
			if (pawn == null)
			{
				pawn2 = null;
			}
			else
			{
				Pawn_RopeTracker roping = pawn.roping;
				pawn2 = ((roping != null) ? roping.RopedByPawn : null);
			}
			Pawn pawn3 = pawn2;
			if (pawn3 == null)
			{
				return null;
			}
			if (!(pawn3.jobs.curDriver is JobDriver_RopeToDestination))
			{
				return null;
			}
			if (!pawn3.CurJob.GetTarget(TargetIndex.B).Cell.IsValid)
			{
				return null;
			}
			if (!pawn3.Spawned || !pawn.CanReach(pawn3, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.FollowRoper, pawn3);
			job.expiryInterval = 140;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x04001EA7 RID: 7847
		private const int FollowJobExpireInterval = 140;
	}
}
