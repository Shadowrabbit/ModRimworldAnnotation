using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012EA RID: 4842
	public class Dialog_Ideo : Window
	{
		// Token: 0x1700145E RID: 5214
		// (get) Token: 0x06007435 RID: 29749 RVA: 0x0027626E File Offset: 0x0027446E
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, Mathf.Min(1000f, (float)UI.screenHeight));
			}
		}

		// Token: 0x06007436 RID: 29750 RVA: 0x0027628A File Offset: 0x0027448A
		public Dialog_Ideo(Ideo ideo)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.ideo = ideo;
		}

		// Token: 0x06007437 RID: 29751 RVA: 0x0026F782 File Offset: 0x0026D982
		public override void PostClose()
		{
			base.PostClose();
			IdeoUIUtility.UnselectCurrent();
		}

		// Token: 0x06007438 RID: 29752 RVA: 0x002762BC File Offset: 0x002744BC
		public override void DoWindowContents(Rect inRect)
		{
			inRect.height -= Window.CloseButSize.y;
			IdeoUIUtility.DoIdeoDetails(inRect, this.ideo, ref this.scrollPosition, ref this.viewHeight, false);
		}

		// Token: 0x04003FF6 RID: 16374
		private Ideo ideo;

		// Token: 0x04003FF7 RID: 16375
		private float viewHeight;

		// Token: 0x04003FF8 RID: 16376
		private Vector2 scrollPosition;
	}
}
