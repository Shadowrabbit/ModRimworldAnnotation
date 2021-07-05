using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007FD RID: 2045
	public class JobGiver_SocialFighting : ThinkNode_JobGiver
	{
		// Token: 0x06003699 RID: 13977 RVA: 0x00135764 File Offset: 0x00133964
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			Pawn otherPawn = ((MentalState_SocialFighting)pawn.MentalState).otherPawn;
			Verb verbToUse;
			if (!InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.SocialFight, otherPawn);
			job.maxNumMeleeAttacks = 1;
			job.verbToUse = verbToUse;
			return job;
		}
	}
}
