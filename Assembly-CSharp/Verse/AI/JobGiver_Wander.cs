using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A9E RID: 2718
	public abstract class JobGiver_Wander : ThinkNode_JobGiver
	{
		// Token: 0x0600407D RID: 16509 RVA: 0x00182C10 File Offset: 0x00180E10
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Wander jobGiver_Wander = (JobGiver_Wander)base.DeepCopy(resolve);
			jobGiver_Wander.wanderRadius = this.wanderRadius;
			jobGiver_Wander.wanderDestValidator = this.wanderDestValidator;
			jobGiver_Wander.ticksBetweenWandersRange = this.ticksBetweenWandersRange;
			jobGiver_Wander.locomotionUrgency = this.locomotionUrgency;
			jobGiver_Wander.maxDanger = this.maxDanger;
			jobGiver_Wander.expiryInterval = this.expiryInterval;
			return jobGiver_Wander;
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x00182C74 File Offset: 0x00180E74
		protected override Job TryGiveJob(Pawn pawn)
		{
			bool flag = pawn.CurJob != null && pawn.CurJob.def == JobDefOf.GotoWander;
			bool nextMoveOrderIsWait = pawn.mindState.nextMoveOrderIsWait;
			if (!flag)
			{
				pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
			}
			if (nextMoveOrderIsWait && !flag)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
				job.expiryInterval = this.ticksBetweenWandersRange.RandomInRange;
				return job;
			}
			IntVec3 exactWanderDest = this.GetExactWanderDest(pawn);
			if (!exactWanderDest.IsValid)
			{
				pawn.mindState.nextMoveOrderIsWait = false;
				return null;
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.GotoWander, exactWanderDest);
			job2.locomotionUrgency = this.locomotionUrgency;
			job2.expiryInterval = this.expiryInterval;
			job2.checkOverrideOnExpire = true;
			return job2;
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x00182D38 File Offset: 0x00180F38
		protected virtual IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 wanderRoot = this.GetWanderRoot(pawn);
			PawnDuty duty = pawn.mindState.duty;
			if (duty != null && duty.wanderRadius != null)
			{
				this.wanderRadius = duty.wanderRadius.Value;
			}
			return RCellFinder.RandomWanderDestFor(pawn, wanderRoot, this.wanderRadius, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger));
		}

		// Token: 0x06004080 RID: 16512
		protected abstract IntVec3 GetWanderRoot(Pawn pawn);

		// Token: 0x04002C63 RID: 11363
		protected float wanderRadius;

		// Token: 0x04002C64 RID: 11364
		protected Func<Pawn, IntVec3, IntVec3, bool> wanderDestValidator;

		// Token: 0x04002C65 RID: 11365
		protected IntRange ticksBetweenWandersRange = new IntRange(20, 100);

		// Token: 0x04002C66 RID: 11366
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x04002C67 RID: 11367
		protected Danger maxDanger = Danger.None;

		// Token: 0x04002C68 RID: 11368
		protected int expiryInterval = -1;
	}
}
