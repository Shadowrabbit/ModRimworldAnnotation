using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200150C RID: 5388
	public static class VersionUpdateDialogMaker
	{
		// Token: 0x0600805E RID: 32862 RVA: 0x002D7BA8 File Offset: 0x002D5DA8
		public static void CreateVersionUpdateDialogIfNecessary()
		{
			if (!VersionUpdateDialogMaker.dialogDone && LastPlayedVersion.Version != null && (VersionControl.CurrentMajor != LastPlayedVersion.Version.Major || VersionControl.CurrentMinor != LastPlayedVersion.Version.Minor))
			{
				VersionUpdateDialogMaker.CreateNewVersionDialog();
			}
		}

		// Token: 0x0600805F RID: 32863 RVA: 0x002D7BE8 File Offset: 0x002D5DE8
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

		// Token: 0x04004FF9 RID: 20473
		private static bool dialogDone;
	}
}
