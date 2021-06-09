using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C98 RID: 3224
	public abstract class JobGiver_AIFollowPawn : ThinkNode_JobGiver
	{
		// Token: 0x06004B23 RID: 19235
		protected abstract Pawn GetFollowee(Pawn pawn);

		// Token: 0x06004B24 RID: 19236
		protected abstract float GetRadius(Pawn pawn);

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06004B25 RID: 19237 RVA: 0x00035A01 File Offset: 0x00033C01
		protected virtual int FollowJobExpireInterval
		{
			get
			{
				return 140;
			}
		}

		// Token: 0x06004B26 RID: 19238 RVA: 0x001A44C4 File Offset: 0x001A26C4
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn followee = this.GetFollowee(pawn);
			if (followee == null)
			{
				Log.Error(base.GetType() + " has null followee. pawn=" + pawn.ToStringSafe<Pawn>(), false);
				return null;
			}
			if (!followee.Spawned || !pawn.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
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
