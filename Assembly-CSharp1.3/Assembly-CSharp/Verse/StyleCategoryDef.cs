using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200010B RID: 267
	public class StyleCategoryDef : Def
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x00021A28 File Offset: 0x0001FC28
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					if (this.iconPath.NullOrEmpty())
					{
						this.cachedIcon = BaseContent.BadTex;
					}
					else
					{
						this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x00021A78 File Offset: 0x0001FC78
		public List<BuildableDef> AllDesignatorBuildables
		{
			get
			{
				if (this.cachedAllDesignatorBuildables == null)
				{
					this.cachedAllDesignatorBuildables = new List<BuildableDef>();
					if (this.addDesignators != null)
					{
						foreach (BuildableDef item in this.addDesignators)
						{
							this.cachedAllDesignatorBuildables.Add(item);
						}
					}
					if (this.addDesignatorGroups != null)
					{
						foreach (DesignatorDropdownGroupDef designatorDropdownGroupDef in this.addDesignatorGroups)
						{
							this.cachedAllDesignatorBuildables.AddRange(designatorDropdownGroupDef.BuildablesWithoutDefaultDesignators());
						}
					}
				}
				return this.cachedAllDesignatorBuildables;
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00021B48 File Offset: 0x0001FD48
		public ThingStyleDef GetStyleForThingDef(BuildableDef thingDef, Precept precept = null)
		{
			ThingStyleDef result;
			try
			{
				for (int i = 0; i < this.thingDefStyles.Count; i++)
				{
					if (this.thingDefStyles[i].ThingDef == thingDef)
					{
						StyleCategoryDef.tmpAvailableStyles.Add(this.thingDefStyles[i].StyleDef);
					}
				}
				if (StyleCategoryDef.tmpAvailableStyles.Count == 0)
				{
					result = null;
				}
				else if (StyleCategoryDef.tmpAvailableStyles.Count == 1 || precept == null)
				{
					result = StyleCategoryDef.tmpAvailableStyles[0];
				}
				else
				{
					result = StyleCategoryDef.tmpAvailableStyles[Rand.RangeSeeded(0, StyleCategoryDef.tmpAvailableStyles.Count, precept.randomSeed)];
				}
			}
			finally
			{
				StyleCategoryDef.tmpAvailableStyles.Clear();
			}
			return result;
		}

		// Token: 0x0400064D RID: 1613
		public List<ThingDefStyle> thingDefStyles;

		// Token: 0x0400064E RID: 1614
		[NoTranslate]
		public string iconPath;

		// Token: 0x0400064F RID: 1615
		public List<BuildableDef> addDesignators;

		// Token: 0x04000650 RID: 1616
		public List<DesignatorDropdownGroupDef> addDesignatorGroups;

		// Token: 0x04000651 RID: 1617
		public SoundDef soundOngoingRitual;

		// Token: 0x04000652 RID: 1618
		public RitualVisualEffectDef ritualVisualEffectDef;

		// Token: 0x04000653 RID: 1619
		private Texture2D cachedIcon;

		// Token: 0x04000654 RID: 1620
		private List<BuildableDef> cachedAllDesignatorBuildables;

		// Token: 0x04000655 RID: 1621
		private static List<ThingStyleDef> tmpAvailableStyles = new List<ThingStyleDef>();
	}
}
