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
	// Token: 0x02000346 RID: 838
	public class ModMetaData : WorkshopUploadable
	{
		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001567 RID: 5479 RVA: 0x000D2D60 File Offset: 0x000D0F60
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

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001568 RID: 5480 RVA: 0x0001538B File Offset: 0x0001358B
		public string FolderName
		{
			get
			{
				return this.RootDir.Name;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001569 RID: 5481 RVA: 0x00015398 File Offset: 0x00013598
		public DirectoryInfo RootDir
		{
			get
			{
				return this.rootDirInt;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600156A RID: 5482 RVA: 0x000153A0 File Offset: 0x000135A0
		public bool IsCoreMod
		{
			get
			{
				return this.SamePackageId(ModContentPack.CoreModPackageId, false);
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600156B RID: 5483 RVA: 0x000153AE File Offset: 0x000135AE
		// (set) Token: 0x0600156C RID: 5484 RVA: 0x000153B6 File Offset: 0x000135B6
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

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600156D RID: 5485 RVA: 0x000153BF File Offset: 0x000135BF
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

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600156E RID: 5486 RVA: 0x000153FA File Offset: 0x000135FA
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

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600156F RID: 5487 RVA: 0x00015435 File Offset: 0x00013635
		public ExpansionDef Expansion
		{
			get
			{
				return ModLister.GetExpansionWithIdentifier(this.PackageId);
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001570 RID: 5488 RVA: 0x000D2DBC File Offset: 0x000D0FBC
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

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001571 RID: 5489 RVA: 0x000D2DE8 File Offset: 0x000D0FE8
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

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x00015442 File Offset: 0x00013642
		public string Author
		{
			get
			{
				return this.meta.author;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x0001544F File Offset: 0x0001364F
		public string Url
		{
			get
			{
				return this.meta.url;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x0001545C File Offset: 0x0001365C
		public int SteamAppId
		{
			get
			{
				return this.meta.steamAppId;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001575 RID: 5493 RVA: 0x000D2E2C File Offset: 0x000D102C
		[Obsolete("Deprecated, will be removed in the future. Use SupportedVersions instead")]
		public string TargetVersion
		{
			get
			{
				if (this.SupportedVersionsReadOnly.Count == 0)
				{
					return "Unknown";
				}
				System.Version version = this.meta.SupportedVersions[0];
				return version.Major + "." + version.Minor;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x00015469 File Offset: 0x00013669
		public List<System.Version> SupportedVersionsReadOnly
		{
			get
			{
				return this.meta.SupportedVersions;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001577 RID: 5495 RVA: 0x00015476 File Offset: 0x00013676
		IEnumerable<System.Version> WorkshopUploadable.SupportedVersions
		{
			get
			{
				return this.SupportedVersionsReadOnly;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x000D2E80 File Offset: 0x000D1080
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

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001579 RID: 5497 RVA: 0x0001547E File Offset: 0x0001367E
		public bool Official
		{
			get
			{
				return this.IsCoreMod || this.Source == ContentSource.OfficialModsFolder;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x00015493 File Offset: 0x00013693
		public ContentSource Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x0001549B File Offset: 0x0001369B
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

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x0600157C RID: 5500 RVA: 0x000154BC File Offset: 0x000136BC
		public string PackageIdNonUnique
		{
			get
			{
				return this.packageIdLowerCase;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600157D RID: 5501 RVA: 0x000154C4 File Offset: 0x000136C4
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.meta.packageId;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x0600157E RID: 5502 RVA: 0x000154D1 File Offset: 0x000136D1
		public List<ModDependency> Dependencies
		{
			get
			{
				return this.meta.modDependencies;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x0600157F RID: 5503 RVA: 0x000154DE File Offset: 0x000136DE
		public List<string> LoadBefore
		{
			get
			{
				return this.meta.loadBefore;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x000154EB File Offset: 0x000136EB
		public List<string> LoadAfter
		{
			get
			{
				return this.meta.loadAfter;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001581 RID: 5505 RVA: 0x000154F8 File Offset: 0x000136F8
		public List<string> IncompatibleWith
		{
			get
			{
				return this.meta.incompatibleWith;
			}
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x000D2ED8 File Offset: 0x000D10D8
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

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001583 RID: 5507 RVA: 0x00015505 File Offset: 0x00013705
		// (set) Token: 0x06001584 RID: 5508 RVA: 0x0001550D File Offset: 0x0001370D
		public bool HadIncorrectlyFormattedVersionInMetadata { get; private set; }

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x00015516 File Offset: 0x00013716
		// (set) Token: 0x06001586 RID: 5510 RVA: 0x0001551E File Offset: 0x0001371E
		public bool HadIncorrectlyFormattedPackageId { get; private set; }

		// Token: 0x06001587 RID: 5511 RVA: 0x000D2F34 File Offset: 0x000D1134
		public ModMetaData(string localAbsPath, bool official = false)
		{
			this.rootDirInt = new DirectoryInfo(localAbsPath);
			this.source = (official ? ContentSource.OfficialModsFolder : ContentSource.ModsFolder);
			this.Init();
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x000D2F90 File Offset: 0x000D1190
		public ModMetaData(WorkshopItem workshopItem)
		{
			this.rootDirInt = workshopItem.Directory;
			this.source = ContentSource.SteamWorkshop;
			this.Init();
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00015527 File Offset: 0x00013727
		public void UnsetPreviewImage()
		{
			this.previewImage = null;
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x00015530 File Offset: 0x00013730
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

		// Token: 0x0600158B RID: 5515 RVA: 0x0001555A File Offset: 0x0001375A
		public List<LoadFolder> LoadFoldersForVersion(string version)
		{
			ModLoadFolders modLoadFolders = this.loadFolders;
			if (modLoadFolders == null)
			{
				return null;
			}
			return modLoadFolders.FoldersForVersion(version);
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x000D2FE4 File Offset: 0x000D11E4
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
			this.meta.ValidateDependencies_NewTmp(shouldLogIssues);
			string publishedFileIdPath = this.PublishedFileIdPath;
			ulong value;
			if (File.Exists(this.PublishedFileIdPath) && ulong.TryParse(File.ReadAllText(publishedFileIdPath), out value))
			{
				this.publishedFileIdInt = new PublishedFileId_t(value);
			}
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0001556E File Offset: 0x0001376E
		internal void DeleteContent()
		{
			this.rootDirInt.Delete(true);
			ModLister.RebuildModList();
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x00015581 File Offset: 0x00013781
		public bool OnSteamWorkshop
		{
			get
			{
				return this.source == ContentSource.SteamWorkshop;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x0600158F RID: 5519 RVA: 0x000D3164 File Offset: 0x000D1364
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

		// Token: 0x06001590 RID: 5520 RVA: 0x00006A05 File Offset: 0x00004C05
		public void PrepareForWorkshopUpload()
		{
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0001558C File Offset: 0x0001378C
		public bool CanToUploadToWorkshop()
		{
			return !this.Official && this.Source == ContentSource.ModsFolder && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x000155B3 File Offset: 0x000137B3
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x000155BB File Offset: 0x000137BB
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			if (this.publishedFileIdInt == newPfid)
			{
				return;
			}
			this.publishedFileIdInt = newPfid;
			File.WriteAllText(this.PublishedFileIdPath, newPfid.ToString());
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x000155EB File Offset: 0x000137EB
		public string GetWorkshopName()
		{
			return this.Name;
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x000155F3 File Offset: 0x000137F3
		public string GetWorkshopDescription()
		{
			return this.Description;
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x000155FB File Offset: 0x000137FB
		public string GetWorkshopPreviewImagePath()
		{
			return this.PreviewImagePath;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00015603 File Offset: 0x00013803
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

		// Token: 0x06001598 RID: 5528 RVA: 0x0001562E File Offset: 0x0001382E
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return this.RootDir;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00015636 File Offset: 0x00013836
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x00015652 File Offset: 0x00013852
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

		// Token: 0x0600159B RID: 5531 RVA: 0x00015662 File Offset: 0x00013862
		public override int GetHashCode()
		{
			return this.PackageId.GetHashCode();
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x0001566F File Offset: 0x0001386F
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

		// Token: 0x0600159D RID: 5533 RVA: 0x000156A6 File Offset: 0x000138A6
		public string ToStringLong()
		{
			return this.PackageIdPlayerFacing + "(" + this.RootDir.ToString() + ")";
		}

		// Token: 0x0400108F RID: 4239
		private DirectoryInfo rootDirInt;

		// Token: 0x04001090 RID: 4240
		private ContentSource source;

		// Token: 0x04001091 RID: 4241
		private Texture2D previewImage;

		// Token: 0x04001092 RID: 4242
		private bool previewImageWasLoaded;

		// Token: 0x04001093 RID: 4243
		public bool enabled = true;

		// Token: 0x04001094 RID: 4244
		private ModMetaData.ModMetaDataInternal meta = new ModMetaData.ModMetaDataInternal();

		// Token: 0x04001095 RID: 4245
		public ModLoadFolders loadFolders;

		// Token: 0x04001096 RID: 4246
		private WorkshopItemHook workshopHookInt;

		// Token: 0x04001097 RID: 4247
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x04001098 RID: 4248
		public bool appendPackageIdSteamPostfix;

		// Token: 0x04001099 RID: 4249
		public bool translationMod;

		// Token: 0x0400109A RID: 4250
		private string packageIdLowerCase;

		// Token: 0x0400109B RID: 4251
		private string descriptionCached;

		// Token: 0x0400109C RID: 4252
		private const string AboutFolderName = "About";

		// Token: 0x0400109D RID: 4253
		public static readonly string SteamModPostfix = "_steam";

		// Token: 0x0400109E RID: 4254
		private List<string> unsatisfiedDepsList = new List<string>();

		// Token: 0x02000347 RID: 839
		private class ModMetaDataInternal
		{
			// Token: 0x17000406 RID: 1030
			// (get) Token: 0x0600159F RID: 5535 RVA: 0x000156D4 File Offset: 0x000138D4
			// (set) Token: 0x060015A0 RID: 5536 RVA: 0x000156DC File Offset: 0x000138DC
			public List<System.Version> SupportedVersions { get; private set; }

			// Token: 0x060015A1 RID: 5537 RVA: 0x000D31BC File Offset: 0x000D13BC
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
						}), false);
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
						}), false);
					}
					return false;
				}
				return true;
			}

			// Token: 0x060015A2 RID: 5538 RVA: 0x000D32B8 File Offset: 0x000D14B8
			public bool TryParseSupportedVersions(bool logIssues = true)
			{
				if (this.targetVersion != null && logIssues)
				{
					Log.Warning("Mod " + this.name + ": targetVersion field is obsolete, use supportedVersions instead.", false);
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
						Log.Warning("Mod " + this.name + " is missing supported versions list in About.xml! (example: <supportedVersions><li>1.0</li></supportedVersions>)", false);
					}
					flag = true;
				}
				else if (this.supportedVersions.Count == 0)
				{
					if (logIssues)
					{
						Log.Error("Mod " + this.name + ": <supportedVersions> in mod About.xml must specify at least one version.", false);
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

			// Token: 0x060015A3 RID: 5539 RVA: 0x000D3434 File Offset: 0x000D1634
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
						Log.Warning("Mod " + this.name + " is missing packageId in About.xml! (example: <packageId>AuthorName.ModName.Specific</packageId>)", false);
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
						}), false);
					}
					flag = true;
				}
				if (!isOfficial && this.packageId.ToLower().Contains(ModContentPack.LudeonPackageIdAuthor))
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " <packageId> contains word \"Ludeon\", which is reserved for official content.", false);
					}
					flag = true;
				}
				return !flag;
			}

			// Token: 0x060015A4 RID: 5540 RVA: 0x000D357C File Offset: 0x000D177C
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

			// Token: 0x060015A5 RID: 5541 RVA: 0x000156E5 File Offset: 0x000138E5
			[Obsolete("Only need this overload to not break mod compatibility.")]
			public void ValidateDependencies()
			{
				this.ValidateDependencies_NewTmp(true);
			}

			// Token: 0x060015A6 RID: 5542 RVA: 0x000D35D4 File Offset: 0x000D17D4
			public void ValidateDependencies_NewTmp(bool logIssues = true)
			{
				for (int i = this.modDependencies.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					ModDependency modDependency = this.modDependencies[i];
					if (modDependency.packageId.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with no <packageId> specified.", false);
						}
						flag = true;
					}
					else if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(modDependency.packageId))
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with invalid <packageId>: " + modDependency.packageId, false);
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
							}), false);
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
							}), false);
						}
						flag = true;
					}
					if (flag)
					{
						this.modDependencies.Remove(modDependency);
					}
				}
			}

			// Token: 0x060015A7 RID: 5543 RVA: 0x000D3744 File Offset: 0x000D1944
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

			// Token: 0x040010A1 RID: 4257
			public string packageId = "";

			// Token: 0x040010A2 RID: 4258
			public string name = "";

			// Token: 0x040010A3 RID: 4259
			public string author = "Anonymous";

			// Token: 0x040010A4 RID: 4260
			public string url = "";

			// Token: 0x040010A5 RID: 4261
			public string description = "No description provided.";

			// Token: 0x040010A6 RID: 4262
			public int steamAppId;

			// Token: 0x040010A7 RID: 4263
			public List<string> supportedVersions;

			// Token: 0x040010A8 RID: 4264
			[Unsaved(true)]
			private string targetVersion;

			// Token: 0x040010A9 RID: 4265
			public List<ModDependency> modDependencies = new List<ModDependency>();

			// Token: 0x040010AA RID: 4266
			public List<string> loadBefore = new List<string>();

			// Token: 0x040010AB RID: 4267
			public List<string> loadAfter = new List<string>();

			// Token: 0x040010AC RID: 4268
			public List<string> incompatibleWith = new List<string>();

			// Token: 0x040010AD RID: 4269
			private ModMetaData.VersionedData<string> descriptionsByVersion;

			// Token: 0x040010AE RID: 4270
			private ModMetaData.VersionedData<List<ModDependency>> modDependenciesByVersion;

			// Token: 0x040010AF RID: 4271
			private ModMetaData.VersionedData<List<string>> loadBeforeByVersion;

			// Token: 0x040010B0 RID: 4272
			private ModMetaData.VersionedData<List<string>> loadAfterByVersion;

			// Token: 0x040010B1 RID: 4273
			private ModMetaData.VersionedData<List<string>> incompatibleWithByVersion;

			// Token: 0x040010B2 RID: 4274
			public static readonly Regex PackageIdFormatRegex = new Regex("(?=.{1,60}$)^(?!\\.)(?=.*?[.])(?!.*([.])\\1+)[a-zA-Z0-9.]{1,}[a-zA-Z0-9]{1}$");
		}

		// Token: 0x02000349 RID: 841
		private class VersionedData<T> where T : class
		{
			// Token: 0x060015AF RID: 5551 RVA: 0x000D386C File Offset: 0x000D1A6C
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
							Log.Warning("More than one value for a same version of " + typeof(T).Name + " named " + xmlRoot.Name, false);
						}
					}
				}
			}

			// Token: 0x060015B0 RID: 5552 RVA: 0x000D396C File Offset: 0x000D1B6C
			public T GetItemForVersion(string ver)
			{
				if (this.itemForVersion.ContainsKey(ver))
				{
					return this.itemForVersion[ver];
				}
				return default(T);
			}

			// Token: 0x040010B8 RID: 4280
			private Dictionary<string, T> itemForVersion = new Dictionary<string, T>();
		}
	}
}
