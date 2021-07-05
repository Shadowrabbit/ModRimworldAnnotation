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
	// Token: 0x02000152 RID: 338
	public static class LanguageDatabase
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x00030E60 File Offset: 0x0002F060
		public static IEnumerable<LoadedLanguage> AllLoadedLanguages
		{
			get
			{
				return LanguageDatabase.languages;
			}
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00030E67 File Offset: 0x0002F067
		public static void SelectLanguage(LoadedLanguage lang)
		{
			Prefs.LangFolderName = lang.folderName;
			LongEventHandler.QueueLongEvent(delegate()
			{
				PlayDataLoader.ClearAllPlayData();
				PlayDataLoader.LoadAllPlayData(false);
			}, "LoadingLongEvent", true, null, true);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00030EA0 File Offset: 0x0002F0A0
		public static void Clear()
		{
			LanguageDatabase.languages.Clear();
			LanguageDatabase.activeLanguage = null;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00030EB4 File Offset: 0x0002F0B4
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
								Log.Error("Failed to get a relative path for a file: " + virtualDirectory.FullPath + ", located in " + text);
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
				Log.Error("No default language found!");
				LanguageDatabase.defaultLanguage = LanguageDatabase.languages[0];
				LanguageDatabase.activeLanguage = LanguageDatabase.languages[0];
			}
			LanguageDatabase.activeLanguage.LoadMetadata();
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00031128 File Offset: 0x0002F328
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

		// Token: 0x0600096C RID: 2412 RVA: 0x00031188 File Offset: 0x0002F388
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

		// Token: 0x04000862 RID: 2146
		private static List<LoadedLanguage> languages = new List<LoadedLanguage>();

		// Token: 0x04000863 RID: 2147
		public static LoadedLanguage activeLanguage;

		// Token: 0x04000864 RID: 2148
		public static LoadedLanguage defaultLanguage;

		// Token: 0x04000865 RID: 2149
		public static readonly string DefaultLangFolderName = "English";

		// Token: 0x04000866 RID: 2150
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
