using System;
using System.IO;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020004E5 RID: 1253
	public static class ScreenshotTaker
	{
		// Token: 0x060025D8 RID: 9688 RVA: 0x000EA8EC File Offset: 0x000E8AEC
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

		// Token: 0x060025D9 RID: 9689 RVA: 0x000EA95F File Offset: 0x000E8B5F
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = (ScreenshotTaker.suppressMessage = true);
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000EA970 File Offset: 0x000E8B70
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
					Log.Warning("Could not take Steam screenshot. Steam offline? Taking normal screenshot. Exception: " + arg);
					ScreenshotTaker.TakeNonSteamShot();
					return;
				}
			}
			ScreenshotTaker.TakeNonSteamShot();
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x000EA9C0 File Offset: 0x000E8BC0
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
				Log.Error("Failed to save screenshot in " + screenshotFolderPath + "\nException follows:\n" + ex.ToString());
			}
		}

		// Token: 0x040017B0 RID: 6064
		private static int lastShotFrame = -999;

		// Token: 0x040017B1 RID: 6065
		private static int screenshotCount = 0;

		// Token: 0x040017B2 RID: 6066
		private static string lastShotFilePath;

		// Token: 0x040017B3 RID: 6067
		private static bool suppressMessage;

		// Token: 0x040017B4 RID: 6068
		private static bool takeScreenshot;
	}
}
