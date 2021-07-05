using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000475 RID: 1141
	public class CreditRecord_RoleTwoCols : CreditsEntry
	{
		// Token: 0x0600226C RID: 8812 RVA: 0x000DA60B File Offset: 0x000D880B
		public CreditRecord_RoleTwoCols()
		{
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000DA79E File Offset: 0x000D899E
		public CreditRecord_RoleTwoCols(string creditee1, string creditee2, string extra = null)
		{
			this.creditee1 = creditee1;
			this.creditee2 = creditee2;
			this.extra = extra;
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000DA7BB File Offset: 0x000D89BB
		public override float DrawHeight(float width)
		{
			if (!this.compressed)
			{
				return 50f;
			}
			return Text.CalcHeight(this.creditee2, width * 0.5f);
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000DA7E0 File Offset: 0x000D89E0
		public override void Draw(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = rect;
			rect2.width = 0f;
			rect2.width = rect.width / 2f;
			Widgets.Label(rect2, this.creditee1);
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			Widgets.Label(rect3, this.creditee2);
			if (!this.extra.NullOrEmpty())
			{
				Rect rect4 = rect3;
				rect4.yMin += 28f;
				Text.Font = GameFont.Tiny;
				GUI.color = new Color(0.7f, 0.7f, 0.7f);
				Widgets.Label(rect4, this.extra);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000DA89A File Offset: 0x000D8A9A
		public CreditRecord_RoleTwoCols Compress()
		{
			this.compressed = true;
			return this;
		}

		// Token: 0x040015BC RID: 5564
		public string creditee1;

		// Token: 0x040015BD RID: 5565
		public string creditee2;

		// Token: 0x040015BE RID: 5566
		public string extra;

		// Token: 0x040015BF RID: 5567
		public bool compressed;
	}
}
