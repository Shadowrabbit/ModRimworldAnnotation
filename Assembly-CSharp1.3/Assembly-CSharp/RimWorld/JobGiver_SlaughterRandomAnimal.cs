using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007FC RID: 2044
	public class JobGiver_SlaughterRandomAnimal : ThinkNode_JobGiver
	{
		// Token: 0x06003697 RID: 13975 RVA: 0x00135704 File Offset: 0x00133904
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_Slaughterer mentalState_Slaughterer = pawn.MentalState as MentalState_Slaughterer;
			if (mentalState_Slaughterer != null && mentalState_Slaughterer.SlaughteredRecently)
			{
				return null;
			}
			Pawn pawn2 = SlaughtererMentalStateUtility.FindAnimal(pawn);
			if (pawn2 == null || !pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Slaughter, pawn2);
			job.ignoreDesignations = true;
			return job;
		}
	}
}
