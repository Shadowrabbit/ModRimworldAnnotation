using System;

namespace Verse.AI
{
	// Token: 0x0200063C RID: 1596
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		// Token: 0x06002D85 RID: 11653
		protected abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06002D86 RID: 11654 RVA: 0x00110204 File Offset: 0x0010E404
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
