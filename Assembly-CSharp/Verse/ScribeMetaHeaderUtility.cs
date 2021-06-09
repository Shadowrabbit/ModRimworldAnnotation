using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020004BE RID: 1214
	public class ScribeMetaHeaderUtility
	{
		// Token: 0x06001E1E RID: 7710 RVA: 0x000FAE04 File Offset: 0x000F9004
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
					List<string> list2 = (from mod in LoadedModManager.RunningMods
					select mod.Name).ToList<string>();
					Scribe_Collections.Look<string>(ref list2, "modNames", LookMode.Undefined, Array.Empty<object>());
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x000FAED0 File Offset: 0x000F90D0
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
				}), false);
			}
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0001AC9D File Offset: 0x00018E9D
		private static bool VersionsMatch()
		{
			return VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == VersionControl.BuildFromVersionString(VersionControl.CurrentVersionStringWithRev);
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x000FAFC0 File Offset: 0x000F91C0
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
							Messages.Message(string.Format("{0}: {1}", "MissingMods".Translate(), enumerable.ToCommaList(false)), MessageTypeDefOf.RejectInput, false);
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

		// Token: 0x06001E22 RID: 7714 RVA: 0x000FB180 File Offset: 0x000F9380
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
				loadedModsSummary = ScribeMetaHeaderUtility.loadedModNamesList.ToCommaList(false);
			}
			runningModsSummary = (from id in list
			select ModLister.GetModWithIdentifier(id, false).Name).ToCommaList(false);
			return false;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000FB264 File Offset: 0x000F9464
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

		// Token: 0x06001E24 RID: 7716 RVA: 0x000FB2B4 File Offset: 0x000F94B4
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
				Log.Error("Exception getting game version of " + file.Name + ": " + ex.ToString(), false);
			}
			return null;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x0001ACB5 File Offset: 0x00018EB5
		public static bool ReadToMetaElement(XmlTextReader textReader)
		{
			return ScribeMetaHeaderUtility.ReadToNextElement(textReader) && ScribeMetaHeaderUtility.ReadToNextElement(textReader) && !(textReader.Name != "meta");
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x0001ACE0 File Offset: 0x00018EE0
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

		// Token: 0x0400156F RID: 5487
		private static ScribeMetaHeaderUtility.ScribeHeaderMode lastMode;

		// Token: 0x04001570 RID: 5488
		public static string loadedGameVersion;

		// Token: 0x04001571 RID: 5489
		public static List<string> loadedModIdsList;

		// Token: 0x04001572 RID: 5490
		public static List<string> loadedModNamesList;

		// Token: 0x04001573 RID: 5491
		public const string MetaNodeName = "meta";

		// Token: 0x04001574 RID: 5492
		public const string GameVersionNodeName = "gameVersion";

		// Token: 0x04001575 RID: 5493
		public const string ModIdsNodeName = "modIds";

		// Token: 0x04001576 RID: 5494
		public const string ModNamesNodeName = "modNames";

		// Token: 0x020004BF RID: 1215
		public enum ScribeHeaderMode
		{
			// Token: 0x04001578 RID: 5496
			None,
			// Token: 0x04001579 RID: 5497
			Map,
			// Token: 0x0400157A RID: 5498
			World,
			// Token: 0x0400157B RID: 5499
			Scenario
		}
	}
}
