using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E7 RID: 4839
	public class Dialog_FactionDuringLanding : Window
	{
		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x060073EE RID: 29678 RVA: 0x0015CA40 File Offset: 0x0015AC40
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x060073EF RID: 29679 RVA: 0x002734F0 File Offset: 0x002716F0
		public Dialog_FactionDuringLanding()
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x060073F0 RID: 29680 RVA: 0x00273518 File Offset: 0x00271718
		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - Window.CloseButSize.y), ref this.scrollPosition, ref this.scrollViewHeight, null);
		}

		// Token: 0x04003FBA RID: 16314
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04003FBB RID: 16315
		private float scrollViewHeight;
	}
}
