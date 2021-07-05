using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000795 RID: 1941
	public class JobGiver_DeliverPawnToAltar : JobGiver_GotoTravelDestination
	{
		// Token: 0x06003523 RID: 13603 RVA: 0x0012CB20 File Offset: 0x0012AD20
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModLister.CheckIdeology("Deliver to altar"))
			{
				return null;
			}
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if (!pawn.CanReach(pawn2.Position, PathEndMode.OnCell, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DeliverToAltar, pawn2, pawn.mindState.duty.focus, pawn.mindState.duty.focusThird);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			job.count = 1;
			return job;
		}
	}
}
