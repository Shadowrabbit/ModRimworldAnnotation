using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007AC RID: 1964
	public class JobGiver_GotoTravelDestination : ThinkNode_JobGiver
	{
		// Token: 0x06003566 RID: 13670 RVA: 0x0012DF5C File Offset: 0x0012C15C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GotoTravelDestination jobGiver_GotoTravelDestination = (JobGiver_GotoTravelDestination)base.DeepCopy(resolve);
			jobGiver_GotoTravelDestination.locomotionUrgency = this.locomotionUrgency;
			jobGiver_GotoTravelDestination.maxDanger = this.maxDanger;
			jobGiver_GotoTravelDestination.jobMaxDuration = this.jobMaxDuration;
			jobGiver_GotoTravelDestination.exactCell = this.exactCell;
			jobGiver_GotoTravelDestination.ritualTagOnArrival = this.ritualTagOnArrival;
			return jobGiver_GotoTravelDestination;
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x0012DFB4 File Offset: 0x0012C1B4
		protected override Job TryGiveJob(Pawn pawn)
		{
			pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
			if (pawn.mindState.nextMoveOrderIsWait && !this.exactCell)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
				job.expiryInterval = this.WaitTicks.RandomInRange;
				return job;
			}
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			if (!pawn.CanReach(cell, PathEndMode.OnCell, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			if (this.exactCell && pawn.Position == cell)
			{
				return null;
			}
			IntVec3 c = cell;
			if (!this.exactCell)
			{
				c = CellFinder.RandomClosewalkCellNear(cell, pawn.Map, 6, null);
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.Goto, c);
			job2.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job2.expiryInterval = this.jobMaxDuration;
			job2.ritualTag = this.ritualTagOnArrival;
			return job2;
		}

		// Token: 0x04001E90 RID: 7824
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x04001E91 RID: 7825
		protected Danger maxDanger = Danger.Some;

		// Token: 0x04001E92 RID: 7826
		protected int jobMaxDuration = 999999;

		// Token: 0x04001E93 RID: 7827
		protected bool exactCell;

		// Token: 0x04001E94 RID: 7828
		protected string ritualTagOnArrival;

		// Token: 0x04001E95 RID: 7829
		private IntRange WaitTicks = new IntRange(30, 80);
	}
}
