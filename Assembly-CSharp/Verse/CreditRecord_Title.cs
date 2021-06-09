using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007D0 RID: 2000
	public class CreditRecord_Title : CreditsEntry
	{
		// Token: 0x06003233 RID: 12851 RVA: 0x0002764F File Offset: 0x0002584F
		public CreditRecord_Title()
		{
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x000276EF File Offset: 0x000258EF
		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x000276FE File Offset: 0x000258FE
		public override float DrawHeight(float width)
		{
			return 100f;
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x0014CE58 File Offset: 0x0014B058
		public override void Draw(Rect rect)
		{
			rect.yMin += 31f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(rect.x + 10f, Mathf.Round(rect.yMax) - 14f, rect.width - 20f);
			GUI.color = Color.white;
		}

		// Token: 0x040022E3 RID: 8931
		public string title;
	}
}
