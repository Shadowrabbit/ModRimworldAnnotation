using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000119 RID: 281
	public class ThinkTreeDef : Def
	{
		// Token: 0x060007A1 RID: 1953 RVA: 0x000238E8 File Offset: 0x00021AE8
		public override void ResolveReferences()
		{
			this.thinkRoot.ResolveSubnodesAndRecur();
			foreach (ThinkNode thinkNode in this.thinkRoot.ThisAndChildrenRecursive)
			{
				thinkNode.ResolveReferences();
			}
			ThinkTreeKeyAssigner.AssignKeys(this.thinkRoot, GenText.StableStringHash(this.defName));
			this.ResolveParentNodes(this.thinkRoot);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00023964 File Offset: 0x00021B64
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			HashSet<int> usedKeys = new HashSet<int>();
			HashSet<ThinkNode> instances = new HashSet<ThinkNode>();
			foreach (ThinkNode node in this.thinkRoot.ThisAndChildrenRecursive)
			{
				int key = node.UniqueSaveKey;
				if (key == -1)
				{
					yield return string.Concat(new object[]
					{
						"Thinknode ",
						node.GetType(),
						" has invalid save key ",
						key
					});
				}
				else if (instances.Contains(node))
				{
					yield return "There are two same ThinkNode instances in one think tree (their type is " + node.GetType() + ")";
				}
				else if (usedKeys.Contains(key))
				{
					yield return string.Concat(new object[]
					{
						"Two ThinkNodes have the same unique save key ",
						key,
						" (one of the nodes is ",
						node.GetType(),
						")"
					});
				}
				if (key != -1)
				{
					usedKeys.Add(key);
				}
				instances.Add(node);
				node = null;
			}
			IEnumerator<ThinkNode> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00023974 File Offset: 0x00021B74
		public bool TryGetThinkNodeWithSaveKey(int key, out ThinkNode outNode)
		{
			outNode = null;
			if (key == -1)
			{
				return false;
			}
			if (key == this.thinkRoot.UniqueSaveKey)
			{
				outNode = this.thinkRoot;
				return true;
			}
			foreach (ThinkNode thinkNode in this.thinkRoot.ChildrenRecursive)
			{
				if (thinkNode.UniqueSaveKey == key)
				{
					outNode = thinkNode;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x000239F4 File Offset: 0x00021BF4
		private void ResolveParentNodes(ThinkNode node)
		{
			for (int i = 0; i < node.subNodes.Count; i++)
			{
				if (node.subNodes[i].parent != null)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Think node ",
						node.subNodes[i],
						" from think tree ",
						this.defName,
						" already has a parent node (",
						node.subNodes[i].parent,
						"). This means that it's referenced by more than one think tree (should have been copied instead)."
					}));
				}
				else
				{
					node.subNodes[i].parent = node;
					this.ResolveParentNodes(node.subNodes[i]);
				}
			}
		}

		// Token: 0x0400073E RID: 1854
		public ThinkNode thinkRoot;

		// Token: 0x0400073F RID: 1855
		[NoTranslate]
		public string insertTag;

		// Token: 0x04000740 RID: 1856
		public float insertPriority;
	}
}
