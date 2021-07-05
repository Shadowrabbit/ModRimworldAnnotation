using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000416 RID: 1046
	public static class ThingCategoryNodeDatabase
	{
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001F76 RID: 8054 RVA: 0x000C40A2 File Offset: 0x000C22A2
		public static TreeNode_ThingCategory RootNode
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode;
			}
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x000C40A9 File Offset: 0x000C22A9
		public static void Clear()
		{
			ThingCategoryNodeDatabase.rootNode = null;
			ThingCategoryNodeDatabase.initialized = false;
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x000C40B8 File Offset: 0x000C22B8
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

		// Token: 0x06001F79 RID: 8057 RVA: 0x000C4250 File Offset: 0x000C2450
		private static void SetNestLevelRecursive(TreeNode_ThingCategory node, int nestDepth)
		{
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				thingCategoryDef.treeNode.nestDepth = nestDepth;
				ThingCategoryNodeDatabase.SetNestLevelRecursive(thingCategoryDef.treeNode, nestDepth + 1);
			}
		}

		// Token: 0x04001316 RID: 4886
		public static bool initialized;

		// Token: 0x04001317 RID: 4887
		private static TreeNode_ThingCategory rootNode;

		// Token: 0x04001318 RID: 4888
		public static List<TreeNode_ThingCategory> allThingCategoryNodes;
	}
}
