using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007AB RID: 1963
	public class JobGiver_ForceSleepNow : ThinkNode_JobGiver
	{
		// Token: 0x06003564 RID: 13668 RVA: 0x0012DF3B File Offset: 0x0012C13B
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position);
			job.forceSleep = true;
			return job;
		}
	}
}
