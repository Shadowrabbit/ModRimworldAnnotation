using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000168 RID: 360
	public class LoadedLanguage
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x00034CC3 File Offset: 0x00032EC3
		public string DisplayName
		{
			get
			{
				return GenText.SplitCamelCase(this.folderName);
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x00034CD0 File Offset: 0x00032ED0
		public string FriendlyNameNative
		{
			get
			{
				if (this.info == null || this.info.friendlyNameNative.NullOrEmpty())
				{
					return this.folderName;
				}
				return this.info.friendlyNameNative;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x00034CFE File Offset: 0x00032EFE
		public string FriendlyNameEnglish
		{
			get
			{
				if (this.info == null || this.info.friendlyNameEnglish.NullOrEmpty())
				{
					return this.folderName;
				}
				return this.info.friendlyNameEnglish;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x00034D2C File Offset: 0x00032F2C
		public IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> AllDirectories
		{
			get
			{
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					foreach (string text in mod.foldersToLoadDescendingOrder)
					{
						string path = Path.Combine(text, "Languages");
						VirtualDirectory directory = AbstractFilesystem.GetDirectory(Path.Combine(path, this.folderName));
						if (directory.Exists)
						{
							yield return new Tuple<VirtualDirectory, ModContentPack, string>(directory, mod, text);
						}
						else
						{
							directory = AbstractFilesystem.GetDirectory(Path.Combine(path, this.legacyFolderName));
							if (directory.Exists)
							{
								yield return new Tuple<VirtualDirectory, ModContentPack, string>(directory, mod, text);
							}
						}
					}
					List<string>.Enumerator enumerator2 = default(List<string>.Enumerator);
					mod = null;
				}
				IEnumerator<ModContentPack> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x00034D3C File Offset: 0x00032F3C
		public LanguageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (LanguageWorker)Activator.CreateInstance(this.info.languageWorkerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x00034D67 File Offset: 0x00032F67
		public LanguageWordInfo WordInfo
		{
			get
			{
				return this.wordInfo;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x00034D6F File Offset: 0x00032F6F
		public string LegacyFolderName
		{
			get
			{
				return this.legacyFolderName;
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00034D78 File Offset: 0x00032F78
		public LoadedLanguage(string folderName)
		{
			this.folderName = folderName;
			this.legacyFolderName = (folderName.Contains("(") ? folderName.Substring(0, folderName.IndexOf("(") - 1) : folderName).Trim();
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00034E1C File Offset: 0x0003301C
		public void LoadMetadata()
		{
			if (this.info != null && this.infoIsRealMetadata)
			{
				return;
			}
			this.infoIsRealMetadata = true;
			foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
			{
				foreach (string path in modContentPack.foldersToLoadDescendingOrder)
				{
					string text = Path.Combine(path, "Languages");
					if (new DirectoryInfo(text).Exists)
					{
						foreach (VirtualDirectory virtualDirectory in AbstractFilesystem.GetDirectories(text, "*", SearchOption.TopDirectoryOnly, false))
						{
							if (virtualDirectory.Name == this.folderName || virtualDirectory.Name == this.legacyFolderName)
							{
								this.info = DirectXmlLoader.ItemFromXmlFile<LanguageInfo>(virtualDirectory, "LanguageInfo.xml", false);
								if (this.info.friendlyNameNative.NullOrEmpty() && virtualDirectory.FileExists("FriendlyName.txt"))
								{
									this.info.friendlyNameNative = virtualDirectory.ReadAllText("FriendlyName.txt");
								}
								if (this.info.friendlyNameNative.NullOrEmpty())
								{
									this.info.friendlyNameNative = this.folderName;
								}
								if (this.info.friendlyNameEnglish.NullOrEmpty())
								{
									this.info.friendlyNameEnglish = this.folderName;
								}
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00035004 File Offset: 0x00033204
		public void InitMetadata(VirtualDirectory directory)
		{
			this.infoIsRealMetadata = false;
			this.info = new LanguageInfo();
			string text = Regex.Replace(directory.Name, "(\\B[A-Z]+?(?=[A-Z][^A-Z])|\\B[A-Z]+?(?=[^A-Z]))", " $1");
			string friendlyNameEnglish = text;
			string friendlyNameNative = text;
			int num = text.FirstIndexOf((char c) => c == '(');
			int num2 = text.LastIndexOf(")");
			if (num >= 0 && num2 >= 0 && num2 > num)
			{
				friendlyNameEnglish = text.Substring(0, num - 1);
				friendlyNameNative = text.Substring(num + 1, num2 - num - 1);
			}
			this.info.friendlyNameEnglish = friendlyNameEnglish;
			this.info.friendlyNameNative = friendlyNameNative;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x000350B4 File Offset: 0x000332B4
		public void LoadData()
		{
			if (this.dataIsLoaded)
			{
				return;
			}
			this.dataIsLoaded = true;
			DeepProfiler.Start("Loading language data: " + this.folderName);
			try
			{
				this.tmpAlreadyLoadedFiles.Clear();
				foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in this.AllDirectories)
				{
					Tuple<VirtualDirectory, ModContentPack, string> localDirectory = tuple;
					if (!this.tmpAlreadyLoadedFiles.ContainsKey(localDirectory.Item2))
					{
						this.tmpAlreadyLoadedFiles[localDirectory.Item2] = new HashSet<string>();
					}
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						if (this.icon == BaseContent.BadTex)
						{
							VirtualFile file = localDirectory.Item1.GetFile("LangIcon.png");
							if (file.Exists)
							{
								this.icon = ModContentLoader<Texture2D>.LoadItem(file).contentItem;
							}
						}
					});
					VirtualDirectory directory = localDirectory.Item1.GetDirectory("CodeLinked");
					if (directory.Exists)
					{
						this.loadErrors.Add("Translations aren't called CodeLinked any more. Please rename to Keyed: " + directory);
					}
					else
					{
						directory = localDirectory.Item1.GetDirectory("Keyed");
					}
					if (directory.Exists)
					{
						foreach (VirtualFile virtualFile in directory.GetFiles("*.xml", SearchOption.AllDirectories))
						{
							if (this.TryRegisterFileIfNew(localDirectory, virtualFile.FullPath))
							{
								this.LoadFromFile_Keyed(virtualFile);
							}
						}
					}
					VirtualDirectory directory2 = localDirectory.Item1.GetDirectory("DefLinked");
					if (directory2.Exists)
					{
						this.loadErrors.Add("Translations aren't called DefLinked any more. Please rename to DefInjected: " + directory2);
					}
					else
					{
						directory2 = localDirectory.Item1.GetDirectory("DefInjected");
					}
					if (directory2.Exists)
					{
						foreach (VirtualDirectory virtualDirectory in directory2.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							string name = virtualDirectory.Name;
							Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name, null);
							if (typeInAnyAssembly == null && name.Length > 3)
							{
								typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name.Substring(0, name.Length - 1), null);
							}
							if (typeInAnyAssembly == null)
							{
								this.loadErrors.Add(string.Concat(new object[]
								{
									"Error loading language from ",
									tuple,
									": dir ",
									virtualDirectory.Name,
									" doesn't correspond to any def type. Skipping..."
								}));
							}
							else
							{
								foreach (VirtualFile virtualFile2 in virtualDirectory.GetFiles("*.xml", SearchOption.AllDirectories))
								{
									if (this.TryRegisterFileIfNew(localDirectory, virtualFile2.FullPath))
									{
										this.LoadFromFile_DefInject(virtualFile2, typeInAnyAssembly);
									}
								}
							}
						}
					}
					this.EnsureAllDefTypesHaveDefInjectionPackage();
					VirtualDirectory directory3 = localDirectory.Item1.GetDirectory("Strings");
					if (directory3.Exists)
					{
						foreach (VirtualDirectory virtualDirectory2 in directory3.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							foreach (VirtualFile virtualFile3 in virtualDirectory2.GetFiles("*.txt", SearchOption.AllDirectories))
							{
								if (this.TryRegisterFileIfNew(localDirectory, virtualFile3.FullPath))
								{
									this.LoadFromFile_Strings(virtualFile3, directory3);
								}
							}
						}
					}
					this.wordInfo.LoadFrom(localDirectory, this);
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception loading language data. Rethrowing. Exception: " + arg);
				throw;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00035538 File Offset: 0x00033738
		public bool TryRegisterFileIfNew(Tuple<VirtualDirectory, ModContentPack, string> dir, string filePath)
		{
			if (!filePath.StartsWith(dir.Item3))
			{
				Log.Error("Failed to get a relative path for a file: " + filePath + ", located in " + dir.Item3);
				return false;
			}
			string item = filePath.Substring(dir.Item3.Length);
			if (!this.tmpAlreadyLoadedFiles.ContainsKey(dir.Item2))
			{
				this.tmpAlreadyLoadedFiles[dir.Item2] = new HashSet<string>();
			}
			else if (this.tmpAlreadyLoadedFiles[dir.Item2].Contains(item))
			{
				return false;
			}
			this.tmpAlreadyLoadedFiles[dir.Item2].Add(item);
			return true;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x000355E4 File Offset: 0x000337E4
		private void LoadFromFile_Strings(VirtualFile file, VirtualDirectory stringsTopDir)
		{
			string text;
			try
			{
				text = file.ReadAllText();
			}
			catch (Exception ex)
			{
				this.loadErrors.Add(string.Concat(new object[]
				{
					"Exception loading from strings file ",
					file,
					": ",
					ex
				}));
				return;
			}
			string text2 = file.FullPath;
			if (stringsTopDir != null)
			{
				text2 = text2.Substring(stringsTopDir.FullPath.Length + 1);
			}
			text2 = text2.Substring(0, text2.Length - Path.GetExtension(text2).Length);
			text2 = text2.Replace('\\', '/');
			List<string> list = new List<string>();
			foreach (string item in GenText.LinesFromString(text))
			{
				list.Add(item);
			}
			List<string> list2;
			if (this.stringFiles.TryGetValue(text2, out list2))
			{
				using (List<string>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string item2 = enumerator2.Current;
						list2.Add(item2);
					}
					return;
				}
			}
			this.stringFiles.Add(text2, list);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0003572C File Offset: 0x0003392C
		private void LoadFromFile_Keyed(VirtualFile file)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			try
			{
				foreach (DirectXmlLoaderSimple.XmlKeyValuePair xmlKeyValuePair in DirectXmlLoaderSimple.ValuesFromXmlFile(file))
				{
					if (dictionary.ContainsKey(xmlKeyValuePair.key))
					{
						this.loadErrors.Add("Duplicate keyed translation key: " + xmlKeyValuePair.key + " in language " + this.folderName);
					}
					else
					{
						dictionary.Add(xmlKeyValuePair.key, xmlKeyValuePair.value);
						dictionary2.Add(xmlKeyValuePair.key, xmlKeyValuePair.lineNumber);
					}
				}
			}
			catch (Exception ex)
			{
				this.loadErrors.Add(string.Concat(new object[]
				{
					"Exception loading from translation file ",
					file,
					": ",
					ex
				}));
				dictionary.Clear();
				dictionary2.Clear();
				this.anyKeyedReplacementsXmlParseError = true;
				this.lastKeyedReplacementsXmlParseErrorInFile = file.Name;
			}
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				string text = keyValuePair.Value;
				LoadedLanguage.KeyedReplacement keyedReplacement = new LoadedLanguage.KeyedReplacement();
				if (text == "TODO")
				{
					keyedReplacement.isPlaceholder = true;
					text = "";
				}
				keyedReplacement.key = keyValuePair.Key;
				keyedReplacement.value = text;
				keyedReplacement.fileSource = file.Name;
				keyedReplacement.fileSourceLine = dictionary2[keyValuePair.Key];
				keyedReplacement.fileSourceFullPath = file.FullPath;
				this.keyedReplacements.SetOrAdd(keyValuePair.Key, keyedReplacement);
			}
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x000358FC File Offset: 0x00033AFC
		public void LoadFromFile_DefInject(VirtualFile file, Type defType)
		{
			DefInjectionPackage defInjectionPackage = (from di in this.defInjections
			where di.defType == defType
			select di).FirstOrDefault<DefInjectionPackage>();
			if (defInjectionPackage == null)
			{
				defInjectionPackage = new DefInjectionPackage(defType);
				this.defInjections.Add(defInjectionPackage);
			}
			bool flag;
			defInjectionPackage.AddDataFromFile(file, out flag);
			if (flag)
			{
				this.anyDefInjectionsXmlParseError = true;
				this.lastDefInjectionsXmlParseErrorInFile = file.Name;
			}
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00035970 File Offset: 0x00033B70
		private void EnsureAllDefTypesHaveDefInjectionPackage()
		{
			using (IEnumerator<Type> enumerator = GenDefDatabase.AllDefTypesWithDatabases().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type defType = enumerator.Current;
					if (!this.defInjections.Any((DefInjectionPackage x) => x.defType == defType))
					{
						this.defInjections.Add(new DefInjectionPackage(defType));
					}
				}
			}
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x000359F0 File Offset: 0x00033BF0
		public bool HaveTextForKey(string key, bool allowPlaceholders = false)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			return key != null && this.keyedReplacements.TryGetValue(key, out keyedReplacement) && (allowPlaceholders || !keyedReplacement.isPlaceholder);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00035A30 File Offset: 0x00033C30
		public bool TryGetTextFromKey(string key, out TaggedString translated)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			if (key == null)
			{
				translated = key;
				return false;
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement) || keyedReplacement.isPlaceholder)
			{
				translated = key;
				return false;
			}
			translated = keyedReplacement.value;
			return true;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00035A94 File Offset: 0x00033C94
		public bool TryGetStringsFromFile(string fileName, out List<string> stringsList)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			if (!this.stringFiles.TryGetValue(fileName, out stringsList))
			{
				stringsList = null;
				return false;
			}
			return true;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00035ABC File Offset: 0x00033CBC
		public string GetKeySourceFileAndLine(string key)
		{
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement))
			{
				return "unknown";
			}
			return keyedReplacement.fileSource + ":" + keyedReplacement.fileSourceLine;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00035AFA File Offset: 0x00033CFA
		public Gender ResolveGender(string str, string fallback = null)
		{
			return this.wordInfo.ResolveGender(str, fallback);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00035B0C File Offset: 0x00033D0C
		public void InjectIntoData_BeforeImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(false);
				}
				catch (Exception arg)
				{
					Log.Error("Critical error while injecting translations into defs: " + arg);
				}
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00035B90 File Offset: 0x00033D90
		public void InjectIntoData_AfterImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			int num = this.loadErrors.Count;
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(true);
					num += defInjectionPackage.loadErrors.Count;
				}
				catch (Exception arg)
				{
					Log.Error("Critical error while injecting translations into defs: " + arg);
				}
			}
			BackstoryTranslationUtility.LoadAndInjectBackstoryData(this.AllDirectories, this.backstoriesLoadErrors);
			num += this.backstoriesLoadErrors.Count;
			if (num != 0)
			{
				this.anyError = true;
				Log.Warning(string.Concat(new object[]
				{
					"Translation data for language ",
					LanguageDatabase.activeLanguage.FriendlyNameEnglish,
					" has ",
					num,
					" errors. Generate translation report for more info."
				}));
			}
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00035C94 File Offset: 0x00033E94
		public override string ToString()
		{
			return this.info.friendlyNameEnglish;
		}

		// Token: 0x04000883 RID: 2179
		public string folderName;

		// Token: 0x04000884 RID: 2180
		public LanguageInfo info;

		// Token: 0x04000885 RID: 2181
		private LanguageWorker workerInt;

		// Token: 0x04000886 RID: 2182
		private LanguageWordInfo wordInfo = new LanguageWordInfo();

		// Token: 0x04000887 RID: 2183
		private bool dataIsLoaded;

		// Token: 0x04000888 RID: 2184
		public List<string> loadErrors = new List<string>();

		// Token: 0x04000889 RID: 2185
		public List<string> backstoriesLoadErrors = new List<string>();

		// Token: 0x0400088A RID: 2186
		public bool anyKeyedReplacementsXmlParseError;

		// Token: 0x0400088B RID: 2187
		public string lastKeyedReplacementsXmlParseErrorInFile;

		// Token: 0x0400088C RID: 2188
		public bool anyDefInjectionsXmlParseError;

		// Token: 0x0400088D RID: 2189
		public string lastDefInjectionsXmlParseErrorInFile;

		// Token: 0x0400088E RID: 2190
		public bool anyError;

		// Token: 0x0400088F RID: 2191
		private string legacyFolderName;

		// Token: 0x04000890 RID: 2192
		private Dictionary<ModContentPack, HashSet<string>> tmpAlreadyLoadedFiles = new Dictionary<ModContentPack, HashSet<string>>();

		// Token: 0x04000891 RID: 2193
		public Texture2D icon = BaseContent.BadTex;

		// Token: 0x04000892 RID: 2194
		public Dictionary<string, LoadedLanguage.KeyedReplacement> keyedReplacements = new Dictionary<string, LoadedLanguage.KeyedReplacement>();

		// Token: 0x04000893 RID: 2195
		public List<DefInjectionPackage> defInjections = new List<DefInjectionPackage>();

		// Token: 0x04000894 RID: 2196
		public Dictionary<string, List<string>> stringFiles = new Dictionary<string, List<string>>();

		// Token: 0x04000895 RID: 2197
		public const string OldKeyedTranslationsFolderName = "CodeLinked";

		// Token: 0x04000896 RID: 2198
		public const string KeyedTranslationsFolderName = "Keyed";

		// Token: 0x04000897 RID: 2199
		public const string OldDefInjectionsFolderName = "DefLinked";

		// Token: 0x04000898 RID: 2200
		public const string DefInjectionsFolderName = "DefInjected";

		// Token: 0x04000899 RID: 2201
		public const string LanguagesFolderName = "Languages";

		// Token: 0x0400089A RID: 2202
		public const string PlaceholderText = "TODO";

		// Token: 0x0400089B RID: 2203
		private bool infoIsRealMetadata;

		// Token: 0x02001932 RID: 6450
		public class KeyedReplacement
		{
			// Token: 0x040060BE RID: 24766
			public string key;

			// Token: 0x040060BF RID: 24767
			public string value;

			// Token: 0x040060C0 RID: 24768
			public string fileSource;

			// Token: 0x040060C1 RID: 24769
			public int fileSourceLine;

			// Token: 0x040060C2 RID: 24770
			public string fileSourceFullPath;

			// Token: 0x040060C3 RID: 24771
			public bool isPlaceholder;
		}
	}
}
