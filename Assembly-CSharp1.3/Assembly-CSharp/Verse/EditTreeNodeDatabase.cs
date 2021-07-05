using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200040B RID: 1035
	public static class EditTreeNodeDatabase
	{
		// Token: 0x06001F01 RID: 7937 RVA: 0x000C143C File Offset: 0x000BF63C
		public static TreeNode_Editor RootOf(object obj)
		{
			for (int i = 0; i < EditTreeNodeDatabase.roots.Count; i++)
			{
				if (EditTreeNodeDatabase.roots[i].obj == obj)
				{
					return EditTreeNodeDatabase.roots[i];
				}
			}
			TreeNode_Editor treeNode_Editor = TreeNode_Editor.NewRootNode(obj);
			EditTreeNodeDatabase.roots.Add(treeNode_Editor);
			return treeNode_Editor;
		}

		// Token: 0x040012E9 RID: 4841
		private static List<TreeNode_Editor> roots = new List<TreeNode_Editor>();
	}
}
