using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A94 RID: 2708
	public class JobGiver_IdleError : ThinkNode_JobGiver
	{
		// Token: 0x06004045 RID: 16453 RVA: 0x000301C3 File Offset: 0x0002E3C3
		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.ErrorOnce(pawn + " issued IdleError wait job. The behavior tree should never get here.", 532983, false);
			Job job = JobMaker.MakeJob(JobDefOf.Wait);
			job.expiryInterval = 100;
			return job;
		}

		// Token: 0x04002C4A RID: 11338
		private const int WaitTime = 100;
	}
}
