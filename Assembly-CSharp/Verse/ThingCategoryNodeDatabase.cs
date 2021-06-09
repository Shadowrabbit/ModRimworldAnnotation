using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000749 RID: 1865
	public static class ThingCategoryNodeDatabase
	{
		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06002EFF RID: 12031 RVA: 0x00024DFB File Offset: 0x00022FFB
		public static TreeNode_ThingCategory RootNode
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode;
			}
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x00024E02 File Offset: 0x00023002
		public static void Clear()
		{
			ThingCategoryNodeDatabase.rootNode = null;
			ThingCategoryNodeDatabase.initialized = false;
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x00139E08 File Offset: 0x00138008
		public static void FinalizeInit()
		{
			ThingCategoryNodeDatabase.rootNode = ThingCategoryDefOf.Root.treeNode;
			foreach (ThingCategoryDef thingCategoryDef in DefDatabase<ThingCategoryDef>.AllDefs)
			{
				if (thingCategoryDef.parent != null)
				{
					thingCategoryDef.parent.childCategories.Add(thingCategoryDef);
				}
			}
			ThingCategoryNodeDatabase.SetNestLevelRecursive(ThingCategoryNodeDatabase.rootNode, 0);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.thingCategories != null)
				{
					foreach (ThingCategoryDef thingCategoryDef2 in thingDef.thingCategories)
					{
						thingCategoryDef2.childThingDefs.Add(thingDef);
					}
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in DefDatabase<SpecialThingFilterDef>.AllDefs)
			{
				specialThingFilterDef.parentCategory.childSpecialFilters.Add(specialThingFilterDef);
			}
			if (ThingCategoryNodeDatabase.rootNode.catDef.childCategories.Any<ThingCategoryDef>())
			{
				ThingCategoryNodeDatabase.rootNode.catDef.childCategories[0].treeNode.SetOpen(-1, true);
			}
			ThingCategoryNodeDatabase.allThingCategoryNodes = ThingCategoryNodeDatabase.rootNode.ChildCategoryNodesAndThis.ToList<TreeNode_ThingCategory>();
			ThingCategoryNodeDatabase.initialized = true;
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x00139FA0 File Offset: 0x001381A0
		private static void SetNestLevelRecursive(TreeNode_ThingCategory node, int nestDepth)
		{
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				thingCategoryDef.treeNode.nestDepth = nestDepth;
				ThingCategoryNodeDatabase.SetNestLevelRecursive(thingCategoryDef.treeNode, nestDepth + 1);
			}
		}

		// Token: 0x04001FE4 RID: 8164
		public static bool initialized;

		// Token: 0x04001FE5 RID: 8165
		private static TreeNode_ThingCategory rootNode;

		// Token: 0x04001FE6 RID: 8166
		public static List<TreeNode_ThingCategory> allThingCategoryNodes;
	}
}
