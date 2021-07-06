using System;
using System.IO;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000891 RID: 2193
	public static class ScreenshotTaker
	{
		// Token: 0x06003668 RID: 13928 RVA: 0x0015BD54 File Offset: 0x00159F54
		public static void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (KeyBindingDefOf.TakeScreenshot.JustPressed || ScreenshotTaker.takeScreenshot)
			{
				ScreenshotTaker.TakeShot();
				ScreenshotTaker.takeScreenshot = false;
			}
			if (Time.frameCount == ScreenshotTaker.lastShotFrame + 1)
			{
				if (ScreenshotTaker.suppressMessage)
				{
					ScreenshotTaker.suppressMessage = false;
					return;
				}
				Messages.Message("MessageScreenshotSavedAs".Translate(ScreenshotTaker.lastShotFilePath), MessageTypeDefOf.TaskCompletion, false);
			}
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x0002A407 File Offset: 0x00028607
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = (ScreenshotTaker.suppressMessage = true);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x0015BDC8 File Offset: 0x00159FC8
		private static void TakeShot()
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				try
				{
					SteamScreenshots.TriggerScreenshot();
					return;
				}
				catch (Exception arg)
				{
					Log.Warning("Could not take Steam screenshot. Steam offline? Taking normal screenshot. Exception: " + arg, false);
					ScreenshotTaker.TakeNonSteamShot();
					return;
				}
			}
			ScreenshotTaker.TakeNonSteamShot();
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x0015BE18 File Offset: 0x0015A018
		private static void TakeNonSteamShot()
		{
			string screenshotFolderPath = GenFilePaths.ScreenshotFolderPath;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(screenshotFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				string text;
				do
				{
					ScreenshotTaker.screenshotCount++;
					text = string.Concat(new object[]
					{
						screenshotFolderPath,
						Path.DirectorySeparatorChar.ToString(),
						"screenshot",
						ScreenshotTaker.screenshotCount,
						".png"
					});
				}
				while (File.Exists(text));
				ScreenCapture.CaptureScreenshot(text);
				ScreenshotTaker.lastShotFrame = Time.frameCount;
				ScreenshotTaker.lastShotFilePath = text;
			}
			catch (Exception ex)
			{
				Log.Error("Failed to save screenshot in " + screenshotFolderPath + "\nException follows:\n" + ex.ToString(), false);
			}
		}

		// Token: 0x040025EC RID: 9708
		private static int lastShotFrame = -999;

		// Token: 0x040025ED RID: 9709
		private static int screenshotCount = 0;

		// Token: 0x040025EE RID: 9710
		private static string lastShotFilePath;

		// Token: 0x040025EF RID: 9711
		private static bool suppressMessage;

		// Token: 0x040025F0 RID: 9712
		private static bool takeScreenshot;
	}
}
