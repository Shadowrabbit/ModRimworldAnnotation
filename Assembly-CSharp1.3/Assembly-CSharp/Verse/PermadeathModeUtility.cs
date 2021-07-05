using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004D9 RID: 1241
	public static class PermadeathModeUtility
	{
		// Token: 0x0600257C RID: 9596 RVA: 0x000E9CC6 File Offset: 0x000E7EC6
		public static string GeneratePermadeathSaveName()
		{
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(GenFile.SanitizedFileName(NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null)), null);
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000E9CEB File Offset: 0x000E7EEB
		public static string GeneratePermadeathSaveNameBasedOnPlayerInput(string factionName, string acceptedNameEvenIfTaken = null)
		{
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(GenFile.SanitizedFileName(factionName), acceptedNameEvenIfTaken);
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x000E9CFC File Offset: 0x000E7EFC
		public static void CheckUpdatePermadeathModeUniqueNameOnGameLoad(string filename)
		{
			if (Current.Game.Info.permadeathMode && Current.Game.Info.permadeathModeUniqueName != filename)
			{
				Log.Warning("Savefile's name has changed and doesn't match permadeath mode's unique name. Fixing...");
				Current.Game.Info.permadeathModeUniqueName = filename;
			}
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000E9D4C File Offset: 0x000E7F4C
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

		// Token: 0x06002580 RID: 9600 RVA: 0x000E9D8B File Offset: 0x000E7F8B
		private static string AppendedPermadeathModeSuffix(string str)
		{
			return str + " " + "PermadeathModeSaveSuffix".Translate();
		}
	}
}
