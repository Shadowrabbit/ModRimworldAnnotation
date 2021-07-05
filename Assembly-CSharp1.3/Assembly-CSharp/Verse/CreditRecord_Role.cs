using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000474 RID: 1140
	public class CreditRecord_Role : CreditsEntry
	{
		// Token: 0x06002267 RID: 8807 RVA: 0x000DA60B File Offset: 0x000D880B
		public CreditRecord_Role()
		{
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000DA656 File Offset: 0x000D8856
		public CreditRecord_Role(string roleKey, string creditee, string extra = null)
		{
			this.roleKey = roleKey;
			this.creditee = creditee;
			this.extra = extra;
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000DA673 File Offset: 0x000D8873
		public override float DrawHeight(float width)
		{
			if (this.roleKey.NullOrEmpty())
			{
				width *= 0.5f;
			}
			if (!this.compressed)
			{
				return 50f;
			}
			return Text.CalcHeight(this.creditee, width * 0.5f);
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x000DA6AC File Offset: 0x000D88AC
		public override void Draw(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = rect;
			rect2.width = 0f;
			if (!this.roleKey.NullOrEmpty())
			{
				rect2.width = rect.width / 2f;
				if (this.displayKey)
				{
					Widgets.Label(rect2, this.roleKey);
				}
			}
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			if (this.roleKey.NullOrEmpty())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			Widgets.Label(rect3, this.creditee);
			Text.Anchor = TextAnchor.MiddleLeft;
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

		// Token: 0x0600226B RID: 8811 RVA: 0x000DA794 File Offset: 0x000D8994
		public CreditRecord_Role Compress()
		{
			this.compressed = true;
			return this;
		}

		// Token: 0x040015B7 RID: 5559
		public string roleKey;

		// Token: 0x040015B8 RID: 5560
		public string creditee;

		// Token: 0x040015B9 RID: 5561
		public string extra;

		// Token: 0x040015BA RID: 5562
		public bool displayKey;

		// Token: 0x040015BB RID: 5563
		public bool compressed;
	}
}
