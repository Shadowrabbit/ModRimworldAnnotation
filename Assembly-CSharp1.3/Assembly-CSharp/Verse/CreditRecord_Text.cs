using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000473 RID: 1139
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x06002263 RID: 8803 RVA: 0x000DA60B File Offset: 0x000D880B
		public CreditRecord_Text()
		{
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000DA613 File Offset: 0x000D8813
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000DA629 File Offset: 0x000D8829
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000DA637 File Offset: 0x000D8837
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x040015B5 RID: 5557
		public string text;

		// Token: 0x040015B6 RID: 5558
		public TextAnchor anchor;
	}
}
