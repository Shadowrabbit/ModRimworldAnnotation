using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	// Token: 0x02000A8A RID: 2698
	public class ThinkNode_SubtreesByTag : ThinkNode
	{
		// Token: 0x0600402C RID: 16428 RVA: 0x00030115 File Offset: 0x0002E315
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_SubtreesByTag thinkNode_SubtreesByTag = (ThinkNode_SubtreesByTag)base.DeepCopy(resolve);
			thinkNode_SubtreesByTag.insertTag = this.insertTag;
			return thinkNode_SubtreesByTag;
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x00006A05 File Offset: 0x00004C05
		protected override void ResolveSubnodes()
		{
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x001821FC File Offset: 0x001803FC
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

		// Token: 0x04002C3E RID: 11326
		[NoTranslate]
		public string insertTag;

		// Token: 0x04002C3F RID: 11327
		[Unsaved(false)]
		private List<ThinkTreeDef> matchedTrees;
	}
}
