using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007AC RID: 1964
	public class ImmediateWindow : Window
	{
		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06003163 RID: 12643 RVA: 0x00026F26 File Offset: 0x00025126
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06003164 RID: 12644 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x00145BC4 File Offset: 0x00143DC4
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

		// Token: 0x06003166 RID: 12646 RVA: 0x00026F33 File Offset: 0x00025133
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}

		// Token: 0x04002226 RID: 8742
		public Action doWindowFunc;
	}
}
