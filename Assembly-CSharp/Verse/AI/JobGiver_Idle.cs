using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A92 RID: 2706
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		// Token: 0x06004040 RID: 16448 RVA: 0x00030175 File Offset: 0x0002E375
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x0003018F File Offset: 0x0002E38F
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait);
			job.expiryInterval = this.ticks;
			return job;
		}

		// Token: 0x04002C49 RID: 11337
		public int ticks = 50;
	}
}
