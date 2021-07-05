using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200023D RID: 573
	public class ModMetaData : WorkshopUploadable
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x0005D800 File Offset: 0x0005BA00
		public Texture2D PreviewImage
		{
			get
			{
				if (this.previewImageWasLoaded)
				{
					return this.previewImage;
				}
				if (File.Exists(this.PreviewImagePath))
				{
					this.previewImage = new Texture2D(0, 0);
					this.previewImage.LoadImage(File.ReadAllBytes(this.PreviewImagePath));
				}
				this.previewImageWasLoaded = true;
				return this.previewImage;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001065 RID: 4197 RVA: 0x0005D85A File Offset: 0x0005BA5A
		public string FolderName
		{
			get
			{
				return this.RootDir.Name;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001066 RID: 4198 RVA: 0x0005D867 File Offset: 0x0005BA67
		public DirectoryInfo RootDir
		{
			get
			{
				return this.rootDirInt;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001067 RID: 4199 RVA: 0x0005D86F File Offset: 0x0005BA6F
		public bool IsCoreMod
		{
			get
			{
				return this.SamePackageId(ModContentPack.CoreModPackageId, false);
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x0005D87D File Offset: 0x0005BA7D
		// (set) Token: 0x06001069 RID: 4201 RVA: 0x0005D885 File Offset: 0x0005BA85
		public bool Active
		{
			get
			{
				return ModsConfig.IsActive(this);
			}
			set
			{
				ModsConfig.SetActive(this, value);
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x0005D88E File Offset: 0x0005BA8E
		public bool VersionCompatible
		{
			get
			{
				if (this.IsCoreMod)
				{
					return true;
				}
				return this.meta.SupportedVersions.Any((System.Version v) => VersionControl.IsCompatible(v));
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x0005D8C9 File Offset: 0x0005BAC9
		public bool MadeForNewerVersion
		{
			get
			{
				if (this.VersionCompatible)
				{
					return false;
				}
				return this.meta.SupportedVersions.Any((System.Version v) => v.Major > VersionControl.CurrentMajor || (v.Major == VersionControl.CurrentMajor && v.Minor > VersionControl.CurrentMinor));
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x0005D904 File Offset: 0x0005BB04
		public ExpansionDef Expansion
		{
			get
			{
				return ModLister.GetExpansionWithIdentifier(this.PackageId);
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x0005D914 File Offset: 0x0005BB14
		public string Name
		{
			get
			{
				ExpansionDef expansion = this.Expansion;
				if (expansion == null)
				{
					return this.meta.name;
				}
				return expansion.label;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x0005D940 File Offset: 0x0005BB40
		public string Description
		{
			get
			{
				if (this.descriptionCached == null)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(this.PackageId);
					this.descriptionCached = ((expansionWithIdentifier != null) ? expansionWithIdentifier.description : this.meta.description);
				}
				return this.descriptionCached;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x0005D983 File Offset: 0x0005BB83
		public string AuthorsString
		{
			get
			{
				return this.Authors.ToCommaList(true, false);
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x0005D992 File Offset: 0x0005BB92
		public IEnumerable<string> Authors
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.meta.author))
				{
					foreach (string text in this.meta.author.Split(ModMetaData.AndToken, StringSplitOptions.RemoveEmptyEntries).SelectMany((string x) => x.Split(new char[]
					{
						','
					})))
					{
						yield return text;
					}
					IEnumerator<string> enumerator = null;
				}
				if (this.meta.authors != null)
				{
					foreach (string text2 in this.meta.authors)
					{
						yield return text2;
					}
					List<string>.Enumerator enumerator2 = default(List<string>.Enumerator);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0005D9A2 File Offset: 0x0005BBA2
		public string Url
		{
			get
			{
				return this.meta.url;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x0005D9AF File Offset: 0x0005BBAF
		public int SteamAppId
		{
			get
			{
				return this.meta.steamAppId;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x0005D9BC File Offset: 0x0005BBBC
		public List<System.Version> SupportedVersionsReadOnly
		{
			get
			{
				return this.meta.SupportedVersions;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x0005D9C9 File Offset: 0x0005BBC9
		IEnumerable<System.Version> WorkshopUploadable.SupportedVersions
		{
			get
			{
				return this.SupportedVersionsReadOnly;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x0005D9D4 File Offset: 0x0005BBD4
		public string PreviewImagePath
		{
			get
			{
				return string.Concat(new string[]
				{
					this.rootDirInt.FullName,
					Path.DirectorySeparatorChar.ToString(),
					"About",
					Path.DirectorySeparatorChar.ToString(),
					"Preview.png"
				});
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x0005DA2A File Offset: 0x0005BC2A
		public bool Official
		{
			get
			{
				return this.IsCoreMod || this.Source == ContentSource.OfficialModsFolder;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x0005DA3F File Offset: 0x0005BC3F
		public ContentSource Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x0005DA47 File Offset: 0x0005BC47
		public string PackageId
		{
			get
			{
				if (!this.appendPackageIdSteamPostfix)
				{
					return this.packageIdLowerCase;
				}
				return this.packageIdLowerCase + ModMetaData.SteamModPostfix;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x0005DA68 File Offset: 0x0005BC68
		public string PackageIdNonUnique
		{
			get
			{
				return this.packageIdLowerCase;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x0005DA70 File Offset: 0x0005BC70
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.meta.packageId;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x0005DA7D File Offset: 0x0005BC7D
		public List<ModDependency> Dependencies
		{
			get
			{
				return this.meta.modDependencies;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x0005DA8A File Offset: 0x0005BC8A
		public List<string> LoadBefore
		{
			get
			{
				return this.meta.loadBefore;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0005DA97 File Offset: 0x0005BC97
		public List<string> LoadAfter
		{
			get
			{
				return this.meta.loadAfter;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x0005DAA4 File Offset: 0x0005BCA4
		public List<string> ForceLoadBefore
		{
			get
			{
				return this.meta.forceLoadBefore;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0005DAB1 File Offset: 0x0005BCB1
		public List<string> ForceLoadAfter
		{
			get
			{
				return this.meta.forceLoadAfter;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x0005DABE File Offset: 0x0005BCBE
		public List<string> IncompatibleWith
		{
			get
			{
				return this.meta.incompatibleWith;
			}
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0005DACC File Offset: 0x0005BCCC
		public List<string> UnsatisfiedDependencies()
		{
			this.unsatisfiedDepsList.Clear();
			for (int i = 0; i < this.Dependencies.Count; i++)
			{
				ModDependency modDependency = this.Dependencies[i];
				if (!modDependency.IsSatisfied)
				{
					this.unsatisfiedDepsList.Add(modDependency.displayName);
				}
			}
			return this.unsatisfiedDepsList;
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06001082 RID: 4226 RVA: 0x0005DB26 File Offset: 0x0005BD26
		// (set) Token: 0x06001083 RID: 4227 RVA: 0x0005DB2E File Offset: 0x0005BD2E
		public bool HadIncorrectlyFormattedVersionInMetadata { get; private set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001084 RID: 4228 RVA: 0x0005DB37 File Offset: 0x0005BD37
		// (set) Token: 0x06001085 RID: 4229 RVA: 0x0005DB3F File Offset: 0x0005BD3F
		public bool HadIncorrectlyFormattedPackageId { get; private set; }

		// Token: 0x06001086 RID: 4230 RVA: 0x0005DB48 File Offset: 0x0005BD48
		public ModMetaData(string localAbsPath, bool official = false)
		{
			this.rootDirInt = new DirectoryInfo(localAbsPath);
			this.source = (official ? ContentSource.OfficialModsFolder : ContentSource.ModsFolder);
			this.Init();
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0005DBA4 File Offset: 0x0005BDA4
		public ModMetaData(WorkshopItem workshopItem)
		{
			this.rootDirInt = workshopItem.Directory;
			this.source = ContentSource.SteamWorkshop;
			this.Init();
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0005DBF8 File Offset: 0x0005BDF8
		public void UnsetPreviewImage()
		{
			this.previewImage = null;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0005DC01 File Offset: 0x0005BE01
		public bool SamePackageId(string otherPackageId, bool ignorePostfix = false)
		{
			if (this.PackageId == null)
			{
				return false;
			}
			if (ignorePostfix)
			{
				return this.packageIdLowerCase.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
			}
			return this.PackageId.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0005DC2B File Offset: 0x0005BE2B
		public List<LoadFolder> LoadFoldersForVersion(string version)
		{
			ModLoadFolders modLoadFolders = this.loadFolders;
			if (modLoadFolders == null)
			{
				return null;
			}
			return modLoadFolders.FoldersForVersion(version);
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0005DC40 File Offset: 0x0005BE40
		private void Init()
		{
			this.meta = DirectXmlLoader.ItemFromXmlFile<ModMetaData.ModMetaDataInternal>(string.Concat(new string[]
			{
				this.RootDir.FullName,
				Path.DirectorySeparatorChar.ToString(),
				"About",
				Path.DirectorySeparatorChar.ToString(),
				"About.xml"
			}), true);
			this.loadFolders = DirectXmlLoader.ItemFromXmlFile<ModLoadFolders>(this.RootDir.FullName + Path.DirectorySeparatorChar.ToString() + "LoadFolders.xml", true);
			bool shouldLogIssues = ModLister.ShouldLogIssues;
			this.HadIncorrectlyFormattedVersionInMetadata = !this.meta.TryParseSupportedVersions(!this.OnSteamWorkshop && shouldLogIssues);
			if (this.meta.name.NullOrEmpty())
			{
				if (this.OnSteamWorkshop)
				{
					this.meta.name = "Workshop mod " + this.FolderName;
				}
				else
				{
					this.meta.name = this.FolderName;
				}
			}
			this.HadIncorrectlyFormattedPackageId = !this.meta.TryParsePackageId(this.Official, !this.OnSteamWorkshop && shouldLogIssues);
			this.packageIdLowerCase = this.meta.packageId.ToLower();
			this.meta.InitVersionedData();
			this.meta.ValidateDependencies(shouldLogIssues);
			string publishedFileIdPath = this.PublishedFileIdPath;
			ulong value;
			if (File.Exists(this.PublishedFileIdPath) && ulong.TryParse(File.ReadAllText(publishedFileIdPath), out value))
			{
				this.publishedFileIdInt = new PublishedFileId_t(value);
			}
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0005DDBE File Offset: 0x0005BFBE
		internal void DeleteContent()
		{
			this.rootDirInt.Delete(true);
			ModLister.RebuildModList();
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x0005DDD1 File Offset: 0x0005BFD1
		public bool OnSteamWorkshop
		{
			get
			{
				return this.source == ContentSource.SteamWorkshop;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x0005DDDC File Offset: 0x0005BFDC
		private string PublishedFileIdPath
		{
			get
			{
				return string.Concat(new string[]
				{
					this.rootDirInt.FullName,
					Path.DirectorySeparatorChar.ToString(),
					"About",
					Path.DirectorySeparatorChar.ToString(),
					"PublishedFileId.txt"
				});
			}
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0000313F File Offset: 0x0000133F
		public void PrepareForWorkshopUpload()
		{
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0005DE32 File Offset: 0x0005C032
		public bool CanToUploadToWorkshop()
		{
			return !this.Official && this.Source == ContentSource.ModsFolder && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0005DE59 File Offset: 0x0005C059
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0005DE61 File Offset: 0x0005C061
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			if (this.publishedFileIdInt == newPfid)
			{
				return;
			}
			this.publishedFileIdInt = newPfid;
			File.WriteAllText(this.PublishedFileIdPath, newPfid.ToString());
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0005DE91 File Offset: 0x0005C091
		public string GetWorkshopName()
		{
			return this.Name;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0005DE99 File Offset: 0x0005C099
		public string GetWorkshopDescription()
		{
			return this.Description;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0005DEA1 File Offset: 0x0005C0A1
		public string GetWorkshopPreviewImagePath()
		{
			return this.PreviewImagePath;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0005DEA9 File Offset: 0x0005C0A9
		public IList<string> GetWorkshopTags()
		{
			if (!this.translationMod)
			{
				return new List<string>
				{
					"Mod"
				};
			}
			return new List<string>
			{
				"Translation"
			};
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0005DED4 File Offset: 0x0005C0D4
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return this.RootDir;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0005DEDC File Offset: 0x0005C0DC
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0005DEF8 File Offset: 0x0005C0F8
		public IEnumerable<ModRequirement> GetRequirements()
		{
			int num;
			for (int i = 0; i < this.Dependencies.Count; i = num + 1)
			{
				yield return this.Dependencies[i];
				num = i;
			}
			for (int i = 0; i < this.meta.incompatibleWith.Count; i = num + 1)
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.meta.incompatibleWith[i], false);
				if (modWithIdentifier != null)
				{
					yield return new ModIncompatibility
					{
						packageId = modWithIdentifier.PackageIdPlayerFacing,
						displayName = modWithIdentifier.Name
					};
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0005DF08 File Offset: 0x0005C108
		public override int GetHashCode()
		{
			return this.PackageId.GetHashCode();
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0005DF15 File Offset: 0x0005C115
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.PackageIdPlayerFacing,
				"|",
				this.Name,
				"]"
			});
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0005DF4C File Offset: 0x0005C14C
		public string ToStringLong()
		{
			return this.PackageIdPlayerFacing + "(" + this.RootDir.ToString() + ")";
		}

		// Token: 0x04000CAD RID: 3245
		private DirectoryInfo rootDirInt;

		// Token: 0x04000CAE RID: 3246
		private ContentSource source;

		// Token: 0x04000CAF RID: 3247
		private Texture2D previewImage;

		// Token: 0x04000CB0 RID: 3248
		private bool previewImageWasLoaded;

		// Token: 0x04000CB1 RID: 3249
		public bool enabled = true;

		// Token: 0x04000CB2 RID: 3250
		private ModMetaData.ModMetaDataInternal meta = new ModMetaData.ModMetaDataInternal();

		// Token: 0x04000CB3 RID: 3251
		public ModLoadFolders loadFolders;

		// Token: 0x04000CB4 RID: 3252
		private WorkshopItemHook workshopHookInt;

		// Token: 0x04000CB5 RID: 3253
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x04000CB6 RID: 3254
		public bool appendPackageIdSteamPostfix;

		// Token: 0x04000CB7 RID: 3255
		public bool translationMod;

		// Token: 0x04000CB8 RID: 3256
		private string packageIdLowerCase;

		// Token: 0x04000CB9 RID: 3257
		private string descriptionCached;

		// Token: 0x04000CBA RID: 3258
		private const string AboutFolderName = "About";

		// Token: 0x04000CBB RID: 3259
		public static readonly string SteamModPostfix = "_steam";

		// Token: 0x04000CBC RID: 3260
		private static readonly string[] AndToken = new string[]
		{
			" and "
		};

		// Token: 0x04000CBD RID: 3261
		private List<string> unsatisfiedDepsList = new List<string>();

		// Token: 0x020019B9 RID: 6585
		private class ModMetaDataInternal
		{
			// Token: 0x1700195E RID: 6494
			// (get) Token: 0x060099E1 RID: 39393 RVA: 0x003629C1 File Offset: 0x00360BC1
			// (set) Token: 0x060099E2 RID: 39394 RVA: 0x003629C9 File Offset: 0x00360BC9
			public List<System.Version> SupportedVersions { get; private set; }

			// Token: 0x060099E3 RID: 39395 RVA: 0x003629D4 File Offset: 0x00360BD4
			private bool TryParseVersion(string str, bool logIssues = true)
			{
				System.Version version;
				if (!VersionControl.TryParseVersionString(str, out version))
				{
					if (logIssues)
					{
						Log.Error(string.Concat(new string[]
						{
							"Unable to parse version string on mod ",
							this.name,
							" from ",
							this.author,
							" \"",
							str,
							"\""
						}));
					}
					return false;
				}
				this.SupportedVersions.Add(version);
				if (!VersionControl.IsWellFormattedVersionString(str))
				{
					if (logIssues)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Malformed (correct format is Major.Minor) version string on mod ",
							this.name,
							" from ",
							this.author,
							" \"",
							str,
							"\" - parsed as \"",
							version.Major.ToString(),
							".",
							version.Minor.ToString(),
							"\""
						}));
					}
					return false;
				}
				return true;
			}

			// Token: 0x060099E4 RID: 39396 RVA: 0x00362AD0 File Offset: 0x00360CD0
			public bool TryParseSupportedVersions(bool logIssues = true)
			{
				if (this.targetVersion != null && logIssues)
				{
					Log.Warning("Mod " + this.name + ": targetVersion field is obsolete, use supportedVersions instead.");
				}
				bool flag = false;
				this.SupportedVersions = new List<System.Version>();
				if (this.packageId.ToLower() == ModContentPack.CoreModPackageId)
				{
					this.SupportedVersions.Add(VersionControl.CurrentVersion);
				}
				else if (this.supportedVersions == null)
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " is missing supported versions list in About.xml! (example: <supportedVersions><li>1.0</li></supportedVersions>)");
					}
					flag = true;
				}
				else if (this.supportedVersions.Count == 0)
				{
					if (logIssues)
					{
						Log.Error("Mod " + this.name + ": <supportedVersions> in mod About.xml must specify at least one version.");
					}
					flag = true;
				}
				else
				{
					for (int i = 0; i < this.supportedVersions.Count; i++)
					{
						flag |= !this.TryParseVersion(this.supportedVersions[i], logIssues);
					}
				}
				this.SupportedVersions = this.SupportedVersions.OrderBy(delegate(System.Version v)
				{
					if (!VersionControl.IsCompatible(v))
					{
						return 100;
					}
					return -100;
				}).ThenByDescending((System.Version v) => v.Major).ThenByDescending((System.Version v) => v.Minor).Distinct<System.Version>().ToList<System.Version>();
				return !flag;
			}

			// Token: 0x060099E5 RID: 39397 RVA: 0x00362C4C File Offset: 0x00360E4C
			public bool TryParsePackageId(bool isOfficial, bool logIssues = true)
			{
				bool flag = false;
				if (this.packageId.NullOrEmpty())
				{
					string text = "none";
					if (!this.description.NullOrEmpty())
					{
						text = GenText.StableStringHash(this.description).ToString().Replace("-", "");
						text = text.Substring(0, Math.Min(3, text.Length));
					}
					this.packageId = this.ConvertToASCII(this.author + text) + "." + this.ConvertToASCII(this.name);
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " is missing packageId in About.xml! (example: <packageId>AuthorName.ModName.Specific</packageId>)");
					}
					flag = true;
				}
				if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(this.packageId))
				{
					if (logIssues)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Mod ",
							this.name,
							" <packageId> (",
							this.packageId,
							") is not in valid format."
						}));
					}
					flag = true;
				}
				if (!isOfficial && this.packageId.ToLower().Contains(ModContentPack.LudeonPackageIdAuthor))
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " <packageId> contains word \"Ludeon\", which is reserved for official content.");
					}
					flag = true;
				}
				return !flag;
			}

			// Token: 0x060099E6 RID: 39398 RVA: 0x00362D90 File Offset: 0x00360F90
			private string ConvertToASCII(string part)
			{
				StringBuilder stringBuilder = new StringBuilder("");
				foreach (char c in part)
				{
					if (!char.IsLetterOrDigit(c) || c >= '\u0080')
					{
						c = c % '\u0019' + 'A';
					}
					stringBuilder.Append(c);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x060099E7 RID: 39399 RVA: 0x00362DE8 File Offset: 0x00360FE8
			public void ValidateDependencies(bool logIssues = true)
			{
				for (int i = this.modDependencies.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					ModDependency modDependency = this.modDependencies[i];
					if (modDependency.packageId.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with no <packageId> specified.");
						}
						flag = true;
					}
					else if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(modDependency.packageId))
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with invalid <packageId>: " + modDependency.packageId);
						}
						flag = true;
					}
					if (modDependency.displayName.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Mod ",
								this.name,
								" has a dependency (",
								modDependency.packageId,
								") with empty display name."
							}));
						}
						flag = true;
					}
					if (modDependency.downloadUrl.NullOrEmpty() && modDependency.steamWorkshopUrl.NullOrEmpty() && !modDependency.packageId.ToLower().Contains(ModContentPack.LudeonPackageIdAuthor))
					{
						if (logIssues)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Mod ",
								this.name,
								" dependency (",
								modDependency.packageId,
								") needs to have <downloadUrl> and/or <steamWorkshopUrl> specified."
							}));
						}
						flag = true;
					}
					if (flag)
					{
						this.modDependencies.Remove(modDependency);
					}
				}
			}

			// Token: 0x060099E8 RID: 39400 RVA: 0x00362F54 File Offset: 0x00361154
			public void InitVersionedData()
			{
				string currentVersionStringWithoutBuild = VersionControl.CurrentVersionStringWithoutBuild;
				ModMetaData.VersionedData<string> versionedData = this.descriptionsByVersion;
				string text = (versionedData != null) ? versionedData.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (text != null)
				{
					this.description = text;
				}
				ModMetaData.VersionedData<List<ModDependency>> versionedData2 = this.modDependenciesByVersion;
				List<ModDependency> list = (versionedData2 != null) ? versionedData2.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list != null)
				{
					this.modDependencies = list;
				}
				ModMetaData.VersionedData<List<string>> versionedData3 = this.loadBeforeByVersion;
				List<string> list2 = (versionedData3 != null) ? versionedData3.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list2 != null)
				{
					this.loadBefore = list2;
				}
				ModMetaData.VersionedData<List<string>> versionedData4 = this.loadAfterByVersion;
				List<string> list3 = (versionedData4 != null) ? versionedData4.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list3 != null)
				{
					this.loadAfter = list3;
				}
				ModMetaData.VersionedData<List<string>> versionedData5 = this.incompatibleWithByVersion;
				List<string> list4 = (versionedData5 != null) ? versionedData5.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list4 != null)
				{
					this.incompatibleWith = list4;
				}
			}

			// Token: 0x040062B3 RID: 25267
			public string packageId = "";

			// Token: 0x040062B4 RID: 25268
			public string name = "";

			// Token: 0x040062B5 RID: 25269
			public string author = "Anonymous";

			// Token: 0x040062B6 RID: 25270
			public List<string> authors;

			// Token: 0x040062B7 RID: 25271
			public string url = "";

			// Token: 0x040062B8 RID: 25272
			public string description = "No description provided.";

			// Token: 0x040062B9 RID: 25273
			public int steamAppId;

			// Token: 0x040062BA RID: 25274
			public List<string> supportedVersions;

			// Token: 0x040062BB RID: 25275
			[Unsaved(true)]
			private string targetVersion;

			// Token: 0x040062BC RID: 25276
			public List<ModDependency> modDependencies = new List<ModDependency>();

			// Token: 0x040062BD RID: 25277
			public List<string> loadBefore = new List<string>();

			// Token: 0x040062BE RID: 25278
			public List<string> loadAfter = new List<string>();

			// Token: 0x040062BF RID: 25279
			public List<string> incompatibleWith = new List<string>();

			// Token: 0x040062C0 RID: 25280
			public List<string> forceLoadBefore = new List<string>();

			// Token: 0x040062C1 RID: 25281
			public List<string> forceLoadAfter = new List<string>();

			// Token: 0x040062C2 RID: 25282
			private ModMetaData.VersionedData<string> descriptionsByVersion;

			// Token: 0x040062C3 RID: 25283
			private ModMetaData.VersionedData<List<ModDependency>> modDependenciesByVersion;

			// Token: 0x040062C4 RID: 25284
			private ModMetaData.VersionedData<List<string>> loadBeforeByVersion;

			// Token: 0x040062C5 RID: 25285
			private ModMetaData.VersionedData<List<string>> loadAfterByVersion;

			// Token: 0x040062C6 RID: 25286
			private ModMetaData.VersionedData<List<string>> incompatibleWithByVersion;

			// Token: 0x040062C7 RID: 25287
			public static readonly Regex PackageIdFormatRegex = new Regex("(?=.{1,60}$)^(?!\\.)(?=.*?[.])(?!.*([.])\\1+)[a-zA-Z0-9.]{1,}[a-zA-Z0-9]{1}$");
		}

		// Token: 0x020019BA RID: 6586
		private class VersionedData<T> where T : class
		{
			// Token: 0x060099EB RID: 39403 RVA: 0x003630A4 File Offset: 0x003612A4
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				foreach (object obj in xmlRoot.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (!(xmlNode is XmlComment))
					{
						string text = xmlNode.Name.ToLower();
						if (text.StartsWith("v"))
						{
							text = text.Substring(1);
						}
						if (!this.itemForVersion.ContainsKey(text))
						{
							this.itemForVersion[text] = ((typeof(T) == typeof(string)) ? ((T)((object)xmlNode.FirstChild.Value)) : DirectXmlToObject.ObjectFromXml<T>(xmlNode, false));
						}
						else
						{
							Log.Warning("More than one value for a same version of " + typeof(T).Name + " named " + xmlRoot.Name);
						}
					}
				}
			}

			// Token: 0x060099EC RID: 39404 RVA: 0x003631A4 File Offset: 0x003613A4
			public T GetItemForVersion(string ver)
			{
				if (this.itemForVersion.ContainsKey(ver))
				{
					return this.itemForVersion[ver];
				}
				return default(T);
			}

			// Token: 0x040062C9 RID: 25289
			private Dictionary<string, T> itemForVersion = new Dictionary<string, T>();
		}
	}
}
