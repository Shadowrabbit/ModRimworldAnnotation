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
	// Token: 0x0200023B RID: 571
	public class ModContentPack
	{
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x0005C57E File Offset: 0x0005A77E
		public string RootDir
		{
			get
			{
				return this.rootDirInt.FullName;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x0005C58B File Offset: 0x0005A78B
		public string PackageId
		{
			get
			{
				return this.packageIdInt;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x0005C593 File Offset: 0x0005A793
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.packageIdPlayerFacingInt;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x0005C59B File Offset: 0x0005A79B
		public int SteamAppId
		{
			get
			{
				ModMetaData modMetaData = this.ModMetaData;
				if (modMetaData == null)
				{
					return 0;
				}
				return modMetaData.SteamAppId;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0005C5AE File Offset: 0x0005A7AE
		public ModMetaData ModMetaData
		{
			get
			{
				return ModLister.GetModWithIdentifier(this.PackageId, false);
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0005C5BC File Offset: 0x0005A7BC
		public string FolderName
		{
			get
			{
				return this.rootDirInt.Name;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x0005C5C9 File Offset: 0x0005A7C9
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x0005C5D1 File Offset: 0x0005A7D1
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

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x0005C5DE File Offset: 0x0005A7DE
		public bool IsCoreMod
		{
			get
			{
				return this.PackageId == ModContentPack.CoreModPackageId;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x0005C5F0 File Offset: 0x0005A7F0
		public bool IsOfficialMod
		{
			get
			{
				return this.official;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x0005C5F8 File Offset: 0x0005A7F8
		public IEnumerable<Def> AllDefs
		{
			get
			{
				return this.defs;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x0005C600 File Offset: 0x0005A800
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

		// Token: 0x0600103B RID: 4155 RVA: 0x0005C618 File Offset: 0x0005A818
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

		// Token: 0x0600103C RID: 4156 RVA: 0x0005C6A0 File Offset: 0x0005A8A0
		public ModContentPack(DirectoryInfo directory, string packageId, string packageIdPlayerFacing, int loadOrder, string name, bool official)
		{
			this.rootDirInt = directory;
			this.loadOrder = loadOrder;
			this.nameInt = name;
			this.official = official;
			this.packageIdInt = packageId.ToLower();
			this.packageIdPlayerFacingInt = packageIdPlayerFacing;
			this.audioClips = new ModContentHolder<AudioClip>(this);
			this.textures = new ModContentHolder<Texture2D>(this);
			this.strings = new ModContentHolder<string>(this);
			this.assetBundles = new ModAssetBundlesHandler(this);
			this.assemblies = new ModAssemblyHandler(this);
			this.InitLoadFolders();
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0005C732 File Offset: 0x0005A932
		public void ClearDestroy()
		{
			this.audioClips.ClearDestroy();
			this.textures.ClearDestroy();
			this.assetBundles.ClearDestroy();
			this.allAssetNamesInBundleCached = null;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0005C75C File Offset: 0x0005A95C
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
			Log.Error("Mod lacks manager for asset type " + this.strings);
			return null;
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0005C7F4 File Offset: 0x0005A9F4
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

		// Token: 0x06001040 RID: 4160 RVA: 0x0005C8B0 File Offset: 0x0005AAB0
		public void ReloadContent()
		{
			LongEventHandler.ExecuteWhenFinished(new Action(this.ReloadContentInt));
			this.assemblies.ReloadAll();
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0005C8CE File Offset: 0x0005AACE
		public IEnumerable<LoadableXmlAsset> LoadDefs()
		{
			if (this.defs.Count != 0)
			{
				Log.ErrorOnce("LoadDefs called with already existing def packages", 39029405);
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

		// Token: 0x06001042 RID: 4162 RVA: 0x0005C8E0 File Offset: 0x0005AAE0
		private void InitLoadFolders()
		{
			this.foldersToLoadDescendingOrder = new List<string>();
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.PackageId, false);
			if (((modWithIdentifier != null) ? modWithIdentifier.loadFolders : null) != null && modWithIdentifier.loadFolders.DefinedVersions().Count > 0)
			{
				List<LoadFolder> list = modWithIdentifier.LoadFoldersForVersion(VersionControl.CurrentVersionStringWithoutBuild);
				if (list != null && list.Count > 0)
				{
					this.<InitLoadFolders>g__AddFolders|52_0(list);
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
				this.<InitLoadFolders>g__AddFolders|52_0(list2);
				return;
				IL_B4:
				List<LoadFolder> list3 = modWithIdentifier.LoadFoldersForVersion("default");
				if (list3 != null)
				{
					this.<InitLoadFolders>g__AddFolders|52_0(list3);
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

		// Token: 0x06001043 RID: 4163 RVA: 0x0005CAA0 File Offset: 0x0005ACA0
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
					Log.Error(string.Format("Unexpected document element in patch XML; got {0}, expected 'Patch'", documentElement.Name));
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
								Log.Error(string.Format("Unexpected element in patch XML; got {0}, expected 'Operation'", xmlNode.Name));
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

		// Token: 0x06001044 RID: 4164 RVA: 0x0005CBE4 File Offset: 0x0005ADE4
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

		// Token: 0x06001045 RID: 4165 RVA: 0x0005CCA0 File Offset: 0x0005AEA0
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
					FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
					Array.Sort<FileInfo>(files, (FileInfo a, FileInfo b) => a.Name.CompareTo(b.Name));
					foreach (FileInfo fileInfo in files)
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

		// Token: 0x06001046 RID: 4166 RVA: 0x0005CDD2 File Offset: 0x0005AFD2
		public bool AnyContentLoaded()
		{
			return this.AnyNonTranslationContentLoaded() || this.AnyTranslationsLoaded();
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0005CDEC File Offset: 0x0005AFEC
		public bool AnyNonTranslationContentLoaded()
		{
			return (this.textures.contentList != null && this.textures.contentList.Count != 0) || (this.audioClips.contentList != null && this.audioClips.contentList.Count != 0) || (this.strings.contentList != null && this.strings.contentList.Count != 0) || !this.assemblies.loadedAssemblies.NullOrEmpty<Assembly>() || !this.assetBundles.loadedAssetBundles.NullOrEmpty<AssetBundle>() || this.loadedAnyPatches || this.AllDefs.Any<Def>();
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0005CEA0 File Offset: 0x0005B0A0
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

		// Token: 0x06001049 RID: 4169 RVA: 0x0005CF18 File Offset: 0x0005B118
		public void ClearPatchesCache()
		{
			this.patches = null;
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0005CF21 File Offset: 0x0005B121
		public void AddDef(Def def, string source = "Unknown")
		{
			def.modContentPack = this;
			def.fileName = source;
			this.defs.Add(def);
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0005CF3D File Offset: 0x0005B13D
		public override string ToString()
		{
			return this.PackageIdPlayerFacing;
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0005CF7C File Offset: 0x0005B17C
		[CompilerGenerated]
		private void <InitLoadFolders>g__AddFolders|52_0(List<LoadFolder> folders)
		{
			for (int i = folders.Count - 1; i >= 0; i--)
			{
				if (folders[i].ShouldLoad)
				{
					this.foldersToLoadDescendingOrder.Add(Path.Combine(this.RootDir, folders[i].folderName));
				}
			}
		}

		// Token: 0x04000C91 RID: 3217
		private DirectoryInfo rootDirInt;

		// Token: 0x04000C92 RID: 3218
		public int loadOrder;

		// Token: 0x04000C93 RID: 3219
		private string nameInt;

		// Token: 0x04000C94 RID: 3220
		private string packageIdInt;

		// Token: 0x04000C95 RID: 3221
		private string packageIdPlayerFacingInt;

		// Token: 0x04000C96 RID: 3222
		private bool official;

		// Token: 0x04000C97 RID: 3223
		private ModContentHolder<AudioClip> audioClips;

		// Token: 0x04000C98 RID: 3224
		private ModContentHolder<Texture2D> textures;

		// Token: 0x04000C99 RID: 3225
		private ModContentHolder<string> strings;

		// Token: 0x04000C9A RID: 3226
		public ModAssetBundlesHandler assetBundles;

		// Token: 0x04000C9B RID: 3227
		public ModAssemblyHandler assemblies;

		// Token: 0x04000C9C RID: 3228
		private List<PatchOperation> patches;

		// Token: 0x04000C9D RID: 3229
		private List<Def> defs = new List<Def>();

		// Token: 0x04000C9E RID: 3230
		private List<List<string>> allAssetNamesInBundleCached;

		// Token: 0x04000C9F RID: 3231
		public List<string> foldersToLoadDescendingOrder;

		// Token: 0x04000CA0 RID: 3232
		private bool loadedAnyPatches;

		// Token: 0x04000CA1 RID: 3233
		public static readonly string LudeonPackageIdAuthor = "ludeon";

		// Token: 0x04000CA2 RID: 3234
		public static readonly string CoreModPackageId = "ludeon.rimworld";

		// Token: 0x04000CA3 RID: 3235
		public static readonly string RoyaltyModPackageId = "ludeon.rimworld.royalty";

		// Token: 0x04000CA4 RID: 3236
		public static readonly string IdeologyModPackageId = "ludeon.rimworld.ideology";

		// Token: 0x04000CA5 RID: 3237
		public static readonly string CommonFolderName = "Common";
	}
}
