using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000341 RID: 833
	public class ModContentPack
	{
		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x00015178 File Offset: 0x00013378
		public string RootDir
		{
			get
			{
				return this.rootDirInt.FullName;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x00015185 File Offset: 0x00013385
		public string PackageId
		{
			get
			{
				return this.packageIdInt;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0001518D File Offset: 0x0001338D
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.packageIdPlayerFacingInt;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001528 RID: 5416 RVA: 0x00015195 File Offset: 0x00013395
		public string FolderName
		{
			get
			{
				return this.rootDirInt.Name;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x000151A2 File Offset: 0x000133A2
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x000151AA File Offset: 0x000133AA
		public int OverwritePriority
		{
			get
			{
				if (!this.IsCoreMod)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x000151B7 File Offset: 0x000133B7
		public bool IsCoreMod
		{
			get
			{
				return this.PackageId == ModContentPack.CoreModPackageId;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x000151C9 File Offset: 0x000133C9
		public IEnumerable<Def> AllDefs
		{
			get
			{
				return this.defs;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x000151D1 File Offset: 0x000133D1
		public IEnumerable<PatchOperation> Patches
		{
			get
			{
				if (this.patches == null)
				{
					this.LoadPatches();
				}
				return this.patches;
			}
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x000D1BF0 File Offset: 0x000CFDF0
		public IEnumerable<string> AllAssetNamesInBundle(int index)
		{
			if (this.allAssetNamesInBundleCached == null)
			{
				this.allAssetNamesInBundleCached = new List<List<string>>();
				foreach (AssetBundle assetBundle in this.assetBundles.loadedAssetBundles)
				{
					this.allAssetNamesInBundleCached.Add(new List<string>(assetBundle.GetAllAssetNames()));
				}
			}
			return this.allAssetNamesInBundleCached[index];
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x000151E7 File Offset: 0x000133E7
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public ModContentPack(DirectoryInfo directory, string packageId, int loadOrder, string name) : this(directory, packageId, packageId, loadOrder, name)
		{
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x000D1C78 File Offset: 0x000CFE78
		public ModContentPack(DirectoryInfo directory, string packageId, string packageIdPlayerFacing, int loadOrder, string name)
		{
			this.rootDirInt = directory;
			this.loadOrder = loadOrder;
			this.nameInt = name;
			this.packageIdInt = packageId.ToLower();
			this.packageIdPlayerFacingInt = packageIdPlayerFacing;
			this.audioClips = new ModContentHolder<AudioClip>(this);
			this.textures = new ModContentHolder<Texture2D>(this);
			this.strings = new ModContentHolder<string>(this);
			this.assetBundles = new ModAssetBundlesHandler(this);
			this.assemblies = new ModAssemblyHandler(this);
			this.InitLoadFolders();
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x000151F5 File Offset: 0x000133F5
		public void ClearDestroy()
		{
			this.audioClips.ClearDestroy();
			this.textures.ClearDestroy();
			this.assetBundles.ClearDestroy();
			this.allAssetNamesInBundleCached = null;
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x000D1D04 File Offset: 0x000CFF04
		public ModContentHolder<T> GetContentHolder<T>() where T : class
		{
			if (typeof(T) == typeof(Texture2D))
			{
				return (ModContentHolder<T>)this.textures;
			}
			if (typeof(T) == typeof(AudioClip))
			{
				return (ModContentHolder<T>)this.audioClips;
			}
			if (typeof(T) == typeof(string))
			{
				return (ModContentHolder<T>)this.strings;
			}
			Log.Error("Mod lacks manager for asset type " + this.strings, false);
			return null;
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x000D1DA0 File Offset: 0x000CFFA0
		private void ReloadContentInt()
		{
			DeepProfiler.Start("Reload audio clips");
			try
			{
				this.audioClips.ReloadAll();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload textures");
			try
			{
				this.textures.ReloadAll();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload strings");
			try
			{
				this.strings.ReloadAll();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload asset bundles");
			try
			{
				this.assetBundles.ReloadAll();
				this.allAssetNamesInBundleCached = null;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0001521F File Offset: 0x0001341F
		public void ReloadContent()
		{
			LongEventHandler.ExecuteWhenFinished(new Action(this.ReloadContentInt));
			this.assemblies.ReloadAll();
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0001523D File Offset: 0x0001343D
		public IEnumerable<LoadableXmlAsset> LoadDefs()
		{
			if (this.defs.Count != 0)
			{
				Log.ErrorOnce("LoadDefs called with already existing def packages", 39029405, false);
			}
			DeepProfiler.Start("Load Assets");
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Defs/", null).ToList<LoadableXmlAsset>();
			DeepProfiler.End();
			DeepProfiler.Start("Parse Assets");
			foreach (LoadableXmlAsset loadableXmlAsset in list)
			{
				yield return loadableXmlAsset;
			}
			List<LoadableXmlAsset>.Enumerator enumerator = default(List<LoadableXmlAsset>.Enumerator);
			DeepProfiler.End();
			yield break;
			yield break;
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x000D1E5C File Offset: 0x000D005C
		private void InitLoadFolders()
		{
			this.foldersToLoadDescendingOrder = new List<string>();
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.PackageId, false);
			if (((modWithIdentifier != null) ? modWithIdentifier.loadFolders : null) != null && modWithIdentifier.loadFolders.DefinedVersions().Count > 0)
			{
				List<LoadFolder> list = modWithIdentifier.LoadFoldersForVersion(VersionControl.CurrentVersionStringWithoutBuild);
				if (list != null && list.Count > 0)
				{
					this.<InitLoadFolders>g__AddFolders|45_0(list);
					return;
				}
				int num = VersionControl.CurrentVersion.Major;
				int num2 = VersionControl.CurrentVersion.Minor;
				List<LoadFolder> list2;
				do
				{
					if (num2 == 0)
					{
						num--;
						num2 = 9;
					}
					else
					{
						num2--;
					}
					if (num < 1)
					{
						goto IL_B4;
					}
					list2 = modWithIdentifier.LoadFoldersForVersion(num + "." + num2);
				}
				while (list2 == null);
				this.<InitLoadFolders>g__AddFolders|45_0(list2);
				return;
				IL_B4:
				List<LoadFolder> list3 = modWithIdentifier.LoadFoldersForVersion("default");
				if (list3 != null)
				{
					this.<InitLoadFolders>g__AddFolders|45_0(list3);
					return;
				}
			}
			if (this.foldersToLoadDescendingOrder.Count == 0)
			{
				string text = Path.Combine(this.RootDir, VersionControl.CurrentVersionStringWithoutBuild);
				if (Directory.Exists(text))
				{
					this.foldersToLoadDescendingOrder.Add(text);
				}
				else
				{
					Version version = new Version(0, 0);
					DirectoryInfo[] directories = this.rootDirInt.GetDirectories();
					for (int i = 0; i < directories.Length; i++)
					{
						Version version2;
						if (VersionControl.TryParseVersionString(directories[i].Name, out version2) && version2 > version)
						{
							version = version2;
						}
					}
					if (version.Major > 0)
					{
						this.foldersToLoadDescendingOrder.Add(Path.Combine(this.RootDir, version.ToString()));
					}
				}
				string text2 = Path.Combine(this.RootDir, ModContentPack.CommonFolderName);
				if (Directory.Exists(text2))
				{
					this.foldersToLoadDescendingOrder.Add(text2);
				}
				this.foldersToLoadDescendingOrder.Add(this.RootDir);
			}
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x000D201C File Offset: 0x000D021C
		private void LoadPatches()
		{
			DeepProfiler.Start("Loading all patches");
			this.patches = new List<PatchOperation>();
			this.loadedAnyPatches = false;
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Patches/", null).ToList<LoadableXmlAsset>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlElement documentElement = list[i].xmlDoc.DocumentElement;
				if (documentElement.Name != "Patch")
				{
					Log.Error(string.Format("Unexpected document element in patch XML; got {0}, expected 'Patch'", documentElement.Name), false);
				}
				else
				{
					foreach (object obj in documentElement.ChildNodes)
					{
						XmlNode xmlNode = (XmlNode)obj;
						if (xmlNode.NodeType == XmlNodeType.Element)
						{
							if (xmlNode.Name != "Operation")
							{
								Log.Error(string.Format("Unexpected element in patch XML; got {0}, expected 'Operation'", xmlNode.Name), false);
							}
							else
							{
								PatchOperation patchOperation = DirectXmlToObject.ObjectFromXml<PatchOperation>(xmlNode, false);
								patchOperation.sourceFile = list[i].FullFilePath;
								this.patches.Add(patchOperation);
								this.loadedAnyPatches = true;
							}
						}
					}
				}
			}
			DeepProfiler.End();
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x000D2164 File Offset: 0x000D0364
		public static Dictionary<string, FileInfo> GetAllFilesForMod(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, contentPath));
				if (directoryInfo.Exists)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
					{
						if (validateExtension == null || validateExtension(fileInfo.Extension))
						{
							string key = fileInfo.FullName.Substring(text.Length + 1);
							if (!dictionary.ContainsKey(key))
							{
								dictionary.Add(key, fileInfo);
							}
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x000D2220 File Offset: 0x000D0420
		public static List<Tuple<string, FileInfo>> GetAllFilesForModPreserveOrder(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			List<Tuple<string, FileInfo>> list2 = new List<Tuple<string, FileInfo>>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, contentPath));
				if (directoryInfo.Exists)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
					{
						if (validateExtension == null || validateExtension(fileInfo.Extension))
						{
							string item = fileInfo.FullName.Substring(text.Length + 1);
							list2.Add(new Tuple<string, FileInfo>(item, fileInfo));
						}
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			for (int k = list2.Count - 1; k >= 0; k--)
			{
				Tuple<string, FileInfo> tuple = list2[k];
				if (!hashSet.Contains(tuple.Item1))
				{
					hashSet.Add(tuple.Item1);
				}
				else
				{
					list2.RemoveAt(k);
				}
			}
			return list2;
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0001524D File Offset: 0x0001344D
		public bool AnyContentLoaded()
		{
			return this.AnyNonTranslationContentLoaded() || this.AnyTranslationsLoaded();
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x000D232C File Offset: 0x000D052C
		public bool AnyNonTranslationContentLoaded()
		{
			return (this.textures.contentList != null && this.textures.contentList.Count != 0) || (this.audioClips.contentList != null && this.audioClips.contentList.Count != 0) || (this.strings.contentList != null && this.strings.contentList.Count != 0) || !this.assemblies.loadedAssemblies.NullOrEmpty<Assembly>() || !this.assetBundles.loadedAssetBundles.NullOrEmpty<AssetBundle>() || this.loadedAnyPatches || this.AllDefs.Any<Def>();
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x000D23E0 File Offset: 0x000D05E0
		public bool AnyTranslationsLoaded()
		{
			foreach (string path in this.foldersToLoadDescendingOrder)
			{
				string path2 = Path.Combine(path, "Languages");
				if (Directory.Exists(path2) && Directory.EnumerateFiles(path2, "*", SearchOption.AllDirectories).Any<string>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x00015264 File Offset: 0x00013464
		public void ClearPatchesCache()
		{
			this.patches = null;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0001526D File Offset: 0x0001346D
		public void AddDef(Def def, string source = "Unknown")
		{
			def.modContentPack = this;
			def.fileName = source;
			this.defs.Add(def);
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00015289 File Offset: 0x00013489
		public override string ToString()
		{
			return this.PackageIdPlayerFacing;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x000D2458 File Offset: 0x000D0658
		[CompilerGenerated]
		private void <InitLoadFolders>g__AddFolders|45_0(List<LoadFolder> folders)
		{
			for (int i = folders.Count - 1; i >= 0; i--)
			{
				if (folders[i].ShouldLoad)
				{
					this.foldersToLoadDescendingOrder.Add(Path.Combine(this.RootDir, folders[i].folderName));
				}
			}
		}

		// Token: 0x04001067 RID: 4199
		private DirectoryInfo rootDirInt;

		// Token: 0x04001068 RID: 4200
		public int loadOrder;

		// Token: 0x04001069 RID: 4201
		private string nameInt;

		// Token: 0x0400106A RID: 4202
		private string packageIdInt;

		// Token: 0x0400106B RID: 4203
		private string packageIdPlayerFacingInt;

		// Token: 0x0400106C RID: 4204
		private ModContentHolder<AudioClip> audioClips;

		// Token: 0x0400106D RID: 4205
		private ModContentHolder<Texture2D> textures;

		// Token: 0x0400106E RID: 4206
		private ModContentHolder<string> strings;

		// Token: 0x0400106F RID: 4207
		public ModAssetBundlesHandler assetBundles;

		// Token: 0x04001070 RID: 4208
		public ModAssemblyHandler assemblies;

		// Token: 0x04001071 RID: 4209
		private List<PatchOperation> patches;

		// Token: 0x04001072 RID: 4210
		private List<Def> defs = new List<Def>();

		// Token: 0x04001073 RID: 4211
		private List<List<string>> allAssetNamesInBundleCached;

		// Token: 0x04001074 RID: 4212
		public List<string> foldersToLoadDescendingOrder;

		// Token: 0x04001075 RID: 4213
		private bool loadedAnyPatches;

		// Token: 0x04001076 RID: 4214
		public static readonly string LudeonPackageIdAuthor = "ludeon";

		// Token: 0x04001077 RID: 4215
		public static readonly string CoreModPackageId = "ludeon.rimworld";

		// Token: 0x04001078 RID: 4216
		public static readonly string RoyaltyModPackageId = "ludeon.rimworld.royalty";

		// Token: 0x04001079 RID: 4217
		public static readonly string CommonFolderName = "Common";
	}
}
