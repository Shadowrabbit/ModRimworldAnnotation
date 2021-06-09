using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000191 RID: 401
	public class ThingCategoryDef : Def
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x0000DC52 File Offset: 0x0000BE52
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

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x0000DC62 File Offset: 0x0000BE62
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

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0000DC72 File Offset: 0x0000BE72
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

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0000DC82 File Offset: 0x0000BE82
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

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0000DC92 File Offset: 0x0000BE92
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

		// Token: 0x06000A03 RID: 2563 RVA: 0x0000DCA2 File Offset: 0x0000BEA2
		public bool ContainedInThisOrDescendant(ThingDef thingDef)
		{
			return this.allChildThingDefsCached.Contains(thingDef);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0009AE20 File Offset: 0x00099020
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
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0000DCB0 File Offset: 0x0000BEB0
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

		// Token: 0x06000A06 RID: 2566 RVA: 0x0000DCDC File Offset: 0x0000BEDC
		public static ThingCategoryDef Named(string defName)
		{
			return DefDatabase<ThingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0000B495 File Offset: 0x00009695
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x040008B6 RID: 2230
		public ThingCategoryDef parent;

		// Token: 0x040008B7 RID: 2231
		[NoTranslate]
		public string iconPath;

		// Token: 0x040008B8 RID: 2232
		public bool resourceReadoutRoot;

		// Token: 0x040008B9 RID: 2233
		[Unsaved(false)]
		public TreeNode_ThingCategory treeNode;

		// Token: 0x040008BA RID: 2234
		[Unsaved(false)]
		public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

		// Token: 0x040008BB RID: 2235
		[Unsaved(false)]
		public List<ThingDef> childThingDefs = new List<ThingDef>();

		// Token: 0x040008BC RID: 2236
		[Unsaved(false)]
		private HashSet<ThingDef> allChildThingDefsCached;

		// Token: 0x040008BD RID: 2237
		[Unsaved(false)]
		public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x040008BE RID: 2238
		[Unsaved(false)]
		public Texture2D icon = BaseContent.BadTex;
	}
}
