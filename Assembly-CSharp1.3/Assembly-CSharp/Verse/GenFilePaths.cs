using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B6 RID: 1206
	public static class GenFilePaths
	{
		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060024C9 RID: 9417 RVA: 0x000E5158 File Offset: 0x000E3358
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
						Log.Message("Save data folder overridden to " + GenFilePaths.saveDataPath);
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

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060024CA RID: 9418 RVA: 0x000E5270 File Offset: 0x000E3470
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

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x000E52C1 File Offset: 0x000E34C1
		private static DirectoryInfo ExecutableDir
		{
			get
			{
				return new DirectoryInfo(UnityData.dataPath).Parent;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060024CC RID: 9420 RVA: 0x000E52D2 File Offset: 0x000E34D2
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

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060024CD RID: 9421 RVA: 0x000E52EF File Offset: 0x000E34EF
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

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060024CE RID: 9422 RVA: 0x000E530C File Offset: 0x000E350C
		public static string ConfigFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Config");
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060024CF RID: 9423 RVA: 0x000E5318 File Offset: 0x000E3518
		private static string SavedGamesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Saves");
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060024D0 RID: 9424 RVA: 0x000E5324 File Offset: 0x000E3524
		private static string ScenariosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Scenarios");
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060024D1 RID: 9425 RVA: 0x000E5330 File Offset: 0x000E3530
		private static string ExternalHistoryFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("External");
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060024D2 RID: 9426 RVA: 0x000E533C File Offset: 0x000E353C
		public static string ScreenshotFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Screenshots");
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060024D3 RID: 9427 RVA: 0x000E5348 File Offset: 0x000E3548
		public static string DevOutputFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("DevOutput");
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x000E5354 File Offset: 0x000E3554
		public static string ModsConfigFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "ModsConfig.xml");
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060024D5 RID: 9429 RVA: 0x000E5365 File Offset: 0x000E3565
		public static string ConceptKnowledgeFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Knowledge.xml");
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060024D6 RID: 9430 RVA: 0x000E5376 File Offset: 0x000E3576
		public static string PrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Prefs.xml");
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060024D7 RID: 9431 RVA: 0x000E5387 File Offset: 0x000E3587
		public static string KeyPrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "KeyPrefs.xml");
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060024D8 RID: 9432 RVA: 0x000E5398 File Offset: 0x000E3598
		public static string LastPlayedVersionFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "LastPlayedVersion.txt");
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060024D9 RID: 9433 RVA: 0x000E53A9 File Offset: 0x000E35A9
		public static string DevModePermanentlyDisabledFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "DevModeDisabled");
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060024DA RID: 9434 RVA: 0x000E53BA File Offset: 0x000E35BA
		public static string BackstoryOutputFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.DevOutputFolderPath, "Fresh_Backstories.xml");
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x000E53CB File Offset: 0x000E35CB
		public static string TempFolderPath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060024DC RID: 9436 RVA: 0x000E53D4 File Offset: 0x000E35D4
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

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060024DD RID: 9437 RVA: 0x000E5448 File Offset: 0x000E3648
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

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x000E54BC File Offset: 0x000E36BC
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

		// Token: 0x060024DF RID: 9439 RVA: 0x000E5530 File Offset: 0x000E3730
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

		// Token: 0x060024E0 RID: 9440 RVA: 0x000E555D File Offset: 0x000E375D
		public static string FilePathForSavedGame(string gameName)
		{
			return Path.Combine(GenFilePaths.SavedGamesFolderPath, gameName + ".rws");
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000E5574 File Offset: 0x000E3774
		public static string AbsPathForScenario(string scenarioName)
		{
			return Path.Combine(GenFilePaths.ScenariosFolderPath, scenarioName + ".rsc");
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000E558C File Offset: 0x000E378C
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

		// Token: 0x060024E3 RID: 9443 RVA: 0x000E5604 File Offset: 0x000E3804
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

		// Token: 0x060024E4 RID: 9444 RVA: 0x000E5654 File Offset: 0x000E3854
		public static string SafeURIForUnityWWWFromPath(string rawPath)
		{
			string text = rawPath;
			for (int i = 0; i < GenFilePaths.FilePathRaw.Length; i++)
			{
				text = text.Replace(GenFilePaths.FilePathRaw[i], GenFilePaths.FilePathSafe[i]);
			}
			return "file:///" + text;
		}

		// Token: 0x040016FE RID: 5886
		private static string saveDataPath = null;

		// Token: 0x040016FF RID: 5887
		private static string modsFolderPath = null;

		// Token: 0x04001700 RID: 5888
		private static string officialModsFolderPath = null;

		// Token: 0x04001701 RID: 5889
		public const string SoundsFolder = "Sounds/";

		// Token: 0x04001702 RID: 5890
		public const string SoundsFolderName = "Sounds";

		// Token: 0x04001703 RID: 5891
		public const string TexturesFolder = "Textures/";

		// Token: 0x04001704 RID: 5892
		public const string TexturesFolderName = "Textures";

		// Token: 0x04001705 RID: 5893
		public const string StringsFolder = "Strings/";

		// Token: 0x04001706 RID: 5894
		public const string DefsFolder = "Defs/";

		// Token: 0x04001707 RID: 5895
		public const string PatchesFolder = "Patches/";

		// Token: 0x04001708 RID: 5896
		public const string AssetBundlesFolderName = "AssetBundles";

		// Token: 0x04001709 RID: 5897
		public const string AssetsFolderName = "Assets";

		// Token: 0x0400170A RID: 5898
		public const string ResourcesFolderName = "Resources";

		// Token: 0x0400170B RID: 5899
		public const string ModsFolderName = "Mods";

		// Token: 0x0400170C RID: 5900
		public const string AssembliesFolder = "Assemblies/";

		// Token: 0x0400170D RID: 5901
		public const string OfficialModsFolderName = "Data";

		// Token: 0x0400170E RID: 5902
		public const string CoreFolderName = "Core";

		// Token: 0x0400170F RID: 5903
		public const string BackstoriesPath = "Backstories";

		// Token: 0x04001710 RID: 5904
		public const string SavedGameExtension = ".rws";

		// Token: 0x04001711 RID: 5905
		public const string ScenarioExtension = ".rsc";

		// Token: 0x04001712 RID: 5906
		public const string ExternalHistoryFileExtension = ".rwh";

		// Token: 0x04001713 RID: 5907
		private const string SaveDataFolderCommand = "savedatafolder";

		// Token: 0x04001714 RID: 5908
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

		// Token: 0x04001715 RID: 5909
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
