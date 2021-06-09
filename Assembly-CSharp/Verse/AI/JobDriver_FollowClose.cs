using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200096E RID: 2414
	public class JobDriver_FollowClose : JobDriver
	{
		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06003B14 RID: 15124 RVA: 0x0016B214 File Offset: 0x00169414
		private Pawn Followee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06003B15 RID: 15125 RVA: 0x0002D5B7 File Offset: 0x0002B7B7
		private bool CurrentlyWalkingToFollowee
		{
			get
			{
				return this.pawn.pather.Moving && this.pawn.pather.Destination == this.Followee;
			}
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x0016CE68 File Offset: 0x0016B068
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.job.followRadius <= 0f)
			{
				Log.Error("Follow radius is <= 0. pawn=" + this.pawn.ToStringSafe<Pawn>(), false);
				this.job.followRadius = 10f;
			}
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x0002D5ED File Offset: 0x0002B7ED
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				tickAction = delegate()
				{
					Pawn followee = this.Followee;
					float followRadius = this.job.followRadius;
					if (!this.pawn.pather.Moving || this.pawn.IsHashIntervalTick(30))
					{
						bool flag = false;
						if (this.CurrentlyWalkingToFollowee)
						{
							if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, followRadius))
							{
								flag = true;
							}
						}
						else
						{
							float radius = followRadius * 1.2f;
							if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, radius))
							{
								flag = true;
							}
							else
							{
								if (!this.pawn.CanReach(followee, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
								{
									base.EndJobWith(JobCondition.Incompletable);
									return;
								}
								this.pawn.pather.StartPath(followee, PathEndMode.Touch);
								this.locomotionUrgencySameAs = null;
							}
						}
						if (flag)
						{
							if (JobDriver_FollowClose.NearDestinationOrNotMoving(this.pawn, followee, followRadius))
							{
								base.EndJobWith(JobCondition.Succeeded);
								return;
							}
							IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
							if (!this.pawn.pather.Moving || this.pawn.pather.Destination.HasThing || !this.pawn.pather.Destination.Cell.InHorDistOf(lastPassableCellInPath, followRadius))
							{
								IntVec3 intVec = CellFinder.RandomClosewalkCellNear(lastPassableCellInPath, base.Map, Mathf.FloorToInt(followRadius), null);
								if (intVec == this.pawn.Position)
								{
									base.EndJobWith(JobCondition.Succeeded);
									return;
								}
								if (intVec.IsValid && this.pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
								{
									this.pawn.pather.StartPath(intVec, PathEndMode.OnCell);
									this.locomotionUrgencySameAs = followee;
									return;
								}
								base.EndJobWith(JobCondition.Incompletable);
								return;
							}
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x0002D573 File Offset: 0x0002B773
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x0016CEB8 File Offset: 0x0016B0B8
		public static bool FarEnoughAndPossibleToStartJob(Pawn follower, Pawn followee, float radius)
		{
			if (radius <= 0f)
			{
				string text = "Checking follow job with radius <= 0. pawn=" + follower.ToStringSafe<Pawn>();
				if (follower.mindState != null && follower.mindState.duty != null)
				{
					text = text + " duty=" + follower.mindState.duty.def;
				}
				Log.ErrorOnce(text, follower.thingIDNumber ^ 843254009, false);
				return false;
			}
			if (!follower.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return false;
			}
			float radius2 = radius * 1.2f;
			return !JobDriver_FollowClose.NearFollowee(follower, followee, radius2) || (!JobDriver_FollowClose.NearDestinationOrNotMoving(follower, followee, radius2) && follower.CanReach(followee.pather.LastPassableCellInPath, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn));
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x0016CF74 File Offset: 0x0016B174
		private static bool NearFollowee(Pawn follower, Pawn followee, float radius)
		{
			return follower.Position.AdjacentTo8WayOrInside(followee.Position) || (follower.Position.InHorDistOf(followee.Position, radius) && GenSight.LineOfSight(follower.Position, followee.Position, follower.Map, false, null, 0, 0));
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x0016CFCC File Offset: 0x0016B1CC
		private static bool NearDestinationOrNotMoving(Pawn follower, Pawn followee, float radius)
		{
			if (!followee.pather.Moving)
			{
				return true;
			}
			IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
			return !lastPassableCellInPath.IsValid || follower.Position.AdjacentTo8WayOrInside(lastPassableCellInPath) || follower.Position.InHorDistOf(lastPassableCellInPath, radius);
		}

		// Token: 0x0400291E RID: 10526
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x0400291F RID: 10527
		private const int CheckPathIntervalTicks = 30;
	}
}
