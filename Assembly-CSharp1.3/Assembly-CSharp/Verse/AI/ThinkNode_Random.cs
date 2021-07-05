using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200062A RID: 1578
	public class ThinkNode_Random : ThinkNode
	{
		// Token: 0x06002D44 RID: 11588 RVA: 0x0010F75C File Offset: 0x0010D95C
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

		// Token: 0x04001BC4 RID: 7108
		private static List<ThinkNode> tempList = new List<ThinkNode>();
	}
}
