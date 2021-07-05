using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000794 RID: 1940
	public class Dialog_ResolutionConfirm : Window
	{
		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x00026883 File Offset: 0x00024A83
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x00026897 File Offset: 0x00024A97
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x000268A8 File Offset: 0x00024AA8
		private Dialog_ResolutionConfirm()
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x000268D0 File Offset: 0x00024AD0
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x000268FF File Offset: 0x00024AFF
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x00026924 File Offset: 0x00024B24
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x001438DC File Offset: 0x00141ADC
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			string label = "ConfirmResolutionChange".Translate(Mathf.CeilToInt(this.TimeUntilRevert));
			Widgets.Label(new Rect(0f, 0f, inRect.width, inRect.height), label);
			if (Widgets.ButtonText(new Rect(0f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionKeep".Translate(), true, true, true))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionRevert".Translate(), true, true, true))
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x001439E4 File Offset: 0x00141BE4
		private void Revert()
		{
			if (Prefs.LogVerbose)
			{
				Log.Message(string.Concat(new object[]
				{
					"Reverting screen settings to ",
					this.oldRes.x,
					"x",
					this.oldRes.z,
					", fs=",
					this.oldFullscreen.ToString()
				}), false);
			}
			ResolutionUtility.SetResolutionRaw(this.oldRes.x, this.oldRes.z, this.oldFullscreen);
			Prefs.FullScreen = this.oldFullscreen;
			Prefs.ScreenWidth = this.oldRes.x;
			Prefs.ScreenHeight = this.oldRes.z;
			Prefs.UIScale = this.oldUIScale;
			GenUI.ClearLabelWidthCache();
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x00026953 File Offset: 0x00024B53
		public override void WindowUpdate()
		{
			if (this.TimeUntilRevert <= 0f)
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x040021A7 RID: 8615
		private float startTime;

		// Token: 0x040021A8 RID: 8616
		private IntVec2 oldRes;

		// Token: 0x040021A9 RID: 8617
		private bool oldFullscreen;

		// Token: 0x040021AA RID: 8618
		private float oldUIScale;

		// Token: 0x040021AB RID: 8619
		private const float RevertTime = 10f;
	}
}
