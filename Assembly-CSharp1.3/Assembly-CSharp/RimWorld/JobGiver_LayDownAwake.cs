using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007AF RID: 1967
	public class JobGiver_LayDownAwake : ThinkNode_JobGiver
	{
		// Token: 0x0600356D RID: 13677 RVA: 0x0012E26C File Offset: 0x0012C46C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.CanReach(pawn.mindState.duty.focusThird, PathEndMode.Touch, Danger.None, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.LayDownAwake, pawn.mindState.duty.focusThird);
		}
	}
}
