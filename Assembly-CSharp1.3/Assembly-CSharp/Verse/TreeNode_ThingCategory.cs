using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000417 RID: 1047
	public class TreeNode_ThingCategory : TreeNode
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001F7B RID: 8059 RVA: 0x000C42BC File Offset: 0x000C24BC
		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001F7C RID: 8060 RVA: 0x000C42C9 File Offset: 0x000C24C9
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x000C42D6 File Offset: 0x000C24D6
		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodesAndThis
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.catDef.ThisAndChildCategoryDefs)
				{
					yield return thingCategoryDef.treeNode;
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001F7E RID: 8062 RVA: 0x000C42E6 File Offset: 0x000C24E6
		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.catDef.childCategories)
				{
					yield return thingCategoryDef.treeNode;
				}
				List<ThingCategoryDef>.Enumerator enumerator = default(List<ThingCategoryDef>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x000C42F6 File Offset: 0x000C24F6
		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x000C4305 File Offset: 0x000C2505
		public override string ToString()
		{
			return this.catDef.defName;
		}

		// Token: 0x04001319 RID: 4889
		public ThingCategoryDef catDef;
	}
}
