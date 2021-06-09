using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001A8 RID: 424
	public static class ThinkTreeKeyAssigner
	{
		// Token: 0x06000ADD RID: 2781 RVA: 0x0000E7ED File Offset: 0x0000C9ED
		public static void Reset()
		{
			ThinkTreeKeyAssigner.assignedKeys.Clear();
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0009F3CC File Offset: 0x0009D5CC
		public static void AssignKeys(ThinkNode rootNode, int startHash)
		{
			Rand.PushState(startHash);
			foreach (ThinkNode thinkNode in rootNode.ThisAndChildrenRecursive)
			{
				thinkNode.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(thinkNode));
			}
			Rand.PopState();
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0000E7F9 File Offset: 0x0000C9F9
		public static void AssignSingleKey(ThinkNode node, int startHash)
		{
			Rand.PushState(startHash);
			node.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(node));
			Rand.PopState();
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0009F428 File Offset: 0x0009D628
		private static int NextUnusedKeyFor(ThinkNode node)
		{
			int num = 0;
			while (node != null)
			{
				num = Gen.HashCombineInt(num, GenText.StableStringHash(node.GetType().Name));
				node = node.parent;
			}
			while (ThinkTreeKeyAssigner.assignedKeys.Contains(num))
			{
				num ^= Rand.Int;
			}
			ThinkTreeKeyAssigner.assignedKeys.Add(num);
			return num;
		}

		// Token: 0x040009A8 RID: 2472
		private static HashSet<int> assignedKeys = new HashSet<int>();
	}
}
