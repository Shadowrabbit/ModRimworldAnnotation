using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A7 RID: 5031
	[StaticConstructorOnStartup]
	public class Listing_ResourceReadout : Listing_Tree
	{
		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x06007A7E RID: 31358 RVA: 0x002B3520 File Offset: 0x002B1720
		protected override float LabelWidth
		{
			get
			{
				return base.ColumnWidth;
			}
		}

		// Token: 0x06007A7F RID: 31359 RVA: 0x002B3528 File Offset: 0x002B1728
		public Listing_ResourceReadout(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007A80 RID: 31360 RVA: 0x002B3538 File Offset: 0x002B1738
		public void DoCategory(TreeNode_ThingCategory node, int nestLevel, int openMask)
		{
			int countIn = this.map.resourceCounter.GetCountIn(node.catDef);
			if (countIn == 0)
			{
				return;
			}
			base.OpenCloseWidget(node, nestLevel, openMask);
			Rect rect = new Rect(0f, this.curY, this.LabelWidth, this.lineHeight)
			{
				xMin = base.XAtIndentLevel(nestLevel) + 18f
			};
			Rect position = rect;
			position.width = 80f;
			position.yMax -= 3f;
			position.yMin += 3f;
			GUI.DrawTexture(position, Listing_ResourceReadout.SolidCategoryBG);
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(node.catDef.LabelCap, node.catDef.GetHashCode()));
			}
			Rect position2 = new Rect(rect);
			position2.width = (position2.height = 28f);
			position2.y = rect.y + rect.height / 2f - position2.height / 2f;
			GUI.DrawTexture(position2, node.catDef.icon);
			Widgets.Label(new Rect(rect)
			{
				xMin = position2.xMax + 6f
			}, countIn.ToStringCached());
			base.EndLine();
			if (this.IsOpen(node, openMask))
			{
				this.DoCategoryChildren(node, nestLevel + 1, openMask);
			}
		}

		// Token: 0x06007A81 RID: 31361 RVA: 0x002B36BC File Offset: 0x002B18BC
		public void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask)
		{
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in node.ChildCategoryNodes)
			{
				if (!treeNode_ThingCategory.catDef.resourceReadoutRoot)
				{
					this.DoCategory(treeNode_ThingCategory, indentLevel, openMask);
				}
			}
			foreach (ThingDef thingDef in node.catDef.childThingDefs)
			{
				if (thingDef.PlayerAcquirable)
				{
					this.DoThingDef(thingDef, indentLevel + 1);
				}
			}
		}

		// Token: 0x06007A82 RID: 31362 RVA: 0x002B376C File Offset: 0x002B196C
		private void DoThingDef(ThingDef thingDef, int nestLevel)
		{
			int count = this.map.resourceCounter.GetCount(thingDef);
			if (count == 0)
			{
				return;
			}
			Rect rect = new Rect(0f, this.curY, this.LabelWidth, this.lineHeight)
			{
				xMin = base.XAtIndentLevel(nestLevel) + 18f
			};
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => thingDef.LabelCap + ": " + thingDef.description.CapitalizeFirst(), (int)thingDef.shortHash));
			}
			Rect rect2 = new Rect(rect);
			rect2.width = (rect2.height = 28f);
			rect2.y = rect.y + rect.height / 2f - rect2.height / 2f;
			Widgets.ThingIcon(rect2, thingDef, null, null, 1f, null);
			Widgets.Label(new Rect(rect)
			{
				xMin = rect2.xMax + 6f
			}, count.ToStringCached());
			base.EndLine();
		}

		// Token: 0x040043BB RID: 17339
		private Map map;

		// Token: 0x040043BC RID: 17340
		private static Texture2D SolidCategoryBG = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.6f));
	}
}
