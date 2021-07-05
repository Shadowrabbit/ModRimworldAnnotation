using System;

namespace Verse.AI
{
	// Token: 0x02000626 RID: 1574
	public class ThinkNode_FilterPriority : ThinkNode
	{
		// Token: 0x06002D3A RID: 11578 RVA: 0x0010F46C File Offset: 0x0010D66C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_FilterPriority thinkNode_FilterPriority = (ThinkNode_FilterPriority)base.DeepCopy(resolve);
			thinkNode_FilterPriority.minPriority = this.minPriority;
			return thinkNode_FilterPriority;
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x0010F488 File Offset: 0x0010D688
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

		// Token: 0x04001BB3 RID: 7091
		public float minPriority = 0.5f;
	}
}
