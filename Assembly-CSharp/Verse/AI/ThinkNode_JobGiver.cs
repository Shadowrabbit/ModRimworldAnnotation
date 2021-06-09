using System;

namespace Verse.AI
{
	// Token: 0x02000A9C RID: 2716
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		// Token: 0x06004077 RID: 16503
		protected abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06004078 RID: 16504 RVA: 0x00182AE4 File Offset: 0x00180CE4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Job job = this.TryGiveJob(pawn);
			if (job == null)
			{
				return ThinkResult.NoJob;
			}
			return new ThinkResult(job, this, null, false);
		}
	}
}
