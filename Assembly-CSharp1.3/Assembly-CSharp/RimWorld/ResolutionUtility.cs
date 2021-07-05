using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012FD RID: 4861
	public static class ResolutionUtility
	{
		// Token: 0x1700147E RID: 5246
		// (get) Token: 0x060074C3 RID: 29891 RVA: 0x0027BD34 File Offset: 0x00279F34
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

		// Token: 0x060074C4 RID: 29892 RVA: 0x0027BDB0 File Offset: 0x00279FB0
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

		// Token: 0x060074C5 RID: 29893 RVA: 0x0027BE2C File Offset: 0x0027A02C
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

		// Token: 0x060074C6 RID: 29894 RVA: 0x0027BE64 File Offset: 0x0027A064
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

		// Token: 0x060074C7 RID: 29895 RVA: 0x0027BE9B File Offset: 0x0027A09B
		public static bool UIScaleSafeWithResolution(float scale, int w, int h)
		{
			return (float)w / scale >= 1024f && (float)h / scale >= 768f;
		}

		// Token: 0x060074C8 RID: 29896 RVA: 0x0027BEB8 File Offset: 0x0027A0B8
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
				}));
				return;
			}
			if (Screen.width != w || Screen.height != h || Screen.fullScreen != fullScreen)
			{
				Screen.SetResolution(w, h, fullScreen);
			}
		}

		// Token: 0x060074C9 RID: 29897 RVA: 0x0027BF28 File Offset: 0x0027A128
		public static void SetNativeResolutionRaw()
		{
			Resolution nativeResolution = ResolutionUtility.NativeResolution;
			ResolutionUtility.SetResolutionRaw(nativeResolution.width, nativeResolution.height, true);
		}

		// Token: 0x060074CA RID: 29898 RVA: 0x0027BF50 File Offset: 0x0027A150
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

		// Token: 0x060074CB RID: 29899 RVA: 0x0027BFE0 File Offset: 0x0027A1E0
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

		// Token: 0x04004058 RID: 16472
		public const int MinResolutionWidth = 1024;

		// Token: 0x04004059 RID: 16473
		public const int MinResolutionHeight = 768;

		// Token: 0x0400405A RID: 16474
		public const int MinRecommendedResolutionWidth = 1700;

		// Token: 0x0400405B RID: 16475
		public const int MinRecommendedResolutionHeight = 910;
	}
}
