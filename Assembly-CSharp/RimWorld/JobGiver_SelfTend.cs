using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CEB RID: 3307
	public class JobGiver_SelfTend : ThinkNode_JobGiver
	{
		// Token: 0x06004C19 RID: 19481 RVA: 0x001A8B0C File Offset: 0x001A6D0C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike || !pawn.health.HasHediffsNeedingTend(false) || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || pawn.InAggroMentalState)
			{
				return null;
			}
			if (pawn.IsColonist && pawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.TendPatient, pawn);
			job.endAfterTendedOnce = true;
			return job;
		}
	}
}
