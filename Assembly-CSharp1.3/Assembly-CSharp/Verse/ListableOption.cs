using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000433 RID: 1075
	public class ListableOption
	{
		// Token: 0x06002065 RID: 8293 RVA: 0x000C8CE6 File Offset: 0x000C6EE6
		public ListableOption(string label, Action action, string uiHighlightTag = null)
		{
			this.label = label;
			this.action = action;
			this.uiHighlightTag = uiHighlightTag;
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x000C8D10 File Offset: 0x000C6F10
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

		// Token: 0x040013A5 RID: 5029
		public string label;

		// Token: 0x040013A6 RID: 5030
		public Action action;

		// Token: 0x040013A7 RID: 5031
		private string uiHighlightTag;

		// Token: 0x040013A8 RID: 5032
		public float minHeight = 45f;
	}
}
