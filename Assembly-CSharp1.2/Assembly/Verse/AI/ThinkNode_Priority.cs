using System;

namespace Verse.AI
{
	// Token: 0x02000A82 RID: 2690
	public class ThinkNode_Priority : ThinkNode
	{
		// Token: 0x06004015 RID: 16405 RVA: 0x00181D64 File Offset: 0x0017FF64
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				ThinkResult result = ThinkResult.NoJob;
				try
				{
					result = this.subNodes[i].TryIssueJobPackage(pawn, jobParams);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception in ",
						base.GetType(),
						" TryIssueJobPackage: ",
						ex.ToString()
					}), false);
				}
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}
	}
}
