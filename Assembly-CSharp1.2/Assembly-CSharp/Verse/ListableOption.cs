using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000775 RID: 1909
	public class ListableOption
	{
		// Token: 0x06003010 RID: 12304 RVA: 0x00025DF9 File Offset: 0x00023FF9
		public ListableOption(string label, Action action, string uiHighlightTag = null)
		{
			this.label = label;
			this.action = action;
			this.uiHighlightTag = uiHighlightTag;
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x0013D76C File Offset: 0x0013B96C
		public virtual float DrawOption(Vector2 pos, float width)
		{
			float b = Text.CalcHeight(this.label, width);
			float num = Mathf.Max(this.minHeight, b);
			Rect rect = new Rect(pos.x, pos.y, width, num);
			if (Widgets.ButtonText(rect, this.label, true, true, true))
			{
				this.action();
			}
			if (this.uiHighlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.uiHighlightTag);
			}
			return num;
		}

		// Token: 0x04002094 RID: 8340
		public string label;

		// Token: 0x04002095 RID: 8341
		public Action action;

		// Token: 0x04002096 RID: 8342
		private string uiHighlightTag;

		// Token: 0x04002097 RID: 8343
		public float minHeight = 45f;
	}
}
