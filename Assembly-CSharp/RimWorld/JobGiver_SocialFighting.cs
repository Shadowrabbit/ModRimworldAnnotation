using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D31 RID: 3377
	public class JobGiver_SocialFighting : ThinkNode_JobGiver
	{
		// Token: 0x06004D40 RID: 19776 RVA: 0x001ADA20 File Offset: 0x001ABC20
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
