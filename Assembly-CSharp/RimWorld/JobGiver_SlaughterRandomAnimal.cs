using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D30 RID: 3376
	public class JobGiver_SlaughterRandomAnimal : ThinkNode_JobGiver
	{
		// Token: 0x06004D3E RID: 19774 RVA: 0x001AD9C0 File Offset: 0x001ABBC0
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
