using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007CF RID: 1999
	public class CreditRecord_Role : CreditsEntry
	{
		// Token: 0x0600322F RID: 12847 RVA: 0x0002764F File Offset: 0x0002584F
		public CreditRecord_Role()
		{
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x0002769A File Offset: 0x0002589A
		public CreditRecord_Role(string roleKey, string creditee, string extra = null)
		{
			this.roleKey = roleKey;
			this.creditee = creditee;
			this.extra = extra;
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x000276B7 File Offset: 0x000258B7
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

		// Token: 0x06003232 RID: 12850 RVA: 0x0014CD68 File Offset: 0x0014AF68
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
					Widgets.Label(rect2, this.roleKey.Translate());
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

		// Token: 0x040022DE RID: 8926
		public string roleKey;

		// Token: 0x040022DF RID: 8927
		public string creditee;

		// Token: 0x040022E0 RID: 8928
		public string extra;

		// Token: 0x040022E1 RID: 8929
		public bool displayKey;

		// Token: 0x040022E2 RID: 8930
		public bool compressed;
	}
}
