using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A15 RID: 6677
	public static class ResolutionUtility
	{
		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x06009388 RID: 37768 RVA: 0x002A8534 File Offset: 0x002A6734
		public static Resolution NativeResolution
		{
			get
			{
				Resolution[] resolutions = Screen.resolutions;
				if (resolutions.Length == 0)
				{
					return Screen.currentResolution;
				}
				Resolution result = resolutions[0];
				for (int i = 1; i < resolutions.Length; i++)
				{
					if (resolutions[i].width > result.width || (resolutions[i].width == result.width && resolutions[i].height > result.height))
					{
						result = resolutions[i];
					}
				}
				return result;
			}
		}

		// Token: 0x06009389 RID: 37769 RVA: 0x002A85B0 File Offset: 0x002A67B0
		public static void SafeSetResolution(Resolution res)
		{
			if (Screen.width == res.width && Screen.height == res.height)
			{
				return;
			}
			IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
			ResolutionUtility.SetResolutionRaw(res.width, res.height, Screen.fullScreen);
			Prefs.ScreenWidth = res.width;
			Prefs.ScreenHeight = res.height;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(oldRes));
		}

		// Token: 0x0600938A RID: 37770 RVA: 0x002A862C File Offset: 0x002A682C
		public static void SafeSetFullscreen(bool fullScreen)
		{
			if (Screen.fullScreen == fullScreen)
			{
				return;
			}
			bool fullScreen2 = Screen.fullScreen;
			Screen.fullScreen = fullScreen;
			Prefs.FullScreen = fullScreen;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreen2));
		}

		// Token: 0x0600938B RID: 37771 RVA: 0x002A8664 File Offset: 0x002A6864
		public static void SafeSetUIScale(float newScale)
		{
			if (Prefs.UIScale == newScale)
			{
				return;
			}
			float uiscale = Prefs.UIScale;
			Prefs.UIScale = newScale;
			GenUI.ClearLabelWidthCache();
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(uiscale));
		}

		// Token: 0x0600938C RID: 37772 RVA: 0x00062D89 File Offset: 0x00060F89
		public static bool UIScaleSafeWithResolution(float scale, int w, int h)
		{
			return (float)w / scale >= 1024f && (float)h / scale >= 768f;
		}

		// Token: 0x0600938D RID: 37773 RVA: 0x002A869C File Offset: 0x002A689C
		public static void SetResolutionRaw(int w, int h, bool fullScreen)
		{
			if (Application.isBatchMode)
			{
				return;
			}
			if (w <= 0 || h <= 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to set resolution to ",
					w,
					"x",
					h
				}), false);
				return;
			}
			if (Screen.width != w || Screen.height != h || Screen.fullScreen != fullScreen)
			{
				Screen.SetResolution(w, h, fullScreen);
			}
		}

		// Token: 0x0600938E RID: 37774 RVA: 0x002A8710 File Offset: 0x002A6910
		public static void SetNativeResolutionRaw()
		{
			Resolution nativeResolution = ResolutionUtility.NativeResolution;
			ResolutionUtility.SetResolutionRaw(nativeResolution.width, nativeResolution.height, true);
		}

		// Token: 0x0600938F RID: 37775 RVA: 0x002A8738 File Offset: 0x002A6938
		public static float GetRecommendedUIScale(int screenWidth, int screenHeight)
		{
			if (screenWidth == 0 || screenHeight == 0)
			{
				Resolution nativeResolution = ResolutionUtility.NativeResolution;
				screenWidth = nativeResolution.width;
				screenHeight = nativeResolution.height;
			}
			if (screenWidth <= 1024 || screenHeight <= 768)
			{
				return 1f;
			}
			for (int i = Dialog_Options.UIScales.Length - 1; i >= 0; i--)
			{
				int num = Mathf.FloorToInt((float)screenWidth / Dialog_Options.UIScales[i]);
				int num2 = Mathf.FloorToInt((float)screenHeight / Dialog_Options.UIScales[i]);
				if (num >= 1700 && num2 >= 910)
				{
					return Dialog_Options.UIScales[i];
				}
			}
			return 1f;
		}

		// Token: 0x06009390 RID: 37776 RVA: 0x002A87C8 File Offset: 0x002A69C8
		public static void Update()
		{
			if (RealTime.frameCount % 30 == 0 && !LongEventHandler.AnyEventNowOrWaiting && !Screen.fullScreen)
			{
				bool flag = false;
				if (Screen.width != Prefs.ScreenWidth)
				{
					Prefs.ScreenWidth = Screen.width;
					flag = true;
				}
				if (Screen.height != Prefs.ScreenHeight)
				{
					Prefs.ScreenHeight = Screen.height;
					flag = true;
				}
				if (flag)
				{
					Prefs.Save();
				}
			}
		}

		// Token: 0x04005D78 RID: 23928
		public const int MinResolutionWidth = 1024;

		// Token: 0x04005D79 RID: 23929
		public const int MinResolutionHeight = 768;

		// Token: 0x04005D7A RID: 23930
		public const int MinRecommendedResolutionWidth = 1700;

		// Token: 0x04005D7B RID: 23931
		public const int MinRecommendedResolutionHeight = 910;
	}
}
