using System;

namespace Verse.AI
{
	// Token: 0x02000A88 RID: 2696
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		// Token: 0x06004024 RID: 16420 RVA: 0x000300C2 File Offset: 0x0002E2C2
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x001820F4 File Offset: 0x001802F4
		public override float GetPriority(Pawn pawn)
		{
			if (this.priority >= 0f)
			{
				return this.priority;
			}
			if (this.subNodes.Any<ThinkNode>())
			{
				return this.subNodes[0].GetPriority(pawn);
			}
			Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode(), false);
			return 0f;
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00182154 File Offset: 0x00180354
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result = base.TryIssueJobPackage(pawn, jobParams);
			if (result.IsValid && result.Tag == null)
			{
				result = new ThinkResult(result.Job, result.SourceNode, new JobTag?(this.tagToGive), false);
			}
			return result;
		}

		// Token: 0x04002C3B RID: 11323
		private JobTag tagToGive;
	}
}
