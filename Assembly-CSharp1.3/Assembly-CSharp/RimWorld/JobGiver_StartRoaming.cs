using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000767 RID: 1895
	public class JobGiver_StartRoaming : ThinkNode_JobGiver
	{
		// Token: 0x06003459 RID: 13401 RVA: 0x00128C40 File Offset: 0x00126E40
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!MentalStateWorker_Roaming.CanRoamNow(pawn))
			{
				return null;
			}
			if (!pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Roaming, null, false, false, null, false, false, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Wait, 10, false);
		}
	}
}
