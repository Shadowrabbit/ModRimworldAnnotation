using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200010F RID: 271
	public class ThingCategoryDef : Def
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x00021FB2 File Offset: 0x000201B2
		public List<ThingDef> SortedChildThingDefs
		{
			get
			{
				return this.sortedChildThingDefsCached;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x00021FBA File Offset: 0x000201BA
		public IEnumerable<ThingCategoryDef> Parents
		{
			get
			{
				if (this.parent != null)
				{
					yield return this.parent;
					foreach (ThingCategoryDef thingCategoryDef in this.parent.Parents)
					{
						yield return thingCategoryDef;
					}
					IEnumerator<ThingCategoryDef> enumerator = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00021FCA File Offset: 0x000201CA
		public IEnumerable<ThingCategoryDef> ThisAndChildCategoryDefs
		{
			get
			{
				yield return this;
				foreach (ThingCategoryDef thingCategoryDef in this.childCategories)
				{
					foreach (ThingCategoryDef thingCategoryDef2 in thingCategoryDef.ThisAndChildCategoryDefs)
					{
						yield return thingCategoryDef2;
					}
					IEnumerator<ThingCategoryDef> enumerator2 = null;
				}
				List<ThingCategoryDef>.Enumerator enumerator = default(List<ThingCategoryDef>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x00021FDA File Offset: 0x000201DA
		public IEnumerable<ThingDef> DescendantThingDefs
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.ThisAndChildCategoryDefs)
				{
					foreach (ThingDef thingDef in thingCategoryDef.childThingDefs)
					{
						yield return thingDef;
					}
					List<ThingDef>.Enumerator enumerator2 = default(List<ThingDef>.Enumerator);
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x00021FEA File Offset: 0x000201EA
		public IEnumerable<SpecialThingFilterDef> DescendantSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.ThisAndChildCategoryDefs)
				{
					foreach (SpecialThingFilterDef specialThingFilterDef in thingCategoryDef.childSpecialFilters)
					{
						yield return specialThingFilterDef;
					}
					List<SpecialThingFilterDef>.Enumerator enumerator2 = default(List<SpecialThingFilterDef>.Enumerator);
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x00021FFA File Offset: 0x000201FA
		public IEnumerable<SpecialThingFilterDef> ParentsSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.Parents)
				{
					foreach (SpecialThingFilterDef specialThingFilterDef in thingCategoryDef.childSpecialFilters)
					{
						yield return specialThingFilterDef;
					}
					List<SpecialThingFilterDef>.Enumerator enumerator2 = default(List<SpecialThingFilterDef>.Enumerator);
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0002200A File Offset: 0x0002020A
		public bool ContainedInThisOrDescendant(ThingDef thingDef)
		{
			return this.allChildThingDefsCached.Contains(thingDef);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00022018 File Offset: 0x00020218
		public override void ResolveReferences()
		{
			this.allChildThingDefsCached = new HashSet<ThingDef>();
			foreach (ThingCategoryDef thingCategoryDef in this.ThisAndChildCategoryDefs)
			{
				foreach (ThingDef item in thingCategoryDef.childThingDefs)
				{
					this.allChildThingDefsCached.Add(item);
				}
			}
			this.sortedChildThingDefsCached = (from n in this.childThingDefs
			orderby n.label
			select n).ToList<ThingDef>();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000220E4 File Offset: 0x000202E4
		public override void PostLoad()
		{
			this.treeNode = new TreeNode_ThingCategory(this);
			if (!this.iconPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00022110 File Offset: 0x00020310
		public static ThingCategoryDef Named(string defName)
		{
			return DefDatabase<ThingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00019789 File Offset: 0x00017989
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x0400067F RID: 1663
		public ThingCategoryDef parent;

		// Token: 0x04000680 RID: 1664
		[NoTranslate]
		public string iconPath;

		// Token: 0x04000681 RID: 1665
		public bool resourceReadoutRoot;

		// Token: 0x04000682 RID: 1666
		[Unsaved(false)]
		public TreeNode_ThingCategory treeNode;

		// Token: 0x04000683 RID: 1667
		[Unsaved(false)]
		public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

		// Token: 0x04000684 RID: 1668
		[Unsaved(false)]
		public List<ThingDef> childThingDefs = new List<ThingDef>();

		// Token: 0x04000685 RID: 1669
		[Unsaved(false)]
		private HashSet<ThingDef> allChildThingDefsCached;

		// Token: 0x04000686 RID: 1670
		[Unsaved(false)]
		private List<ThingDef> sortedChildThingDefsCached;

		// Token: 0x04000687 RID: 1671
		[Unsaved(false)]
		public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04000688 RID: 1672
		[Unsaved(false)]
		public Texture2D icon = BaseContent.BadTex;
	}
}
