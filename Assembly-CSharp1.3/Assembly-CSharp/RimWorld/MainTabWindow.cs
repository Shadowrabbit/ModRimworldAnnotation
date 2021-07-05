using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A8B RID: 2699
	public abstract class MainTabWindow : Window
	{
		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x0015CA40 File Offset: 0x0015AC40
		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06004072 RID: 16498 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Left;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06004073 RID: 16499 RVA: 0x0015CA54 File Offset: 0x0015AC54
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

		// Token: 0x06004074 RID: 16500 RVA: 0x0015CAA5 File Offset: 0x0015ACA5
		public MainTabWindow()
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = SoundDefOf.TabClose;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0015CADC File Offset: 0x0015ACDC
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

		// Token: 0x06004076 RID: 16502 RVA: 0x0015CB46 File Offset: 0x0015AD46
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.def.closesWorldView)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
			}
		}

		// Token: 0x04002511 RID: 9489
		public MainButtonDef def;
	}
}
