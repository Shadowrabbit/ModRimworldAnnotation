using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000343 RID: 835
	public static class ModLister
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x000152FF File Offset: 0x000134FF
		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x000D2620 File Offset: 0x000D0820
		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x000D267C File Offset: 0x000D087C
		public static List<ExpansionDef> AllExpansions
		{
			get
			{
				if (ModLister.AllExpansionsCached.NullOrEmpty<ExpansionDef>())
				{
					ModLister.AllExpansionsCached = DefDatabase<ExpansionDef>.AllDefsListForReading.Where(delegate(ExpansionDef e)
					{
						ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(e.linkedMod, false);
						return modWithIdentifier == null || modWithIdentifier.Official;
					}).ToList<ExpansionDef>();
				}
				return ModLister.AllExpansionsCached;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x00015306 File Offset: 0x00013506
		public static bool RoyaltyInstalled
		{
			get
			{
				return ModLister.royaltyInstalled && !Prefs.SimulateNotOwningRoyalty;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x00015319 File Offset: 0x00013519
		public static bool ShouldLogIssues
		{
			get
			{
				return !ModLister.modListBuilt && !ModLister.nestedRebuildInProgress;
			}
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0001532C File Offset: 0x0001352C
		static ModLister()
		{
			ModLister.RebuildModList();
			ModLister.modListBuilt = true;
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00006A05 File Offset: 0x00004C05
		public static void EnsureInit()
		{
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x000D26D0 File Offset: 0x000D08D0
		public static void RebuildModList()
		{
			ModLister.nestedRebuildInProgress = ModLister.rebuildingModList;
			ModLister.rebuildingModList = true;
			string s = "Rebuilding mods list";
			ModLister.mods.Clear();
			WorkshopItems.EnsureInit();
			s += "\nAdding official mods from content folder:";
			foreach (string localAbsPath in from d in new DirectoryInfo(GenFilePaths.OfficialModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData = new ModMetaData(localAbsPath, true);
				if (ModLister.TryAddMod(modMetaData))
				{
					s = s + "\n  Adding " + modMetaData.ToStringLong();
				}
			}
			s += "\nAdding mods from mods folder:";
			foreach (string localAbsPath2 in from d in new DirectoryInfo(GenFilePaths.ModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData2 = new ModMetaData(localAbsPath2, false);
				if (ModLister.TryAddMod(modMetaData2))
				{
					s = s + "\n  Adding " + modMetaData2.ToStringLong();
				}
			}
			s += "\nAdding mods from Steam:";
			foreach (WorkshopItem workshopItem in from it in WorkshopItems.AllSubscribedItems
			where it is WorkshopItem_Mod
			select it)
			{
				ModMetaData modMetaData3 = new ModMetaData(workshopItem);
				if (ModLister.TryAddMod(modMetaData3))
				{
					s = s + "\n  Adding " + modMetaData3.ToStringLong();
				}
			}
			s += "\nDeactivating not-installed mods:";
			ModsConfig.DeactivateNotInstalledMods(delegate(string log)
			{
				s = s + "\n   " + log;
			});
			if (Prefs.SimulateNotOwningRoyalty)
			{
				ModsConfig.SetActive(ModContentPack.RoyaltyModPackageId, false);
			}
			if (ModLister.mods.Count((ModMetaData m) => m.Active) == 0)
			{
				s += "\nThere are no active mods. Activating Core mod.";
				ModLister.mods.First((ModMetaData m) => m.IsCoreMod).Active = true;
			}
			ModLister.RecacheRoyaltyInstalled();
			if (Prefs.LogVerbose)
			{
				Log.Message(s, false);
			}
			ModLister.rebuildingModList = false;
			ModLister.nestedRebuildInProgress = false;
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x000D29C4 File Offset: 0x000D0BC4
		public static int InstalledModsListHash(bool activeOnly)
		{
			int num = 17;
			int num2 = 0;
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				if (!activeOnly || ModsConfig.IsActive(modMetaData.PackageId))
				{
					num = num * 31 + modMetaData.GetHashCode();
					num = num * 31 + num2 * 2654241;
					num2++;
				}
			}
			return num;
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x000D2A3C File Offset: 0x000D0C3C
		public static ModMetaData GetModWithIdentifier(string identifier, bool ignorePostfix = false)
		{
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].SamePackageId(identifier, ignorePostfix))
				{
					return ModLister.mods[i];
				}
			}
			return null;
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x000D2A80 File Offset: 0x000D0C80
		public static ModMetaData GetActiveModWithIdentifier(string identifier)
		{
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].SamePackageId(identifier, true) && ModLister.mods[i].Active)
				{
					return ModLister.mods[i];
				}
			}
			return null;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x000D2AD8 File Offset: 0x000D0CD8
		public static ExpansionDef GetExpansionWithIdentifier(string packageId)
		{
			for (int i = 0; i < ModLister.AllExpansions.Count; i++)
			{
				if (ModLister.AllExpansions[i].linkedMod == packageId)
				{
					return ModLister.AllExpansions[i];
				}
			}
			return null;
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x000D2B20 File Offset: 0x000D0D20
		public static bool HasActiveModWithName(string name)
		{
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].Active && ModLister.mods[i].Name == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x000D2B70 File Offset: 0x000D0D70
		public static bool AnyFromListActive(List<string> mods)
		{
			using (List<string>.Enumerator enumerator = mods.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (ModLister.GetActiveModWithIdentifier(enumerator.Current) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x000D2BC4 File Offset: 0x000D0DC4
		private static void RecacheRoyaltyInstalled()
		{
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].SamePackageId(ModContentPack.RoyaltyModPackageId, false))
				{
					ModLister.royaltyInstalled = true;
					return;
				}
			}
			ModLister.royaltyInstalled = false;
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x000D2C0C File Offset: 0x000D0E0C
		private static bool TryAddMod(ModMetaData mod)
		{
			if (mod.Official && !mod.IsCoreMod && SteamManager.Initialized && mod.SteamAppId != 0)
			{
				bool flag = true;
				try
				{
					flag = SteamApps.BIsDlcInstalled(new AppId_t((uint)mod.SteamAppId));
				}
				catch (Exception arg)
				{
					Log.Error("Could not determine if a DLC is installed: " + arg, false);
				}
				if (!flag)
				{
					return false;
				}
			}
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(mod.PackageId, false);
			if (modWithIdentifier == null)
			{
				ModLister.mods.Add(mod);
				return true;
			}
			if (mod.RootDir.FullName != modWithIdentifier.RootDir.FullName)
			{
				if (mod.OnSteamWorkshop != modWithIdentifier.OnSteamWorkshop)
				{
					ModMetaData modMetaData = mod.OnSteamWorkshop ? mod : modWithIdentifier;
					if (!modMetaData.appendPackageIdSteamPostfix)
					{
						modMetaData.appendPackageIdSteamPostfix = true;
						return ModLister.TryAddMod(mod);
					}
				}
				Log.Error(string.Concat(new string[]
				{
					"Tried loading mod with the same packageId multiple times: ",
					mod.PackageIdPlayerFacing,
					". Ignoring the duplicates.\n",
					mod.RootDir.FullName,
					"\n",
					modWithIdentifier.RootDir.FullName
				}), false);
				return false;
			}
			return false;
		}

		// Token: 0x0400107F RID: 4223
		private static List<ModMetaData> mods = new List<ModMetaData>();

		// Token: 0x04001080 RID: 4224
		private static bool royaltyInstalled;

		// Token: 0x04001081 RID: 4225
		private static bool modListBuilt;

		// Token: 0x04001082 RID: 4226
		private static bool rebuildingModList;

		// Token: 0x04001083 RID: 4227
		private static bool nestedRebuildInProgress;

		// Token: 0x04001084 RID: 4228
		private static List<ExpansionDef> AllExpansionsCached;
	}
}
