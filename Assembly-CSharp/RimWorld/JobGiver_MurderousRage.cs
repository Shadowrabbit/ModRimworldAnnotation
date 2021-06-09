using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D2E RID: 3374
	public class JobGiver_MurderousRage : ThinkNode_JobGiver
	{
		// Token: 0x06004D3A RID: 19770 RVA: 0x001AD8FC File Offset: 0x001ABAFC
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_MurderousRage mentalState_MurderousRage = pawn.MentalState as MentalState_MurderousRage;
			if (mentalState_MurderousRage == null || !mentalState_MurderousRage.IsTargetStillValidAndReachable())
			{
				return null;
			}
			Thing spawnedParentOrMe = mentalState_MurderousRage.target.SpawnedParentOrMe;
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, spawnedParentOrMe);
			job.canBash = true;
			job.killIncappedTarget = true;
			if (spawnedParentOrMe != mentalState_MurderousRage.target)
			{
				job.maxNumMeleeAttacks = 2;
			}
			return job;
		}
	}
}
