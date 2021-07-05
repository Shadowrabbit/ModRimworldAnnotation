using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CBD RID: 3261
	public class JobGiver_GotoTravelDestination : ThinkNode_JobGiver
	{
		// Token: 0x06004B90 RID: 19344 RVA: 0x00035D9E File Offset: 0x00033F9E
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GotoTravelDestination jobGiver_GotoTravelDestination = (JobGiver_GotoTravelDestination)base.DeepCopy(resolve);
			jobGiver_GotoTravelDestination.locomotionUrgency = this.locomotionUrgency;
			jobGiver_GotoTravelDestination.maxDanger = this.maxDanger;
			jobGiver_GotoTravelDestination.jobMaxDuration = this.jobMaxDuration;
			jobGiver_GotoTravelDestination.exactCell = this.exactCell;
			return jobGiver_GotoTravelDestination;
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x001A5FEC File Offset: 0x001A41EC
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
			if (!pawn.CanReach(cell, PathEndMode.OnCell, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, TraverseMode.ByPawn))
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
			return job2;
		}

		// Token: 0x040031DD RID: 12765
		private LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x040031DE RID: 12766
		private Danger maxDanger = Danger.Some;

		// Token: 0x040031DF RID: 12767
		private int jobMaxDuration = 999999;

		// Token: 0x040031E0 RID: 12768
		private bool exactCell;

		// Token: 0x040031E1 RID: 12769
		private IntRange WaitTicks = new IntRange(30, 80);
	}
}
