using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200063B RID: 1595
	public abstract class ThinkNode
	{
		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002D79 RID: 11641 RVA: 0x00110098 File Offset: 0x0010E298
		public int UniqueSaveKey
		{
			get
			{
				return this.uniqueSaveKeyInt;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002D7A RID: 11642 RVA: 0x001100A0 File Offset: 0x0010E2A0
		public IEnumerable<ThinkNode> ThisAndChildrenRecursive
		{
			get
			{
				yield return this;
				foreach (ThinkNode thinkNode in this.ChildrenRecursive)
				{
					yield return thinkNode;
				}
				IEnumerator<ThinkNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002D7B RID: 11643 RVA: 0x001100B0 File Offset: 0x0010E2B0
		public IEnumerable<ThinkNode> ChildrenRecursive
		{
			get
			{
				int num;
				for (int i = 0; i < this.subNodes.Count; i = num + 1)
				{
					foreach (ThinkNode thinkNode in this.subNodes[i].ThisAndChildrenRecursive)
					{
						yield return thinkNode;
					}
					IEnumerator<ThinkNode> enumerator = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x001100C0 File Offset: 0x0010E2C0
		public virtual float GetPriority(Pawn pawn)
		{
			if (this.priority < 0f)
			{
				Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode());
				return 0f;
			}
			return this.priority;
		}

		// Token: 0x06002D7D RID: 11645
		public abstract ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams);

		// Token: 0x06002D7E RID: 11646 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void ResolveSubnodes()
		{
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x001100F4 File Offset: 0x0010E2F4
		public void ResolveSubnodesAndRecur()
		{
			if (this.uniqueSaveKeyInt != -2)
			{
				return;
			}
			this.ResolveSubnodes();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				this.subNodes[i].ResolveSubnodesAndRecur();
			}
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x0011013C File Offset: 0x0010E33C
		public virtual ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode thinkNode = (ThinkNode)Activator.CreateInstance(base.GetType());
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				thinkNode.subNodes.Add(this.subNodes[i].DeepCopy(resolve));
			}
			thinkNode.priority = this.priority;
			thinkNode.leaveJoinableLordIfIssuesJob = this.leaveJoinableLordIfIssuesJob;
			thinkNode.uniqueSaveKeyInt = this.uniqueSaveKeyInt;
			if (resolve)
			{
				thinkNode.ResolveSubnodesAndRecur();
			}
			ThinkTreeKeyAssigner.AssignSingleKey(thinkNode, 0);
			return thinkNode;
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x001101C2 File Offset: 0x0010E3C2
		internal void SetUniqueSaveKey(int key)
		{
			this.uniqueSaveKeyInt = key;
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x001101CB File Offset: 0x0010E3CB
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.uniqueSaveKeyInt, 1157295731);
		}

		// Token: 0x04001BD8 RID: 7128
		public List<ThinkNode> subNodes = new List<ThinkNode>();

		// Token: 0x04001BD9 RID: 7129
		public bool leaveJoinableLordIfIssuesJob;

		// Token: 0x04001BDA RID: 7130
		protected float priority = -1f;

		// Token: 0x04001BDB RID: 7131
		[Unsaved(false)]
		private int uniqueSaveKeyInt = -2;

		// Token: 0x04001BDC RID: 7132
		[Unsaved(false)]
		public ThinkNode parent;

		// Token: 0x04001BDD RID: 7133
		public const int InvalidSaveKey = -1;

		// Token: 0x04001BDE RID: 7134
		protected const int UnresolvedSaveKey = -2;
	}
}
