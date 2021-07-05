using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F6 RID: 2038
	public class JobGiver_InducePrisonerToEscape : ThinkNode_JobGiver
	{
		// Token: 0x06003688 RID: 13960 RVA: 0x00135318 File Offset: 0x00133518
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = JailbreakerMentalStateUtility.FindPrisoner(pawn);
			if (pawn2 == null || !pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InducePrisonerToEscape, pawn2);
			job.interaction = InteractionDefOf.SparkJailbreak;
			return job;
		}
	}
}
