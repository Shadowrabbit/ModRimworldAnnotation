using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CBC RID: 3260
	public class JobGiver_ForceSleepNow : ThinkNode_JobGiver
	{
		// Token: 0x06004B8E RID: 19342 RVA: 0x00035D80 File Offset: 0x00033F80
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position);
			job.forceSleep = true;
			return job;
		}
	}
}
