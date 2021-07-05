using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000796 RID: 1942
	public class JobGiver_DeliverPawnToCell : JobGiver_GotoTravelDestination
	{
		// Token: 0x06003525 RID: 13605 RVA: 0x0012CBD3 File Offset: 0x0012ADD3
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DeliverPawnToCell jobGiver_DeliverPawnToCell = (JobGiver_DeliverPawnToCell)base.DeepCopy(resolve);
			jobGiver_DeliverPawnToCell.addArrivalTagIfTargetIsDead = this.addArrivalTagIfTargetIsDead;
			return jobGiver_DeliverPawnToCell;
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x0012CBF0 File Offset: 0x0012ADF0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if ((this.addArrivalTagIfTargetIsDead && pawn2.Dead) || pawn2.Position == pawn.mindState.duty.focus.Cell)
			{
				Lord lord = pawn2.GetLord();
				LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
				if (lordJob_Ritual != null && !lordJob_Ritual.PawnTagSet(pawn2, "Arrived"))
				{
					lordJob_Ritual.AddTagForPawn(pawn2, "Arrived");
				}
				Lord lord2 = pawn.GetLord();
				lordJob_Ritual = (((lord2 != null) ? lord2.LordJob : null) as LordJob_Ritual);
				if (lordJob_Ritual != null && !lordJob_Ritual.PawnTagSet(pawn, "Arrived"))
				{
					lordJob_Ritual.AddTagForPawn(pawn, "Arrived");
				}
				return null;
			}
			if (!pawn.CanReach(pawn2, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DeliverToCell, pawn2, pawn.mindState.duty.focus, pawn.mindState.duty.focusThird);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			job.count = 1;
			job.ritualTag = "Arrived";
			return job;
		}

		// Token: 0x04001E7A RID: 7802
		public bool addArrivalTagIfTargetIsDead;
	}
}
