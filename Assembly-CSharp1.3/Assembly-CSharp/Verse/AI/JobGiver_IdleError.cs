using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000636 RID: 1590
	public class JobGiver_IdleError : ThinkNode_JobGiver
	{
		// Token: 0x06002D65 RID: 11621 RVA: 0x0010FDC0 File Offset: 0x0010DFC0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.ErrorOnce(pawn + " issued IdleError wait job. The behavior tree should never get here.", 532983);
			Job job = JobMaker.MakeJob(JobDefOf.Wait);
			job.expiryInterval = 100;
			return job;
		}

		// Token: 0x04001BD2 RID: 7122
		private const int WaitTime = 100;
	}
}
