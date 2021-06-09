using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000884 RID: 2180
	public static class PermadeathModeUtility
	{
		// Token: 0x0600360E RID: 13838 RVA: 0x00029E63 File Offset: 0x00028063
		public static string GeneratePermadeathSaveName()
		{
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(GenFile.SanitizedFileName(NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null)), null);
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x00029E88 File Offset: 0x00028088
		public static string GeneratePermadeathSaveNameBasedOnPlayerInput(string factionName, string acceptedNameEvenIfTaken = null)
		{
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(GenFile.SanitizedFileName(factionName), acceptedNameEvenIfTaken);
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x0015B784 File Offset: 0x00159984
		public static void CheckUpdatePermadeathModeUniqueNameOnGameLoad(string filename)
		{
			if (Current.Game.Info.permadeathMode && Current.Game.Info.permadeathModeUniqueName != filename)
			{
				Log.Warning("Savefile's name has changed and doesn't match permadeath mode's unique name. Fixing...", false);
				Current.Game.Info.permadeathModeUniqueName = filename;
			}
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x0015B7D4 File Offset: 0x001599D4
		private static string NewPermadeathSaveNameWithAppendedNumberIfNecessary(string name, string acceptedNameEvenIfTaken = null)
		{
			int num = 0;
			string text;
			do
			{
				num++;
				text = name;
				if (num != 1)
				{
					text += num;
				}
				text = PermadeathModeUtility.AppendedPermadeathModeSuffix(text);
			}
			while (SaveGameFilesUtility.SavedGameNamedExists(text) && text != acceptedNameEvenIfTaken);
			return text;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x00029E96 File Offset: 0x00028096
		private static string AppendedPermadeathModeSuffix(string str)
		{
			return str + " " + "PermadeathModeSaveSuffix".Translate();
		}
	}
}
