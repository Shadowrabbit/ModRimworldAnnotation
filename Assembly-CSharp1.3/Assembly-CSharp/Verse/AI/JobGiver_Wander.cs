using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200063E RID: 1598
	public abstract class JobGiver_Wander : ThinkNode_JobGiver
	{
		// Token: 0x06002D8B RID: 11659 RVA: 0x0011034C File Offset: 0x0010E54C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Wander jobGiver_Wander = (JobGiver_Wander)base.DeepCopy(resolve);
			jobGiver_Wander.wanderRadius = this.wanderRadius;
			jobGiver_Wander.wanderDestValidator = this.wanderDestValidator;
			jobGiver_Wander.ticksBetweenWandersRange = this.ticksBetweenWandersRange;
			jobGiver_Wander.locomotionUrgency = this.locomotionUrgency;
			jobGiver_Wander.locomotionUrgencyOutsideRadius = this.locomotionUrgencyOutsideRadius;
			jobGiver_Wander.maxDanger = this.maxDanger;
			jobGiver_Wander.expiryInterval = this.expiryInterval;
			return jobGiver_Wander;
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x001103BC File Offset: 0x0010E5BC
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
			LocomotionUrgency value = this.locomotionUrgency;
			if (this.locomotionUrgencyOutsideRadius != null && !pawn.Position.InHorDistOf(this.GetWanderRoot(pawn), this.wanderRadius))
			{
				value = this.locomotionUrgencyOutsideRadius.Value;
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.GotoWander, exactWanderDest);
			job2.locomotionUrgency = value;
			job2.expiryInterval = this.expiryInterval;
			job2.checkOverrideOnExpire = true;
			return job2;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x001104B8 File Offset: 0x0010E6B8
		protected virtual IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 wanderRoot = this.GetWanderRoot(pawn);
			float value = this.wanderRadius;
			PawnDuty duty = pawn.mindState.duty;
			if (duty != null && duty.wanderRadius != null)
			{
				value = duty.wanderRadius.Value;
			}
			return RCellFinder.RandomWanderDestFor(pawn, wanderRoot, value, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger));
		}

		// Token: 0x06002D8E RID: 11662
		protected abstract IntVec3 GetWanderRoot(Pawn pawn);

		// Token: 0x04001BE0 RID: 7136
		protected float wanderRadius;

		// Token: 0x04001BE1 RID: 7137
		protected Func<Pawn, IntVec3, IntVec3, bool> wanderDestValidator;

		// Token: 0x04001BE2 RID: 7138
		protected IntRange ticksBetweenWandersRange = new IntRange(20, 100);

		// Token: 0x04001BE3 RID: 7139
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x04001BE4 RID: 7140
		protected LocomotionUrgency? locomotionUrgencyOutsideRadius;

		// Token: 0x04001BE5 RID: 7141
		protected Danger maxDanger = Danger.None;

		// Token: 0x04001BE6 RID: 7142
		protected int expiryInterval = -1;
	}
}
