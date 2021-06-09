﻿using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A86 RID: 2694
	public class ThinkNode_PrioritySorter : ThinkNode
	{
		// Token: 0x0600401D RID: 16413 RVA: 0x00030090 File Offset: 0x0002E290
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_PrioritySorter thinkNode_PrioritySorter = (ThinkNode_PrioritySorter)base.DeepCopy(resolve);
			thinkNode_PrioritySorter.minPriority = this.minPriority;
			return thinkNode_PrioritySorter;
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x00181ED8 File Offset: 0x001800D8
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
						}), false);
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
					}), false);
				}
				if (result.IsValid)
				{
					return result;
				}
				ThinkNode_PrioritySorter.workingNodes.RemoveAt(num2);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04002C38 RID: 11320
		public float minPriority;

		// Token: 0x04002C39 RID: 11321
		private static List<ThinkNode> workingNodes = new List<ThinkNode>();
	}
}
