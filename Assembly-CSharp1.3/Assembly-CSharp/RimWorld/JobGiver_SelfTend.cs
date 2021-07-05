using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D2 RID: 2002
	public class JobGiver_SelfTend : ThinkNode_JobGiver
	{
		// Token: 0x060035DC RID: 13788 RVA: 0x00131178 File Offset: 0x0012F378
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
