using System;

namespace Verse.AI
{
	// Token: 0x02000A83 RID: 2691
	public class ThinkNode_FilterPriority : ThinkNode
	{
		// Token: 0x06004017 RID: 16407 RVA: 0x00030036 File Offset: 0x0002E236
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_FilterPriority thinkNode_FilterPriority = (ThinkNode_FilterPriority)base.DeepCopy(resolve);
			thinkNode_FilterPriority.minPriority = this.minPriority;
			return thinkNode_FilterPriority;
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x00181DFC File Offset: 0x0017FFFC
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.subNodes[i].GetPriority(pawn) > this.minPriority)
				{
					ThinkResult result = this.subNodes[i].TryIssueJobPackage(pawn, jobParams);
					if (result.IsValid)
					{
						return result;
					}
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04002C2A RID: 11306
		public float minPriority = 0.5f;
	}
}
