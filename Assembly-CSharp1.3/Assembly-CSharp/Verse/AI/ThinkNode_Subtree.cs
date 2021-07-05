using System;

namespace Verse.AI
{
	// Token: 0x0200062C RID: 1580
	public class ThinkNode_Subtree : ThinkNode
	{
		// Token: 0x06002D4B RID: 11595 RVA: 0x0010F8B8 File Offset: 0x0010DAB8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Subtree thinkNode_Subtree = (ThinkNode_Subtree)base.DeepCopy(false);
			thinkNode_Subtree.treeDef = this.treeDef;
			if (resolve)
			{
				thinkNode_Subtree.ResolveSubnodesAndRecur();
				thinkNode_Subtree.subtreeNode = thinkNode_Subtree.subNodes[this.subNodes.IndexOf(this.subtreeNode)];
			}
			return thinkNode_Subtree;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x0010F90A File Offset: 0x0010DB0A
		protected override void ResolveSubnodes()
		{
			this.subtreeNode = this.treeDef.thinkRoot.DeepCopy(true);
			this.subNodes.Add(this.subtreeNode);
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x0010F934 File Offset: 0x0010DB34
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			return this.subtreeNode.TryIssueJobPackage(pawn, jobParams);
		}

		// Token: 0x04001BC6 RID: 7110
		private ThinkTreeDef treeDef;

		// Token: 0x04001BC7 RID: 7111
		[Unsaved(false)]
		public ThinkNode subtreeNode;
	}
}
