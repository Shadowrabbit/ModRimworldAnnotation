using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorld.IO;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000203 RID: 515
	public static class LanguageDatabase
	{
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x000101A4 File Offset: 0x0000E3A4
		public static IEnumerable<LoadedLanguage> AllLoadedLanguages
		{
			get
			{
				return LanguageDatabase.languages;
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x000101AB File Offset: 0x0000E3AB
		public static void SelectLanguage(LoadedLanguage lang)
		{
			Prefs.LangFolderName = lang.folderName;
			LongEventHandler.QueueLongEvent(delegate()
			{
				PlayDataLoader.ClearAllPlayData();
				PlayDataLoader.LoadAllPlayData(false);
			}, "LoadingLongEvent", true, null, true);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x000101E4 File Offset: 0x0000E3E4
		public static void Clear()
		{
			LanguageDatabase.languages.Clear();
			LanguageDatabase.activeLanguage = null;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000AC424 File Offset: 0x000AA624
		public static void InitAllMetadata()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
			{
				HashSet<string> hashSet = new HashSet<string>();
				foreach (string path in modContentPack.foldersToLoadDescendingOrder)
				{
					string text = Path.Combine(path, "Languages");
					if (new DirectoryInfo(text).Exists)
					{
						foreach (VirtualDirectory virtualDirectory in AbstractFilesystem.GetDirectories(text, "*", SearchOption.TopDirectoryOnly, false))
						{
							if (!virtualDirectory.FullPath.StartsWith(text))
							{
								Log.Error("Failed to get a relative path for a file: " + virtualDirectory.FullPath + ", located in " + text, false);
							}
							else
							{
								string item = virtualDirectory.FullPath.Substring(text.Length);
								if (!hashSet.Contains(item))
								{
									LanguageDatabase.InitLanguageMetadataFrom(virtualDirectory);
									hashSet.Add(item);
								}
							}
						}
					}
				}
			}
			LanguageDatabase.languages.SortBy((LoadedLanguage l) => l.folderName);
			LanguageDatabase.defaultLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == LanguageDatabase.DefaultLangFolderName);
			LanguageDatabase.activeLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == Prefs.LangFolderName);
			if (LanguageDatabase.activeLanguage == null)
			{
				Prefs.LangFolderName = LanguageDatabase.DefaultLangFolderName;
				LanguageDatabase.activeLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == Prefs.LangFolderName);
			}
			if (LanguageDatabase.activeLanguage == null || LanguageDatabase.defaultLanguage == null)
			{
				Log.Error("No default language found!", false);
				LanguageDatabase.defaultLanguage = LanguageDatabase.languages[0];
				LanguageDatabase.activeLanguage = LanguageDatabase.languages[0];
			}
			LanguageDatabase.activeLanguage.LoadMetadata();
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x000AC69C File Offset: 0x000AA89C
		private static LoadedLanguage InitLanguageMetadataFrom(VirtualDirectory langDir)
		{
			LoadedLanguage loadedLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage lib) => lib.folderName == langDir.Name || lib.LegacyFolderName == langDir.Name);
			if (loadedLanguage == null)
			{
				loadedLanguage = new LoadedLanguage(langDir.Name);
				LanguageDatabase.languages.Add(loadedLanguage);
			}
			if (loadedLanguage != null)
			{
				loadedLanguage.InitMetadata(langDir);
			}
			return loadedLanguage;
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x000AC6FC File Offset: 0x000AA8FC
		public static string SystemLanguageFolderName()
		{
			if (SteamManager.Initialized)
			{
				string text = SteamApps.GetCurrentGameLanguage().CapitalizeFirst();
				if (LanguageDatabase.SupportedAutoSelectLanguages.Contains(text))
				{
					return text;
				}
			}
			string text2 = Application.systemLanguage.ToString();
			if (LanguageDatabase.SupportedAutoSelectLanguages.Contains(text2))
			{
				return text2;
			}
			return LanguageDatabase.DefaultLangFolderName;
		}

		// Token: 0x04000B53 RID: 2899
		private static List<LoadedLanguage> languages = new List<LoadedLanguage>();

		// Token: 0x04000B54 RID: 2900
		public static LoadedLanguage activeLanguage;

		// Token: 0x04000B55 RID: 2901
		public static LoadedLanguage defaultLanguage;

		// Token: 0x04000B56 RID: 2902
		public static readonly string DefaultLangFolderName = "English";

		// Token: 0x04000B57 RID: 2903
		private static readonly List<string> SupportedAutoSelectLanguages = new List<string>
		{
			"Arabic",
			"ChineseSimplified",
			"ChineseTraditional",
			"Czech",
			"Danish",
			"Dutch",
			"English",
			"Estonian",
			"Finnish",
			"French",
			"German",
			"Hungarian",
			"Italian",
			"Japanese",
			"Korean",
			"Norwegian",
			"Polish",
			"Portuguese",
			"PortugueseBrazilian",
			"Romanian",
			"Russian",
			"Slovak",
			"Spanish",
			"SpanishLatin",
			"Swedish",
			"Turkish",
			"Ukrainian"
		};
	}
}
