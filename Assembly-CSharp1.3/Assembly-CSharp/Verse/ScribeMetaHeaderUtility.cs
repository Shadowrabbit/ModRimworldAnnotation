using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200032F RID: 815
	public class ScribeMetaHeaderUtility
	{
		// Token: 0x0600172D RID: 5933 RVA: 0x000893EC File Offset: 0x000875EC
		public static void WriteMetaHeader()
		{
			if (Scribe.EnterNode("meta"))
			{
				try
				{
					string currentVersionStringWithRev = VersionControl.CurrentVersionStringWithRev;
					Scribe_Values.Look<string>(ref currentVersionStringWithRev, "gameVersion", null, false);
					List<string> list = (from mod in LoadedModManager.RunningMods
					select mod.PackageId).ToList<string>();
					Scribe_Collections.Look<string>(ref list, "modIds", LookMode.Undefined, Array.Empty<object>());
					List<int> list2 = (from mod in LoadedModManager.RunningMods
					select mod.SteamAppId).ToList<int>();
					Scribe_Collections.Look<int>(ref list2, "modSteamIds", LookMode.Undefined, Array.Empty<object>());
					List<string> list3 = (from mod in LoadedModManager.RunningMods
					select mod.Name).ToList<string>();
					Scribe_Collections.Look<string>(ref list3, "modNames", LookMode.Undefined, Array.Empty<object>());
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x000894F8 File Offset: 0x000876F8
		public static void LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode mode, bool logVersionConflictWarning)
		{
			ScribeMetaHeaderUtility.loadedGameVersion = "Unknown";
			ScribeMetaHeaderUtility.loadedModIdsList = null;
			ScribeMetaHeaderUtility.loadedModNamesList = null;
			ScribeMetaHeaderUtility.lastMode = mode;
			if (Scribe.mode != LoadSaveMode.Inactive && Scribe.EnterNode("meta"))
			{
				try
				{
					Scribe_Values.Look<string>(ref ScribeMetaHeaderUtility.loadedGameVersion, "gameVersion", null, false);
					Scribe_Collections.Look<string>(ref ScribeMetaHeaderUtility.loadedModIdsList, "modIds", LookMode.Undefined, Array.Empty<object>());
					Scribe_Collections.Look<string>(ref ScribeMetaHeaderUtility.loadedModNamesList, "modNames", LookMode.Undefined, Array.Empty<object>());
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (logVersionConflictWarning && (mode == ScribeMetaHeaderUtility.ScribeHeaderMode.Map || !UnityData.isEditor) && !ScribeMetaHeaderUtility.VersionsMatch())
			{
				Log.Warning(string.Concat(new object[]
				{
					"Loaded file (",
					mode,
					") is from version ",
					ScribeMetaHeaderUtility.loadedGameVersion,
					", we are running version ",
					VersionControl.CurrentVersionStringWithRev,
					"."
				}));
			}
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x000895E8 File Offset: 0x000877E8
		private static bool VersionsMatch()
		{
			return VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == VersionControl.BuildFromVersionString(VersionControl.CurrentVersionStringWithRev);
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x00089600 File Offset: 0x00087800
		public static bool TryCreateDialogsForVersionMismatchWarnings(Action confirmedAction)
		{
			string text = null;
			string text2 = null;
			if (!BackCompatibility.IsSaveCompatibleWith(ScribeMetaHeaderUtility.loadedGameVersion) && !ScribeMetaHeaderUtility.VersionsMatch())
			{
				text2 = "VersionMismatch".Translate();
				string value = ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty() ? ("(" + "UnknownLower".TranslateSimple() + ")") : ScribeMetaHeaderUtility.loadedGameVersion;
				if (ScribeMetaHeaderUtility.lastMode == ScribeMetaHeaderUtility.ScribeHeaderMode.Map)
				{
					text = "SaveGameIncompatibleWarningText".Translate(value, VersionControl.CurrentVersionString);
				}
				else if (ScribeMetaHeaderUtility.lastMode == ScribeMetaHeaderUtility.ScribeHeaderMode.World)
				{
					text = "WorldFileVersionMismatch".Translate(value, VersionControl.CurrentVersionString);
				}
				else
				{
					text = "FileIncompatibleWarning".Translate(value, VersionControl.CurrentVersionString);
				}
			}
			bool flag = false;
			string value2;
			string value3;
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out value2, out value3))
			{
				flag = true;
				string text3 = "ModsMismatchWarningText".Translate(value2, value3);
				if (text == null)
				{
					text = text3;
				}
				else
				{
					text = text + "\n\n" + text3;
				}
				if (text2 == null)
				{
					text2 = "ModsMismatchWarningTitle".Translate();
				}
			}
			if (text != null)
			{
				Dialog_MessageBox dialog = Dialog_MessageBox.CreateConfirmation(text, confirmedAction, false, text2);
				dialog.buttonAText = "LoadAnyway".Translate();
				if (flag)
				{
					dialog.buttonCText = "ChangeLoadedMods".Translate();
					dialog.buttonCAction = delegate()
					{
						if (Current.ProgramState == ProgramState.Entry)
						{
							ModsConfig.SetActiveToList(ScribeMetaHeaderUtility.loadedModIdsList);
						}
						ModsConfig.SaveFromList(ScribeMetaHeaderUtility.loadedModIdsList);
						IEnumerable<string> enumerable = from id in Enumerable.Range(0, ScribeMetaHeaderUtility.loadedModIdsList.Count)
						where ModLister.GetModWithIdentifier(ScribeMetaHeaderUtility.loadedModIdsList[id], false) == null
						select ScribeMetaHeaderUtility.loadedModNamesList[id];
						if (enumerable.Any<string>())
						{
							Messages.Message(string.Format("{0}: {1}", "MissingMods".Translate(), enumerable.ToCommaList(false, false)), MessageTypeDefOf.RejectInput, false);
							dialog.buttonCClose = false;
						}
						ModsConfig.RestartFromChangedMods();
					};
				}
				Find.WindowStack.Add(dialog);
				return true;
			}
			return false;
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x000897C0 File Offset: 0x000879C0
		public static bool LoadedModsMatchesActiveMods(out string loadedModsSummary, out string runningModsSummary)
		{
			loadedModsSummary = null;
			runningModsSummary = null;
			List<string> list = (from mod in LoadedModManager.RunningMods
			select mod.PackageId).ToList<string>();
			List<string> b = (from mod in LoadedModManager.RunningMods
			select mod.FolderName).ToList<string>();
			if (ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, list) || ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, b))
			{
				return true;
			}
			if (ScribeMetaHeaderUtility.loadedModNamesList == null)
			{
				loadedModsSummary = "None".Translate();
			}
			else
			{
				loadedModsSummary = ScribeMetaHeaderUtility.loadedModNamesList.ToCommaList(false, false);
			}
			runningModsSummary = (from id in list
			select ModLister.GetModWithIdentifier(id, false).Name).ToCommaList(false, false);
			return false;
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x000898A4 File Offset: 0x00087AA4
		private static bool ModListsMatch(List<string> a, List<string> b)
		{
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x000898F4 File Offset: 0x00087AF4
		public static string GameVersionOf(FileInfo file)
		{
			if (!file.Exists)
			{
				throw new ArgumentException();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(file.FullName))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader) && xmlTextReader.ReadToDescendant("gameVersion"))
						{
							return VersionControl.VersionStringWithoutRev(xmlTextReader.ReadString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception getting game version of " + file.Name + ": " + ex.ToString());
			}
			return null;
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x000899B0 File Offset: 0x00087BB0
		public static bool ReadToMetaElement(XmlTextReader textReader)
		{
			return ScribeMetaHeaderUtility.ReadToNextElement(textReader) && ScribeMetaHeaderUtility.ReadToNextElement(textReader) && !(textReader.Name != "meta");
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x000899DB File Offset: 0x00087BDB
		private static bool ReadToNextElement(XmlTextReader textReader)
		{
			while (textReader.Read())
			{
				if (textReader.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400101B RID: 4123
		private static ScribeMetaHeaderUtility.ScribeHeaderMode lastMode;

		// Token: 0x0400101C RID: 4124
		public static string loadedGameVersion;

		// Token: 0x0400101D RID: 4125
		public static List<string> loadedModIdsList;

		// Token: 0x0400101E RID: 4126
		public static List<string> loadedModNamesList;

		// Token: 0x0400101F RID: 4127
		public static List<int> loadedModSteamIdsList;

		// Token: 0x04001020 RID: 4128
		public const string MetaNodeName = "meta";

		// Token: 0x04001021 RID: 4129
		public const string GameVersionNodeName = "gameVersion";

		// Token: 0x04001022 RID: 4130
		public const string ModIdsNodeName = "modIds";

		// Token: 0x04001023 RID: 4131
		public const string ModNamesNodeName = "modNames";

		// Token: 0x04001024 RID: 4132
		public const string ModSteamIdsNodeName = "modSteamIds";

		// Token: 0x02001A5B RID: 6747
		public enum ScribeHeaderMode
		{
			// Token: 0x040064D6 RID: 25814
			None,
			// Token: 0x040064D7 RID: 25815
			Map,
			// Token: 0x040064D8 RID: 25816
			World,
			// Token: 0x040064D9 RID: 25817
			Scenario
		}
	}
}
