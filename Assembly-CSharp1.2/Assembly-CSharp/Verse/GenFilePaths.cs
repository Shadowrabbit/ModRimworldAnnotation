using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000848 RID: 2120
	public static class GenFilePaths
	{
		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x0600350A RID: 13578 RVA: 0x001567DC File Offset: 0x001549DC
		public static string SaveDataFolderPath
		{
			get
			{
				if (GenFilePaths.saveDataPath == null)
				{
					string text;
					if (GenCommandLine.TryGetCommandLineArg("savedatafolder", out text))
					{
						text.TrimEnd(new char[]
						{
							'\\',
							'/'
						});
						if (text == "")
						{
							text = (Path.DirectorySeparatorChar.ToString() ?? "");
						}
						GenFilePaths.saveDataPath = text;
						Log.Message("Save data folder overridden to " + GenFilePaths.saveDataPath, false);
					}
					else
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
						if (UnityData.isEditor)
						{
							GenFilePaths.saveDataPath = Path.Combine(directoryInfo.Parent.ToString(), "SaveData");
						}
						else if (UnityData.platform == RuntimePlatform.OSXPlayer || UnityData.platform == RuntimePlatform.OSXEditor)
						{
							string path = Path.Combine(Directory.GetParent(UnityData.persistentDataPath).ToString(), "RimWorld");
							if (!Directory.Exists(path))
							{
								Directory.CreateDirectory(path);
							}
							GenFilePaths.saveDataPath = path;
						}
						else
						{
							GenFilePaths.saveDataPath = Application.persistentDataPath;
						}
					}
					DirectoryInfo directoryInfo2 = new DirectoryInfo(GenFilePaths.saveDataPath);
					if (!directoryInfo2.Exists)
					{
						directoryInfo2.Create();
					}
				}
				return GenFilePaths.saveDataPath;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x0600350B RID: 13579 RVA: 0x001568F4 File Offset: 0x00154AF4
		public static string ScenarioPreviewImagePath
		{
			get
			{
				if (!UnityData.isEditor)
				{
					return Path.Combine(GenFilePaths.ExecutableDir.FullName, "ScenarioPreview.jpg");
				}
				return Path.Combine(Path.Combine(Path.Combine(GenFilePaths.ExecutableDir.FullName, "PlatformSpecific"), "All"), "ScenarioPreview.jpg");
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x0600350C RID: 13580 RVA: 0x00029543 File Offset: 0x00027743
		private static DirectoryInfo ExecutableDir
		{
			get
			{
				return new DirectoryInfo(UnityData.dataPath).Parent;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x0600350D RID: 13581 RVA: 0x00029554 File Offset: 0x00027754
		public static string ModsFolderPath
		{
			get
			{
				if (GenFilePaths.modsFolderPath == null)
				{
					GenFilePaths.modsFolderPath = GenFilePaths.GetOrCreateModsFolder("Mods");
				}
				return GenFilePaths.modsFolderPath;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x0600350E RID: 13582 RVA: 0x00029571 File Offset: 0x00027771
		public static string OfficialModsFolderPath
		{
			get
			{
				if (GenFilePaths.officialModsFolderPath == null)
				{
					GenFilePaths.officialModsFolderPath = GenFilePaths.GetOrCreateModsFolder("Data");
				}
				return GenFilePaths.officialModsFolderPath;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x0600350F RID: 13583 RVA: 0x0002958E File Offset: 0x0002778E
		public static string ConfigFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Config");
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06003510 RID: 13584 RVA: 0x0002959A File Offset: 0x0002779A
		private static string SavedGamesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Saves");
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06003511 RID: 13585 RVA: 0x000295A6 File Offset: 0x000277A6
		private static string ScenariosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Scenarios");
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06003512 RID: 13586 RVA: 0x000295B2 File Offset: 0x000277B2
		private static string ExternalHistoryFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("External");
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06003513 RID: 13587 RVA: 0x000295BE File Offset: 0x000277BE
		public static string ScreenshotFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Screenshots");
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06003514 RID: 13588 RVA: 0x000295CA File Offset: 0x000277CA
		public static string DevOutputFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("DevOutput");
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06003515 RID: 13589 RVA: 0x000295D6 File Offset: 0x000277D6
		public static string ModsConfigFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "ModsConfig.xml");
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06003516 RID: 13590 RVA: 0x000295E7 File Offset: 0x000277E7
		public static string ConceptKnowledgeFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Knowledge.xml");
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06003517 RID: 13591 RVA: 0x000295F8 File Offset: 0x000277F8
		public static string PrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Prefs.xml");
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06003518 RID: 13592 RVA: 0x00029609 File Offset: 0x00027809
		public static string KeyPrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "KeyPrefs.xml");
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06003519 RID: 13593 RVA: 0x0002961A File Offset: 0x0002781A
		public static string LastPlayedVersionFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "LastPlayedVersion.txt");
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x0600351A RID: 13594 RVA: 0x0002962B File Offset: 0x0002782B
		public static string DevModePermanentlyDisabledFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "DevModeDisabled");
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x0600351B RID: 13595 RVA: 0x0002963C File Offset: 0x0002783C
		public static string BackstoryOutputFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.DevOutputFolderPath, "Fresh_Backstories.xml");
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600351C RID: 13596 RVA: 0x0002964D File Offset: 0x0002784D
		public static string TempFolderPath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x0600351D RID: 13597 RVA: 0x00156948 File Offset: 0x00154B48
		public static IEnumerable<FileInfo> AllSavedGameFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.SavedGamesFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rws"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x0600351E RID: 13598 RVA: 0x001569BC File Offset: 0x00154BBC
		public static IEnumerable<FileInfo> AllCustomScenarioFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ScenariosFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rsc"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x0600351F RID: 13599 RVA: 0x00156A30 File Offset: 0x00154C30
		public static IEnumerable<FileInfo> AllExternalHistoryFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ExternalHistoryFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rwh"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x00156AA4 File Offset: 0x00154CA4
		private static string FolderUnderSaveData(string folderName)
		{
			string text = Path.Combine(GenFilePaths.SaveDataFolderPath, folderName);
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			return text;
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x00029654 File Offset: 0x00027854
		public static string FilePathForSavedGame(string gameName)
		{
			return Path.Combine(GenFilePaths.SavedGamesFolderPath, gameName + ".rws");
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x0002966B File Offset: 0x0002786B
		public static string AbsPathForScenario(string scenarioName)
		{
			return Path.Combine(GenFilePaths.ScenariosFolderPath, scenarioName + ".rsc");
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x00156AD4 File Offset: 0x00154CD4
		public static string ContentPath<T>()
		{
			if (typeof(T) == typeof(AudioClip))
			{
				return "Sounds/";
			}
			if (typeof(T) == typeof(Texture2D))
			{
				return "Textures/";
			}
			if (typeof(T) == typeof(string))
			{
				return "Strings/";
			}
			throw new ArgumentException();
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x00156B4C File Offset: 0x00154D4C
		private static string GetOrCreateModsFolder(string folderName)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
			DirectoryInfo directoryInfo2;
			if (UnityData.isEditor)
			{
				directoryInfo2 = directoryInfo;
			}
			else
			{
				directoryInfo2 = directoryInfo.Parent;
			}
			string text = Path.Combine(directoryInfo2.ToString(), folderName);
			DirectoryInfo directoryInfo3 = new DirectoryInfo(text);
			if (!directoryInfo3.Exists)
			{
				directoryInfo3.Create();
			}
			return text;
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x00156B9C File Offset: 0x00154D9C
		public static string SafeURIForUnityWWWFromPath(string rawPath)
		{
			string text = rawPath;
			for (int i = 0; i < GenFilePaths.FilePathRaw.Length; i++)
			{
				text = text.Replace(GenFilePaths.FilePathRaw[i], GenFilePaths.FilePathSafe[i]);
			}
			return "file:///" + text;
		}

		// Token: 0x040024D7 RID: 9431
		private static string saveDataPath = null;

		// Token: 0x040024D8 RID: 9432
		private static string modsFolderPath = null;

		// Token: 0x040024D9 RID: 9433
		private static string officialModsFolderPath = null;

		// Token: 0x040024DA RID: 9434
		public const string SoundsFolder = "Sounds/";

		// Token: 0x040024DB RID: 9435
		public const string SoundsFolderName = "Sounds";

		// Token: 0x040024DC RID: 9436
		public const string TexturesFolder = "Textures/";

		// Token: 0x040024DD RID: 9437
		public const string TexturesFolderName = "Textures";

		// Token: 0x040024DE RID: 9438
		public const string StringsFolder = "Strings/";

		// Token: 0x040024DF RID: 9439
		public const string DefsFolder = "Defs/";

		// Token: 0x040024E0 RID: 9440
		public const string PatchesFolder = "Patches/";

		// Token: 0x040024E1 RID: 9441
		public const string AssetBundlesFolderName = "AssetBundles";

		// Token: 0x040024E2 RID: 9442
		public const string AssetsFolderName = "Assets";

		// Token: 0x040024E3 RID: 9443
		public const string ResourcesFolderName = "Resources";

		// Token: 0x040024E4 RID: 9444
		public const string ModsFolderName = "Mods";

		// Token: 0x040024E5 RID: 9445
		public const string AssembliesFolder = "Assemblies/";

		// Token: 0x040024E6 RID: 9446
		public const string OfficialModsFolderName = "Data";

		// Token: 0x040024E7 RID: 9447
		public const string CoreFolderName = "Core";

		// Token: 0x040024E8 RID: 9448
		public const string BackstoriesPath = "Backstories";

		// Token: 0x040024E9 RID: 9449
		public const string SavedGameExtension = ".rws";

		// Token: 0x040024EA RID: 9450
		public const string ScenarioExtension = ".rsc";

		// Token: 0x040024EB RID: 9451
		public const string ExternalHistoryFileExtension = ".rwh";

		// Token: 0x040024EC RID: 9452
		private const string SaveDataFolderCommand = "savedatafolder";

		// Token: 0x040024ED RID: 9453
		private static readonly string[] FilePathRaw = new string[]
		{
			"Ž",
			"ž",
			"Ÿ",
			"¡",
			"¢",
			"£",
			"¤",
			"¥",
			"¦",
			"§",
			"¨",
			"©",
			"ª",
			"À",
			"Á",
			"Â",
			"Ã",
			"Ä",
			"Å",
			"Æ",
			"Ç",
			"È",
			"É",
			"Ê",
			"Ë",
			"Ì",
			"Í",
			"Î",
			"Ï",
			"Ð",
			"Ñ",
			"Ò",
			"Ó",
			"Ô",
			"Õ",
			"Ö",
			"Ù",
			"Ú",
			"Û",
			"Ü",
			"Ý",
			"Þ",
			"ß",
			"à",
			"á",
			"â",
			"ã",
			"ä",
			"å",
			"æ",
			"ç",
			"è",
			"é",
			"ê",
			"ë",
			"ì",
			"í",
			"î",
			"ï",
			"ð",
			"ñ",
			"ò",
			"ó",
			"ô",
			"õ",
			"ö",
			"ù",
			"ú",
			"û",
			"ü",
			"ý",
			"þ",
			"ÿ"
		};

		// Token: 0x040024EE RID: 9454
		private static readonly string[] FilePathSafe = new string[]
		{
			"%8E",
			"%9E",
			"%9F",
			"%A1",
			"%A2",
			"%A3",
			"%A4",
			"%A5",
			"%A6",
			"%A7",
			"%A8",
			"%A9",
			"%AA",
			"%C0",
			"%C1",
			"%C2",
			"%C3",
			"%C4",
			"%C5",
			"%C6",
			"%C7",
			"%C8",
			"%C9",
			"%CA",
			"%CB",
			"%CC",
			"%CD",
			"%CE",
			"%CF",
			"%D0",
			"%D1",
			"%D2",
			"%D3",
			"%D4",
			"%D5",
			"%D6",
			"%D9",
			"%DA",
			"%DB",
			"%DC",
			"%DD",
			"%DE",
			"%DF",
			"%E0",
			"%E1",
			"%E2",
			"%E3",
			"%E4",
			"%E5",
			"%E6",
			"%E7",
			"%E8",
			"%E9",
			"%EA",
			"%EB",
			"%EC",
			"%ED",
			"%EE",
			"%EF",
			"%F0",
			"%F1",
			"%F2",
			"%F3",
			"%F4",
			"%F5",
			"%F6",
			"%F9",
			"%FA",
			"%FB",
			"%FC",
			"%FD",
			"%FE",
			"%FF"
		};
	}
}
