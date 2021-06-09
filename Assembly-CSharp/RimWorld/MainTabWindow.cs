using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB5 RID: 4021
	public abstract class MainTabWindow : Window
	{
		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x060057F9 RID: 22521 RVA: 0x0003D168 File Offset: 0x0003B368
		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x060057FA RID: 22522 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Left;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x060057FB RID: 22523 RVA: 0x001CEEA0 File Offset: 0x001CD0A0
		public override Vector2 InitialSize
		{
			get
			{
				Vector2 requestedTabSize = this.RequestedTabSize;
				if (requestedTabSize.y > (float)(UI.screenHeight - 35))
				{
					requestedTabSize.y = (float)(UI.screenHeight - 35);
				}
				if (requestedTabSize.x > (float)UI.screenWidth)
				{
					requestedTabSize.x = (float)UI.screenWidth;
				}
				return requestedTabSize;
			}
		}

		// Token: 0x060057FC RID: 22524 RVA: 0x0003D179 File Offset: 0x0003B379
		public MainTabWindow()
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = SoundDefOf.TabClose;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x060057FD RID: 22525 RVA: 0x00026FE3 File Offset: 0x000251E3
		public override void DoWindowContents(Rect inRect)
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x060057FE RID: 22526 RVA: 0x001CEEF4 File Offset: 0x001CD0F4
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			if (this.Anchor == MainTabWindowAnchor.Left)
			{
				this.windowRect.x = 0f;
			}
			else
			{
				this.windowRect.x = (float)UI.screenWidth - this.windowRect.width;
			}
			this.windowRect.y = (float)(UI.screenHeight - 35) - this.windowRect.height;
		}

		// Token: 0x040039F6 RID: 14838
		public MainButtonDef def;
	}
}
