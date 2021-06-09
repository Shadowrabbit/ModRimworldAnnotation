using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200074A RID: 1866
	public class TreeNode_ThingCategory : TreeNode
	{
		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06002F04 RID: 12036 RVA: 0x00024E10 File Offset: 0x00023010
		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002F05 RID: 12037 RVA: 0x00024E1D File Offset: 0x0002301D
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002F06 RID: 12038 RVA: 0x00024E2A File Offset: 0x0002302A
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

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002F07 RID: 12039 RVA: 0x00024E3A File Offset: 0x0002303A
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

		// Token: 0x06002F08 RID: 12040 RVA: 0x00024E4A File Offset: 0x0002304A
		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x00024E59 File Offset: 0x00023059
		public override string ToString()
		{
			return this.catDef.defName;
		}

		// Token: 0x04001FE7 RID: 8167
		public ThingCategoryDef catDef;
	}
}
