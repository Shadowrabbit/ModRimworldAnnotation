using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	// Token: 0x0200062D RID: 1581
	public class ThinkNode_SubtreesByTag : ThinkNode
	{
		// Token: 0x06002D4F RID: 11599 RVA: 0x0010F943 File Offset: 0x0010DB43
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_SubtreesByTag thinkNode_SubtreesByTag = (ThinkNode_SubtreesByTag)base.DeepCopy(resolve);
			thinkNode_SubtreesByTag.insertTag = this.insertTag;
			return thinkNode_SubtreesByTag;
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x0000313F File Offset: 0x0000133F
		protected override void ResolveSubnodes()
		{
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x0010F960 File Offset: 0x0010DB60
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.matchedTrees == null)
			{
				this.matchedTrees = new List<ThinkTreeDef>();
				foreach (ThinkTreeDef thinkTreeDef in DefDatabase<ThinkTreeDef>.AllDefs)
				{
					if (thinkTreeDef.insertTag == this.insertTag)
					{
						this.matchedTrees.Add(thinkTreeDef);
					}
				}
				this.matchedTrees = (from tDef in this.matchedTrees
				orderby tDef.insertPriority descending
				select tDef).ToList<ThinkTreeDef>();
			}
			for (int i = 0; i < this.matchedTrees.Count; i++)
			{
				ThinkResult result = this.matchedTrees[i].thinkRoot.TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04001BC8 RID: 7112
		[NoTranslate]
		public string insertTag;

		// Token: 0x04001BC9 RID: 7113
		[Unsaved(false)]
		private List<ThinkTreeDef> matchedTrees;
	}
}
