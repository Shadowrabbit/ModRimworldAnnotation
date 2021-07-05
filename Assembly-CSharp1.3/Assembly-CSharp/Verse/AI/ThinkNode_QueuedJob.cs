using System;

namespace Verse.AI
{
	// Token: 0x0200063D RID: 1597
	public class ThinkNode_QueuedJob : ThinkNode
	{
		// Token: 0x06002D88 RID: 11656 RVA: 0x00110233 File Offset: 0x0010E433
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_QueuedJob thinkNode_QueuedJob = (ThinkNode_QueuedJob)base.DeepCopy(resolve);
			thinkNode_QueuedJob.inBedOnly = this.inBedOnly;
			return thinkNode_QueuedJob;
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x00110250 File Offset: 0x0010E450
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

		// Token: 0x04001BDF RID: 7135
		public bool inBedOnly;
	}
}
