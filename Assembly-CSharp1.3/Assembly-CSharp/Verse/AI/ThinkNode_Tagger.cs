using System;

namespace Verse.AI
{
	// Token: 0x0200062B RID: 1579
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		// Token: 0x06002D47 RID: 11591 RVA: 0x0010F7ED File Offset: 0x0010D9ED
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x0010F808 File Offset: 0x0010DA08
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
			Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode());
			return 0f;
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x0010F864 File Offset: 0x0010DA64
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result = base.TryIssueJobPackage(pawn, jobParams);
			if (result.IsValid && result.Tag == null)
			{
				result = new ThinkResult(result.Job, result.SourceNode, new JobTag?(this.tagToGive), false);
			}
			return result;
		}

		// Token: 0x04001BC5 RID: 7109
		private JobTag tagToGive;
	}
}
