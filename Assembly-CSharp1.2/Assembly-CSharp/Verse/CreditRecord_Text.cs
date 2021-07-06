using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007CE RID: 1998
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x0600322B RID: 12843 RVA: 0x0002764F File Offset: 0x0002584F
		public CreditRecord_Text()
		{
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x00027657 File Offset: 0x00025857
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x0002766D File Offset: 0x0002586D
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x0002767B File Offset: 0x0002587B
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x040022DC RID: 8924
		public string text;

		// Token: 0x040022DD RID: 8925
		public TextAnchor anchor;
	}
}
