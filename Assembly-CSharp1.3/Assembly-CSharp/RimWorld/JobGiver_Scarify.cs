using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A4 RID: 1956
	public class JobGiver_Scarify : ThinkNode_JobGiver
	{
		// Token: 0x0600354E RID: 13646 RVA: 0x0012D8D4 File Offset: 0x0012BAD4
		protected override Job TryGiveJob(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation;
			if (lord == null || (lordJob_Ritual_Mutilation = (lord.LordJob as LordJob_Ritual_Mutilation)) == null)
			{
				return null;
			}
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if (lordJob_Ritual_Mutilation.mutilatedPawns.Contains(pawn2) || !pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, false) || !JobDriver_Scarify.AvailableOnNow(pawn2, null))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Scarify, pawn2, pawn.mindState.duty.focus);
		}
	}
}
