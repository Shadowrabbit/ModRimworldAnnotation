using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200079F RID: 1951
	public class JobGiver_GotoAndStandSociallyActive : ThinkNode_JobGiver
	{
		// Token: 0x06003540 RID: 13632 RVA: 0x0012D504 File Offset: 0x0012B704
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 dest = this.GetDest(pawn);
			Job job = JobMaker.MakeJob(JobDefOf.GotoAndBeSociallyActive, dest, dest + pawn.Rotation.FacingCell);
			job.locomotionUrgency = this.locomotionUrgency;
			job.expiryInterval = this.expiryInterval;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x0012D564 File Offset: 0x0012B764
		public IntVec3 GetDest(Pawn pawn)
		{
			Predicate<IntVec3> validatorRelaxed = (IntVec3 x) => this.allowUnroofed || !x.Roofed(pawn.Map);
			Predicate<IntVec3> cellValidator = delegate(IntVec3 x)
			{
				if (!RitualUility.GoodSpectateCellForRitual(x, pawn, pawn.Map))
				{
					return false;
				}
				if (this.minDistanceToOtherReservedCell > 0)
				{
					foreach (IntVec3 c in GenRadial.RadialCellsAround(x, (float)this.minDistanceToOtherReservedCell, true))
					{
						if (!pawn.CanReserveAndReach(c, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
						{
							return false;
						}
					}
				}
				return validatorRelaxed(x);
			};
			IntVec3 result;
			if (this.desiredRadius > 0f && GatheringsUtility.TryFindRandomCellInGatheringAreaWithRadius(pawn, this.desiredRadius, cellValidator, out result))
			{
				return result;
			}
			if (GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, validatorRelaxed, out result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x0012D5E8 File Offset: 0x0012B7E8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GotoAndStandSociallyActive jobGiver_GotoAndStandSociallyActive = (JobGiver_GotoAndStandSociallyActive)base.DeepCopy(resolve);
			jobGiver_GotoAndStandSociallyActive.locomotionUrgency = this.locomotionUrgency;
			jobGiver_GotoAndStandSociallyActive.allowUnroofed = this.allowUnroofed;
			jobGiver_GotoAndStandSociallyActive.desiredRadius = this.desiredRadius;
			jobGiver_GotoAndStandSociallyActive.minDistanceToOtherReservedCell = this.minDistanceToOtherReservedCell;
			jobGiver_GotoAndStandSociallyActive.expiryInterval = this.expiryInterval;
			return jobGiver_GotoAndStandSociallyActive;
		}

		// Token: 0x04001E83 RID: 7811
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x04001E84 RID: 7812
		public bool allowUnroofed = true;

		// Token: 0x04001E85 RID: 7813
		protected int expiryInterval = -1;

		// Token: 0x04001E86 RID: 7814
		public float desiredRadius = -1f;

		// Token: 0x04001E87 RID: 7815
		public int minDistanceToOtherReservedCell = -1;
	}
}
