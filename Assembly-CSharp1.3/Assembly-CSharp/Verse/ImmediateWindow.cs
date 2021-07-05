using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200045A RID: 1114
	public class ImmediateWindow : Window
	{
		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060021A8 RID: 8616 RVA: 0x000D2834 File Offset: 0x000D0A34
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060021A9 RID: 8617 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000D2844 File Offset: 0x000D0A44
		public ImmediateWindow()
		{
			this.doCloseButton = false;
			this.doCloseX = false;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.focusWhenOpened = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000D2896 File Offset: 0x000D0A96
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000D28A3 File Offset: 0x000D0AA3
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			Action action = this.doClickOutsideFunc;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0400150B RID: 5387
		public Action doWindowFunc;

		// Token: 0x0400150C RID: 5388
		public Action doClickOutsideFunc;
	}
}
