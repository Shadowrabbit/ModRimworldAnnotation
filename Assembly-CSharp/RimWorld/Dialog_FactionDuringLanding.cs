using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019DB RID: 6619
	public class Dialog_FactionDuringLanding : Window
	{
		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x06009245 RID: 37445 RVA: 0x0003D168 File Offset: 0x0003B368
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x06009246 RID: 37446 RVA: 0x00062066 File Offset: 0x00060266
		public Dialog_FactionDuringLanding()
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06009247 RID: 37447 RVA: 0x002A0794 File Offset: 0x0029E994
		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents_NewTemp(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - this.CloseButSize.y), ref this.scrollPosition, ref this.scrollViewHeight, null);
		}

		// Token: 0x04005C7D RID: 23677
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04005C7E RID: 23678
		private float scrollViewHeight;
	}
}
