using System;

namespace Verse.AI
{
	// Token: 0x02000A89 RID: 2697
	public class ThinkNode_Subtree : ThinkNode
	{
		// Token: 0x06004028 RID: 16424 RVA: 0x001821A8 File Offset: 0x001803A8
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

		// Token: 0x06004029 RID: 16425 RVA: 0x000300DC File Offset: 0x0002E2DC
		protected override void ResolveSubnodes()
		{
			this.subtreeNode = this.treeDef.thinkRoot.DeepCopy(true);
			this.subNodes.Add(this.subtreeNode);
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00030106 File Offset: 0x0002E306
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			return this.subtreeNode.TryIssueJobPackage(pawn, jobParams);
		}

		// Token: 0x04002C3C RID: 11324
		private ThinkTreeDef treeDef;

		// Token: 0x04002C3D RID: 11325
		[Unsaved(false)]
		public ThinkNode subtreeNode;
	}
}
