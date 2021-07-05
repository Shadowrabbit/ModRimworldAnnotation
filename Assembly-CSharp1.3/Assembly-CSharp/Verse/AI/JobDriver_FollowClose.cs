using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000590 RID: 1424
	public class JobDriver_FollowClose : JobDriver
	{
		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x000FC2F8 File Offset: 0x000FA4F8
		private Pawn Followee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x000FC31E File Offset: 0x000FA51E
		private bool CurrentlyWalkingToFollowee
		{
			get
			{
				return this.pawn.pather.Moving && this.pawn.pather.Destination == this.Followee;
			}
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x000FC354 File Offset: 0x000FA554
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.job.followRadius <= 0f)
			{
				Log.Error("Follow radius is <= 0. pawn=" + this.pawn.ToStringSafe<Pawn>());
				this.job.followRadius = 10f;
			}
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x000FC3A3 File Offset: 0x000FA5A3
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
								if (!this.pawn.CanReach(followee, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
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
								if (intVec.IsValid && this.pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
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

		// Token: 0x060029B0 RID: 10672 RVA: 0x000FA4B3 File Offset: 0x000F86B3
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000FC3B4 File Offset: 0x000FA5B4
		public static bool FarEnoughAndPossibleToStartJob(Pawn follower, Pawn followee, float radius)
		{
			if (radius <= 0f)
			{
				string text = "Checking follow job with radius <= 0. pawn=" + follower.ToStringSafe<Pawn>();
				if (follower.mindState != null && follower.mindState.duty != null)
				{
					text = text + " duty=" + follower.mindState.duty.def;
				}
				Log.ErrorOnce(text, follower.thingIDNumber ^ 843254009);
				return false;
			}
			if (!follower.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return false;
			}
			float radius2 = radius * 1.2f;
			return !JobDriver_FollowClose.NearFollowee(follower, followee, radius2) || (!JobDriver_FollowClose.NearDestinationOrNotMoving(follower, followee, radius2) && follower.CanReach(followee.pather.LastPassableCellInPath, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn));
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x000FC470 File Offset: 0x000FA670
		private static bool NearFollowee(Pawn follower, Pawn followee, float radius)
		{
			return follower.Position.AdjacentTo8WayOrInside(followee.Position) || (follower.Position.InHorDistOf(followee.Position, radius) && GenSight.LineOfSight(follower.Position, followee.Position, follower.Map, false, null, 0, 0));
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000FC4C8 File Offset: 0x000FA6C8
		private static bool NearDestinationOrNotMoving(Pawn follower, Pawn followee, float radius)
		{
			if (!followee.pather.Moving)
			{
				return true;
			}
			IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
			return !lastPassableCellInPath.IsValid || follower.Position.AdjacentTo8WayOrInside(lastPassableCellInPath) || follower.Position.InHorDistOf(lastPassableCellInPath, radius);
		}

		// Token: 0x04001A11 RID: 6673
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x04001A12 RID: 6674
		private const int CheckPathIntervalTicks = 30;
	}
}
