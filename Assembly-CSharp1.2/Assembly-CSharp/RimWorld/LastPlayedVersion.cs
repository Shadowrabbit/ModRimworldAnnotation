using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D81 RID: 7553
	public static class LastPlayedVersion
	{
		// Token: 0x1700192D RID: 6445
		// (get) Token: 0x0600A42C RID: 42028 RVA: 0x0006CD8B File Offset: 0x0006AF8B
		public static Version Version
		{
			get
			{
				LastPlayedVersion.InitializeIfNeeded();
				return LastPlayedVersion.lastPlayedVersionInt;
			}
		}

		// Token: 0x0600A42D RID: 42029 RVA: 0x002FCC0C File Offset: 0x002FAE0C
		public static void InitializeIfNeeded()
		{
			if (LastPlayedVersion.initialized)
			{
				return;
			}
			try
			{
				string text = null;
				if (File.Exists(GenFilePaths.LastPlayedVersionFilePath))
				{
					try
					{
						text = File.ReadAllText(GenFilePaths.LastPlayedVersionFilePath);
					}
					catch (Exception ex)
					{
						Log.Error("Exception getting last played version data. Path: " + GenFilePaths.LastPlayedVersionFilePath + ". Exception: " + ex.ToString(), false);
					}
				}
				if (text != null)
				{
					try
					{
						LastPlayedVersion.lastPlayedVersionInt = VersionControl.VersionFromString(text);
					}
					catch (Exception ex2)
					{
						Log.Error("Exception parsing last version from string '" + text + "': " + ex2.ToString(), false);
					}
				}
				if (LastPlayedVersion.lastPlayedVersionInt != VersionControl.CurrentVersion)
				{
					File.WriteAllText(GenFilePaths.LastPlayedVersionFilePath, VersionControl.CurrentVersionString);
				}
			}
			finally
			{
				LastPlayedVersion.initialized = true;
			}
		}

		// Token: 0x04006F54 RID: 28500
		private static bool initialized;

		// Token: 0x04006F55 RID: 28501
		private static Version lastPlayedVersionInt;
	}
}
