using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D2A RID: 3370
	public class JobGiver_InducePrisonerToEscape : ThinkNode_JobGiver
	{
		// Token: 0x06004D2D RID: 19757 RVA: 0x001AD678 File Offset: 0x001AB878
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = JailbreakerMentalStateUtility.FindPrisoner(pawn);
			if (pawn2 == null || !pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InducePrisonerToEscape, pawn2);
			job.interaction = InteractionDefOf.SparkJailbreak;
			return job;
		}
	}
}
