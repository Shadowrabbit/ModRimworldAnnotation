using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A99 RID: 2713
	public abstract class ThinkNode
	{
		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06004059 RID: 16473 RVA: 0x000302B5 File Offset: 0x0002E4B5
		public int UniqueSaveKey
		{
			get
			{
				return this.uniqueSaveKeyInt;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x0600405A RID: 16474 RVA: 0x000302BD File Offset: 0x0002E4BD
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

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x000302CD File Offset: 0x0002E4CD
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

		// Token: 0x0600405C RID: 16476 RVA: 0x000302DD File Offset: 0x0002E4DD
		public virtual float GetPriority(Pawn pawn)
		{
			if (this.priority < 0f)
			{
				Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode(), false);
				return 0f;
			}
			return this.priority;
		}

		// Token: 0x0600405D RID: 16477
		public abstract ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams);

		// Token: 0x0600405E RID: 16478 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void ResolveSubnodes()
		{
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x00182768 File Offset: 0x00180968
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

		// Token: 0x06004060 RID: 16480 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x001827B0 File Offset: 0x001809B0
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

		// Token: 0x06004062 RID: 16482 RVA: 0x0003030F File Offset: 0x0002E50F
		internal void SetUniqueSaveKey(int key)
		{
			this.uniqueSaveKeyInt = key;
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x00030318 File Offset: 0x0002E518
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.uniqueSaveKeyInt, 1157295731);
		}

		// Token: 0x04002C50 RID: 11344
		public List<ThinkNode> subNodes = new List<ThinkNode>();

		// Token: 0x04002C51 RID: 11345
		public bool leaveJoinableLordIfIssuesJob;

		// Token: 0x04002C52 RID: 11346
		protected float priority = -1f;

		// Token: 0x04002C53 RID: 11347
		[Unsaved(false)]
		private int uniqueSaveKeyInt = -2;

		// Token: 0x04002C54 RID: 11348
		[Unsaved(false)]
		public ThinkNode parent;

		// Token: 0x04002C55 RID: 11349
		public const int InvalidSaveKey = -1;

		// Token: 0x04002C56 RID: 11350
		protected const int UnresolvedSaveKey = -2;
	}
}
