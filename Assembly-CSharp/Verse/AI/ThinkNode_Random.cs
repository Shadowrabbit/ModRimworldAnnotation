using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A87 RID: 2695
	public class ThinkNode_Random : ThinkNode
	{
		// Token: 0x06004021 RID: 16417 RVA: 0x0018206C File Offset: 0x0018026C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_Random.tempList.Clear();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				ThinkNode_Random.tempList.Add(this.subNodes[i]);
			}
			ThinkNode_Random.tempList.Shuffle<ThinkNode>();
			for (int j = 0; j < ThinkNode_Random.tempList.Count; j++)
			{
				ThinkResult result = ThinkNode_Random.tempList[j].TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04002C3A RID: 11322
		private static List<ThinkNode> tempList = new List<ThinkNode>();
	}
}
