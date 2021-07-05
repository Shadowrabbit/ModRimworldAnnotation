using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200044A RID: 1098
	public class Dialog_ResolutionConfirm : Window
	{
		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002143 RID: 8515 RVA: 0x000D0136 File Offset: 0x000CE336
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002144 RID: 8516 RVA: 0x000D014A File Offset: 0x000CE34A
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000D015B File Offset: 0x000CE35B
		private Dialog_ResolutionConfirm()
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x000D0183 File Offset: 0x000CE383
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x000D01B2 File Offset: 0x000CE3B2
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x000D01D7 File Offset: 0x000CE3D7
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x000D0208 File Offset: 0x000CE408
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

		// Token: 0x0600214A RID: 8522 RVA: 0x000D0310 File Offset: 0x000CE510
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
				}));
			}
			ResolutionUtility.SetResolutionRaw(this.oldRes.x, this.oldRes.z, this.oldFullscreen);
			Prefs.FullScreen = this.oldFullscreen;
			Prefs.ScreenWidth = this.oldRes.x;
			Prefs.ScreenHeight = this.oldRes.z;
			Prefs.UIScale = this.oldUIScale;
			GenUI.ClearLabelWidthCache();
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000D03DC File Offset: 0x000CE5DC
		public override void WindowUpdate()
		{
			if (this.TimeUntilRevert <= 0f)
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x040014A2 RID: 5282
		private float startTime;

		// Token: 0x040014A3 RID: 5283
		private IntVec2 oldRes;

		// Token: 0x040014A4 RID: 5284
		private bool oldFullscreen;

		// Token: 0x040014A5 RID: 5285
		private float oldUIScale;

		// Token: 0x040014A6 RID: 5286
		private const float RevertTime = 10f;
	}
}
