using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200077D RID: 1917
	public abstract class JobGiver_AIFollowPawn : ThinkNode_JobGiver
	{
		// Token: 0x060034CB RID: 13515
		protected abstract Pawn GetFollowee(Pawn pawn);

		// Token: 0x060034CC RID: 13516
		protected abstract float GetRadius(Pawn pawn);

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x060034CD RID: 13517 RVA: 0x0012B0FE File Offset: 0x001292FE
		protected virtual int FollowJobExpireInterval
		{
			get
			{
				return 140;
			}
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x0012B108 File Offset: 0x00129308
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn followee = this.GetFollowee(pawn);
			if (followee == null)
			{
				Log.Error(base.GetType() + " has null followee. pawn=" + pawn.ToStringSafe<Pawn>());
				return null;
			}
			if (!followee.Spawned || !pawn.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			float radius = this.GetRadius(pawn);
			if (!JobDriver_FollowClose.FarEnoughAndPossibleToStartJob(pawn, followee, radius))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.FollowClose, followee);
			job.expiryInterval = this.FollowJobExpireInterval;
			job.checkOverrideOnExpire = true;
			job.followRadius = radius;
			return job;
		}
	}
}
