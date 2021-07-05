using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000629 RID: 1577
	public class ThinkNode_PrioritySorter : ThinkNode
	{
		// Token: 0x06002D40 RID: 11584 RVA: 0x0010F59F File Offset: 0x0010D79F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_PrioritySorter thinkNode_PrioritySorter = (ThinkNode_PrioritySorter)base.DeepCopy(resolve);
			thinkNode_PrioritySorter.minPriority = this.minPriority;
			return thinkNode_PrioritySorter;
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x0010F5BC File Offset: 0x0010D7BC
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_PrioritySorter.workingNodes.Clear();
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				ThinkNode_PrioritySorter.workingNodes.Insert(Rand.Range(0, ThinkNode_PrioritySorter.workingNodes.Count - 1), this.subNodes[i]);
			}
			while (ThinkNode_PrioritySorter.workingNodes.Count > 0)
			{
				float num = 0f;
				int num2 = -1;
				for (int j = 0; j < ThinkNode_PrioritySorter.workingNodes.Count; j++)
				{
					float num3 = 0f;
					try
					{
						num3 = ThinkNode_PrioritySorter.workingNodes[j].GetPriority(pawn);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception in ",
							base.GetType(),
							" GetPriority: ",
							ex.ToString()
						}));
					}
					if (num3 > 0f && num3 >= this.minPriority && num3 > num)
					{
						num = num3;
						num2 = j;
					}
				}
				if (num2 == -1)
				{
					break;
				}
				ThinkResult result = ThinkResult.NoJob;
				try
				{
					result = ThinkNode_PrioritySorter.workingNodes[num2].TryIssueJobPackage(pawn, jobParams);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception in ",
						base.GetType(),
						" TryIssueJobPackage: ",
						ex2.ToString()
					}));
				}
				if (result.IsValid)
				{
					return result;
				}
				ThinkNode_PrioritySorter.workingNodes.RemoveAt(num2);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04001BC2 RID: 7106
		public float minPriority;

		// Token: 0x04001BC3 RID: 7107
		private static List<ThinkNode> workingNodes = new List<ThinkNode>();
	}
}
