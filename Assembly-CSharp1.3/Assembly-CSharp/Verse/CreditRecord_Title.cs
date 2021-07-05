using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000476 RID: 1142
	public class CreditRecord_Title : CreditsEntry
	{
		// Token: 0x06002271 RID: 8817 RVA: 0x000DA60B File Offset: 0x000D880B
		public CreditRecord_Title()
		{
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000DA8A4 File Offset: 0x000D8AA4
		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000DA8B3 File Offset: 0x000D8AB3
		public override float DrawHeight(float width)
		{
			return 100f;
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000DA8BC File Offset: 0x000D8ABC
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

		// Token: 0x040015C0 RID: 5568
		public string title;
	}
}
