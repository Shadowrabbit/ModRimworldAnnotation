using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000118 RID: 280
	public static class ThinkTreeKeyAssigner
	{
		// Token: 0x0600079C RID: 1948 RVA: 0x00023800 File Offset: 0x00021A00
		public static void Reset()
		{
			ThinkTreeKeyAssigner.assignedKeys.Clear();
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0002380C File Offset: 0x00021A0C
		public static void AssignKeys(ThinkNode rootNode, int startHash)
		{
			Rand.PushState(startHash);
			foreach (ThinkNode thinkNode in rootNode.ThisAndChildrenRecursive)
			{
				thinkNode.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(thinkNode));
			}
			Rand.PopState();
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00023868 File Offset: 0x00021A68
		public static void AssignSingleKey(ThinkNode node, int startHash)
		{
			Rand.PushState(startHash);
			node.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(node));
			Rand.PopState();
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00023884 File Offset: 0x00021A84
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

		// Token: 0x0400073D RID: 1853
		private static HashSet<int> assignedKeys = new HashSet<int>();
	}
}
