using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000737 RID: 1847
	public static class EditTreeNodeDatabase
	{
		// Token: 0x06002E82 RID: 11906 RVA: 0x00137AF8 File Offset: 0x00135CF8
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

		// Token: 0x04001FAF RID: 8111
		private static List<TreeNode_Editor> roots = new List<TreeNode_Editor>();
	}
}
