using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000792 RID: 1938
	public class JobGiver_Blind : ThinkNode_JobGiver
	{
		// Token: 0x0600351C RID: 13596 RVA: 0x0012C970 File Offset: 0x0012AB70
		protected override Job TryGiveJob(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation;
			if (lord == null || (lordJob_Ritual_Mutilation = (lord.LordJob as LordJob_Ritual_Mutilation)) == null)
			{
				return null;
			}
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if (lordJob_Ritual_Mutilation.mutilatedPawns.Contains(pawn2) || !pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Blind, pawn2, pawn.mindState.duty.focus);
		}
	}
}
