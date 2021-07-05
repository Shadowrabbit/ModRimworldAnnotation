using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D82 RID: 7554
	public static class VersionUpdateDialogMaker
	{
		// Token: 0x0600A42F RID: 42031 RVA: 0x0006CD97 File Offset: 0x0006AF97
		public static void CreateVersionUpdateDialogIfNecessary()
		{
			if (!VersionUpdateDialogMaker.dialogDone && LastPlayedVersion.Version != null && (VersionControl.CurrentMajor != LastPlayedVersion.Version.Major || VersionControl.CurrentMinor != LastPlayedVersion.Version.Minor))
			{
				VersionUpdateDialogMaker.CreateNewVersionDialog();
			}
		}

		// Token: 0x0600A430 RID: 42032 RVA: 0x002FCCE0 File Offset: 0x002FAEE0
		private static void CreateNewVersionDialog()
		{
			string value = LastPlayedVersion.Version.Major + "." + LastPlayedVersion.Version.Minor;
			string value2 = VersionControl.CurrentMajor + "." + VersionControl.CurrentMinor;
			string text = "GameUpdatedToNewVersionInitial".Translate(value, value2);
			text += "\n\n";
			if (BackCompatibility.IsSaveCompatibleWith(LastPlayedVersion.Version.ToString()))
			{
				text += "GameUpdatedToNewVersionSavesCompatible".Translate();
			}
			else
			{
				text += "GameUpdatedToNewVersionSavesIncompatible".Translate();
			}
			text += "\n\n";
			text += "GameUpdatedToNewVersionSteam".Translate();
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null));
			VersionUpdateDialogMaker.dialogDone = true;
		}

		// Token: 0x04006F56 RID: 28502
		private static bool dialogDone;
	}
}
