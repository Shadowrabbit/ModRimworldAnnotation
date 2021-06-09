using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200077E RID: 1918
	[StaticConstructorOnStartup]
	public class TabRecord
	{
		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x00025EB2 File Offset: 0x000240B2
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

		// Token: 0x06003027 RID: 12327 RVA: 0x00025ECE File Offset: 0x000240CE
		public TabRecord(string label, Action clickedAction, bool selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selected = selected;
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x00025EF6 File Offset: 0x000240F6
		public TabRecord(string label, Action clickedAction, Func<bool> selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selectedGetter = selected;
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x0013E174 File Offset: 0x0013C374
		public void Draw(Rect rect)
		{
			Rect drawRect = new Rect(rect);
			drawRect.width = 30f;
			drawRect.xMax = Widgets.AdjustCoordToUIScalingCeil(drawRect.xMax);
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

		// Token: 0x040020B6 RID: 8374
		public string label = "Tab";

		// Token: 0x040020B7 RID: 8375
		public Action clickedAction;

		// Token: 0x040020B8 RID: 8376
		public bool selected;

		// Token: 0x040020B9 RID: 8377
		public Func<bool> selectedGetter;

		// Token: 0x040020BA RID: 8378
		private const float TabEndWidth = 30f;

		// Token: 0x040020BB RID: 8379
		private const float TabMiddleGraphicWidth = 4f;

		// Token: 0x040020BC RID: 8380
		private static readonly Texture2D TabAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TabAtlas", true);
	}
}
