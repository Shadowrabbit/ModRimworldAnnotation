using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000789 RID: 1929
	public class JobGiver_PrepareCaravan_GatherItems : ThinkNode_JobGiver
	{
		// Token: 0x060034F8 RID: 13560 RVA: 0x0012C0B4 File Offset: 0x0012A2B4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(pawn, lord);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.PrepareCaravan_GatherItems, thing);
			job.lord = lord;
			return job;
		}
	}
}
