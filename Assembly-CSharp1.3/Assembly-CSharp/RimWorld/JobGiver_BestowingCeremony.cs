using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007BB RID: 1979
	public class JobGiver_BestowingCeremony : ThinkNode_JobGiver
	{
		// Token: 0x06003592 RID: 13714 RVA: 0x0012ECC4 File Offset: 0x0012CEC4
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.BestowingCeremony, duty.focus.Pawn, duty.focusSecond);
		}
	}
}
