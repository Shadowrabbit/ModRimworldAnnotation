using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D32 RID: 3378
	public class JobGiver_Tantrum : ThinkNode_JobGiver
	{
		// Token: 0x06004D42 RID: 19778 RVA: 0x001ADA80 File Offset: 0x001ABC80
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_Tantrum mentalState_Tantrum = pawn.MentalState as MentalState_Tantrum;
			if (mentalState_Tantrum == null || mentalState_Tantrum.target == null || !pawn.CanReach(mentalState_Tantrum.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Verb verbToUse = null;
			Pawn pawn2 = mentalState_Tantrum.target as Pawn;
			if (pawn2 != null)
			{
				if (pawn2.Downed)
				{
					return null;
				}
				if (!InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
				{
					return null;
				}
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, mentalState_Tantrum.target);
			job.maxNumMeleeAttacks = 1;
			job.verbToUse = verbToUse;
			return job;
		}
	}
}
