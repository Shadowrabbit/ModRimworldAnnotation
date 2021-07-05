using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x0200150B RID: 5387
	public static class LastPlayedVersion
	{
		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x0600805B RID: 32859 RVA: 0x002D7AC6 File Offset: 0x002D5CC6
		public static Version Version
		{
			get
			{
				LastPlayedVersion.InitializeIfNeeded();
				return LastPlayedVersion.lastPlayedVersionInt;
			}
		}

		// Token: 0x0600805C RID: 32860 RVA: 0x002D7AD4 File Offset: 0x002D5CD4
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
						Log.Error("Exception getting last played version data. Path: " + GenFilePaths.LastPlayedVersionFilePath + ". Exception: " + ex.ToString());
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
						Log.Error("Exception parsing last version from string '" + text + "': " + ex2.ToString());
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

		// Token: 0x04004FF7 RID: 20471
		private static bool initialized;

		// Token: 0x04004FF8 RID: 20472
		private static Version lastPlayedVersionInt;
	}
}
