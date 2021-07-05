using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000794 RID: 1940
	public class JobGiver_Dance : ThinkNode_JobGiver
	{
		// Token: 0x06003520 RID: 13600 RVA: 0x0012CACB File Offset: 0x0012ACCB
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Dance jobGiver_Dance = (JobGiver_Dance)base.DeepCopy(resolve);
			jobGiver_Dance.ticksRange = this.ticksRange;
			return jobGiver_Dance;
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x0012CAE5 File Offset: 0x0012ACE5
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Dance);
			job.expiryInterval = this.ticksRange.RandomInRange;
			return job;
		}

		// Token: 0x04001E79 RID: 7801
		public IntRange ticksRange = new IntRange(300, 600);
	}
}
