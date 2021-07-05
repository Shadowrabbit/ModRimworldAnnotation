using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200023C RID: 572
	public static class ModLister
	{
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x0005CFCC File Offset: 0x0005B1CC
		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x0005CFD4 File Offset: 0x0005B1D4
		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x0005D030 File Offset: 0x0005B230
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

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x0005D081 File Offset: 0x0005B281
		public static bool RoyaltyInstalled
		{
			get
			{
				return ModLister.royaltyInstalled && !Prefs.SimulateNotOwningRoyalty;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x0005D094 File Offset: 0x0005B294
		public static bool IdeologyInstalled
		{
			get
			{
				return ModLister.ideologyInstalled && !Prefs.SimulateNotOwningIdology;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x0005D0A7 File Offset: 0x0005B2A7
		public static bool ShouldLogIssues
		{
			get
			{
				return !ModLister.modListBuilt && !ModLister.nestedRebuildInProgress;
			}
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0005D0BA File Offset: 0x0005B2BA
		static ModLister()
		{
			ModLister.RebuildModList();
			ModLister.modListBuilt = true;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x0000313F File Offset: 0x0000133F
		public static void EnsureInit()
		{
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0005D0D4 File Offset: 0x0005B2D4
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
			if (Prefs.SimulateNotOwningIdology)
			{
				ModsConfig.SetActive(ModContentPack.IdeologyModPackageId, false);
			}
			if (ModLister.mods.Count((ModMetaData m) => m.Active) == 0)
			{
				s += "\nThere are no active mods. Activating Core mod.";
				ModLister.mods.First((ModMetaData m) => m.IsCoreMod).Active = true;
			}
			ModLister.RecacheExpansionsInstalled();
			if (Prefs.LogVerbose)
			{
				Log.Message(s);
			}
			ModLister.rebuildingModList = false;
			ModLister.nestedRebuildInProgress = false;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0005D3D8 File Offset: 0x0005B5D8
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

		// Token: 0x06001058 RID: 4184 RVA: 0x0005D450 File Offset: 0x0005B650
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

		// Token: 0x06001059 RID: 4185 RVA: 0x0005D494 File Offset: 0x0005B694
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

		// Token: 0x0600105A RID: 4186 RVA: 0x0005D4EC File Offset: 0x0005B6EC
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

		// Token: 0x0600105B RID: 4187 RVA: 0x0005D534 File Offset: 0x0005B734
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

		// Token: 0x0600105C RID: 4188 RVA: 0x0005D584 File Offset: 0x0005B784
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

		// Token: 0x0600105D RID: 4189 RVA: 0x0005D5D8 File Offset: 0x0005B7D8
		private static void RecacheExpansionsInstalled()
		{
			ModLister.royaltyInstalled = false;
			ModLister.ideologyInstalled = false;
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].SamePackageId(ModContentPack.RoyaltyModPackageId, false))
				{
					ModLister.royaltyInstalled = true;
				}
				else if (ModLister.mods[i].SamePackageId(ModContentPack.IdeologyModPackageId, false))
				{
					ModLister.ideologyInstalled = true;
				}
			}
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0005D644 File Offset: 0x0005B844
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
					Log.Error("Could not determine if a DLC is installed: " + arg);
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
				}));
				return false;
			}
			return false;
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0005D770 File Offset: 0x0005B970
		private static bool CheckDLC(bool dlc, string featureName, string dlcNameIndef, string installedPropertyName)
		{
			if (!dlc)
			{
				Log.ErrorOnce(string.Format("{0} is {1}-specific game system. If you want to use this code please check ModLister.{2} before calling it.", featureName, dlcNameIndef, installedPropertyName), featureName.GetHashCode());
			}
			return dlc;
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0005D78E File Offset: 0x0005B98E
		public static bool CheckRoyalty(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled, featureNameSingular, "a Royalty", "RoyaltyInstalled");
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0005D7A5 File Offset: 0x0005B9A5
		public static bool CheckIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.IdeologyInstalled, featureNameSingular, "an Ideology", "IdeologyInstalled");
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0005D7BC File Offset: 0x0005B9BC
		public static bool CheckRoyaltyAndIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled && ModLister.IdeologyInstalled, featureNameSingular, "a Royalty and Ideology", "RoyaltyInstalled and IdeologyInstalled");
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0005D7DD File Offset: 0x0005B9DD
		public static bool CheckRoyaltyOrIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.IdeologyInstalled, featureNameSingular, "a Royalty or Ideology", "RoyaltyInstalled or IdeologyInstalled");
		}

		// Token: 0x04000CA6 RID: 3238
		private static List<ModMetaData> mods = new List<ModMetaData>();

		// Token: 0x04000CA7 RID: 3239
		private static bool modListBuilt;

		// Token: 0x04000CA8 RID: 3240
		private static bool rebuildingModList;

		// Token: 0x04000CA9 RID: 3241
		private static bool nestedRebuildInProgress;

		// Token: 0x04000CAA RID: 3242
		private static List<ExpansionDef> AllExpansionsCached;

		// Token: 0x04000CAB RID: 3243
		private static bool royaltyInstalled;

		// Token: 0x04000CAC RID: 3244
		private static bool ideologyInstalled;
	}
}
