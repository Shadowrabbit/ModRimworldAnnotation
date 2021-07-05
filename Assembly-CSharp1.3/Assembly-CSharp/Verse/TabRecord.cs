using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200043B RID: 1083
	[StaticConstructorOnStartup]
	public class TabRecord
	{
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600207D RID: 8317 RVA: 0x000C97E7 File Offset: 0x000C79E7
		public bool Selected
		{
			get
			{
				if (this.selectedGetter == null)
				{
					return this.selected;
				}
				return this.selectedGetter();
			}
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000C9803 File Offset: 0x000C7A03
		public TabRecord(string label, Action clickedAction, bool selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selected = selected;
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000C982B File Offset: 0x000C7A2B
		public TabRecord(string label, Action clickedAction, Func<bool> selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selectedGetter = selected;
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x000C9854 File Offset: 0x000C7A54
		public void Draw(Rect rect)
		{
			Rect drawRect = new Rect(rect);
			drawRect.width = 30f;
			Rect drawRect2 = new Rect(rect);
			drawRect2.width = 30f;
			drawRect2.x = rect.x + rect.width - 30f;
			Rect uvRect = new Rect(0.53125f, 0f, 0.46875f, 1f);
			Rect drawRect3 = new Rect(rect);
			drawRect3.x += drawRect.width;
			drawRect3.width -= 60f;
			drawRect3.xMin = Widgets.AdjustCoordToUIScalingFloor(drawRect3.xMin);
			drawRect3.xMax = Widgets.AdjustCoordToUIScalingCeil(drawRect3.xMax);
			Rect uvRect2 = new Rect(30f, 0f, 4f, (float)TabRecord.TabAtlas.height).ToUVRect(new Vector2((float)TabRecord.TabAtlas.width, (float)TabRecord.TabAtlas.height));
			Widgets.DrawTexturePart(drawRect, new Rect(0f, 0f, 0.46875f, 1f), TabRecord.TabAtlas);
			Widgets.DrawTexturePart(drawRect3, uvRect2, TabRecord.TabAtlas);
			Widgets.DrawTexturePart(drawRect2, uvRect, TabRecord.TabAtlas);
			GUI.color = (this.labelColor ?? Color.white);
			Rect rect2 = rect;
			rect2.width -= 10f;
			if (Mouse.IsOver(rect2))
			{
				GUI.color = Color.yellow;
				rect2.x += 2f;
				rect2.y -= 2f;
			}
			Text.WordWrap = false;
			Widgets.Label(rect2, this.label);
			Text.WordWrap = true;
			GUI.color = Color.white;
			if (!this.Selected)
			{
				Rect drawRect4 = new Rect(rect);
				drawRect4.y += rect.height;
				drawRect4.y -= 1f;
				drawRect4.height = 1f;
				Rect uvRect3 = new Rect(0.5f, 0.01f, 0.01f, 0.01f);
				Widgets.DrawTexturePart(drawRect4, uvRect3, TabRecord.TabAtlas);
			}
		}

		// Token: 0x040013C1 RID: 5057
		public string label = "Tab";

		// Token: 0x040013C2 RID: 5058
		public Action clickedAction;

		// Token: 0x040013C3 RID: 5059
		public bool selected;

		// Token: 0x040013C4 RID: 5060
		public Func<bool> selectedGetter;

		// Token: 0x040013C5 RID: 5061
		public Color? labelColor;

		// Token: 0x040013C6 RID: 5062
		private const float TabEndWidth = 30f;

		// Token: 0x040013C7 RID: 5063
		private const float TabMiddleGraphicWidth = 4f;

		// Token: 0x040013C8 RID: 5064
		private static readonly Texture2D TabAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TabAtlas", true);
	}
}
