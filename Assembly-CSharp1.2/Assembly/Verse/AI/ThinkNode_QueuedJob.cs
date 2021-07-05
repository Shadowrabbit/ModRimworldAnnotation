using System;

namespace Verse.AI
{
	// Token: 0x02000A9D RID: 2717
	public class ThinkNode_QueuedJob : ThinkNode
	{
		// Token: 0x0600407A RID: 16506 RVA: 0x000303DC File Offset: 0x0002E5DC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_QueuedJob thinkNode_QueuedJob = (ThinkNode_QueuedJob)base.DeepCopy(resolve);
			thinkNode_QueuedJob.inBedOnly = this.inBedOnly;
			return thinkNode_QueuedJob;
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x00182B14 File Offset: 0x00180D14
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			JobQueue jobQueue = pawn.jobs.jobQueue;
			if (pawn.Downed || jobQueue.AnyCanBeginNow(pawn, this.inBedOnly))
			{
				while (jobQueue.Count > 0 && !jobQueue.Peek().job.CanBeginNow(pawn, this.inBedOnly))
				{
					QueuedJob queuedJob = jobQueue.Dequeue();
					pawn.ClearReservationsForJob(queuedJob.job);
					if (pawn.jobs.debugLog)
					{
						pawn.jobs.DebugLogEvent("   Throwing away queued job that I cannot begin now: " + queuedJob.job);
					}
				}
			}
			if (jobQueue.Count > 0 && jobQueue.Peek().job.CanBeginNow(pawn, this.inBedOnly))
			{
				QueuedJob queuedJob2 = jobQueue.Dequeue();
				if (pawn.jobs.debugLog)
				{
					pawn.jobs.DebugLogEvent("   Returning queued job: " + queuedJob2.job);
				}
				return new ThinkResult(queuedJob2.job, this, queuedJob2.tag, true);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04002C62 RID: 11362
		public bool inBedOnly;
	}
}
