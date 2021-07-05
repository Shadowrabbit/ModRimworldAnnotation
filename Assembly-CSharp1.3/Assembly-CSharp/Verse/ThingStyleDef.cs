using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000116 RID: 278
	public class ThingStyleDef : Def
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x00023653 File Offset: 0x00021853
		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0002365B File Offset: 0x0002185B
		public Texture2D UIIcon
		{
			get
			{
				return this.uiIcon;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00023664 File Offset: 0x00021864
		public StyleCategoryDef Category
		{
			get
			{
				if (this.hasCategory == null)
				{
					foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						if (styleCategoryDef.thingDefStyles.Any((ThingDefStyle x) => x.StyleDef == this))
						{
							this.cachedCategory = styleCategoryDef;
							this.hasCategory = new bool?(true);
							break;
						}
					}
					if (this.cachedCategory == null)
					{
						this.hasCategory = new bool?(false);
					}
				}
				return this.cachedCategory;
			}
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00023700 File Offset: 0x00021900
		public override void PostLoad()
		{
			if (this.graphicData != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					if (this.graphicData.shaderType == null)
					{
						this.graphicData.shaderType = ShaderTypeDefOf.Cutout;
					}
					this.graphic = this.graphicData.Graphic;
					if (this.graphic == BaseContent.BadGraphic)
					{
						this.graphic = null;
					}
					this.ResolveUIIcon();
				});
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0002371C File Offset: 0x0002191C
		private void ResolveUIIcon()
		{
			if (!this.uiIconPath.NullOrEmpty())
			{
				this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				return;
			}
			if (this.graphic != null)
			{
				Material material = this.graphic.ExtractInnerGraphicFor(null).MatAt(Rot4.North, null);
				this.uiIcon = (Texture2D)material.mainTexture;
			}
		}

		// Token: 0x04000732 RID: 1842
		[MustTranslate]
		public string overrideLabel;

		// Token: 0x04000733 RID: 1843
		public GraphicData graphicData;

		// Token: 0x04000734 RID: 1844
		[NoTranslate]
		public string uiIconPath;

		// Token: 0x04000735 RID: 1845
		[NoTranslate]
		public string wornGraphicPath;

		// Token: 0x04000736 RID: 1846
		public Color color;

		// Token: 0x04000737 RID: 1847
		private Graphic graphic;

		// Token: 0x04000738 RID: 1848
		private Texture2D uiIcon;

		// Token: 0x04000739 RID: 1849
		private StyleCategoryDef cachedCategory;

		// Token: 0x0400073A RID: 1850
		private bool? hasCategory;
	}
}
