using System;

namespace Verse.AI
{
	// Token: 0x02000625 RID: 1573
	public class ThinkNode_Priority : ThinkNode
	{
		// Token: 0x06002D38 RID: 11576 RVA: 0x0010F3CC File Offset: 0x0010D5CC
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
					}));
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
