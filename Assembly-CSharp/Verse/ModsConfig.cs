using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000352 RID: 850
	public static class ModsConfig
	{
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x000D42B0 File Offset: 0x000D24B0
		public static IEnumerable<ModMetaData> ActiveModsInLoadOrder
		{
			get
			{
				ModLister.EnsureInit();
				if (ModsConfig.activeModsInLoadOrderCachedDirty)
				{
					ModsConfig.activeModsInLoadOrderCached.Clear();
					for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
					{
						ModsConfig.activeModsInLoadOrderCached.Add(ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false));
					}
					ModsConfig.activeModsInLoadOrderCachedDirty = false;
				}
				return ModsConfig.activeModsInLoadOrderCached;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x00015915 File Offset: 0x00013B15
		public static bool RoyaltyActive
		{
			get
			{
				return ModsConfig.royaltyActive;
			}
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x000D4318 File Offset: 0x000D2518
		static ModsConfig()
		{
			bool flag = false;
			bool flag2 = false;
			ModsConfig.data = DirectXmlLoader.ItemFromXmlFile<ModsConfig.ModsConfigData>(GenFilePaths.ModsConfigFilePath, true);
			if (ModsConfig.data.version != null)
			{
				bool flag3 = false;
				int num2;
				if (ModsConfig.data.version.Contains("."))
				{
					int num = VersionControl.MinorFromVersionString(ModsConfig.data.version);
					if (VersionControl.MajorFromVersionString(ModsConfig.data.version) != VersionControl.CurrentMajor || num != VersionControl.CurrentMinor)
					{
						flag3 = true;
					}
				}
				else if (ModsConfig.data.version.Length > 0 && ModsConfig.data.version.All((char x) => char.IsNumber(x)) && int.TryParse(ModsConfig.data.version, out num2) && num2 <= 2009)
				{
					flag3 = true;
				}
				if (flag3)
				{
					Log.Message(string.Concat(new string[]
					{
						"Mods config data is from version ",
						ModsConfig.data.version,
						" while we are running ",
						VersionControl.CurrentVersionStringWithRev,
						". Resetting."
					}), false);
					ModsConfig.data = new ModsConfig.ModsConfigData();
					flag = true;
				}
			}
			for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
			{
				string packageId = ModsConfig.data.activeMods[i];
				if (ModLister.GetModWithIdentifier(packageId, false) == null)
				{
					ModMetaData modMetaData = ModLister.AllInstalledMods.FirstOrDefault((ModMetaData m) => m.FolderName == packageId);
					if (modMetaData != null)
					{
						ModsConfig.data.activeMods[i] = modMetaData.PackageId;
						flag2 = true;
					}
					string text;
					if (ModsConfig.TryGetPackageIdWithoutExtraSteamPostfix(packageId, out text) && ModLister.GetModWithIdentifier(text, false) != null)
					{
						ModsConfig.data.activeMods[i] = text;
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModMetaData modMetaData2 in ModLister.AllInstalledMods)
			{
				if (modMetaData2.Active)
				{
					if (hashSet.Contains(modMetaData2.PackageIdNonUnique))
					{
						modMetaData2.Active = false;
						Log.Warning("There was more than one enabled instance of mod with PackageID: " + modMetaData2.PackageIdNonUnique + ". Disabling the duplicates.", false);
						continue;
					}
					hashSet.Add(modMetaData2.PackageIdNonUnique);
				}
				if (!modMetaData2.IsCoreMod && modMetaData2.Official && ModsConfig.IsExpansionNew(modMetaData2.PackageId))
				{
					ModsConfig.SetActive(modMetaData2.PackageId, true);
					ModsConfig.AddKnownExpansion(modMetaData2.PackageId);
					flag2 = true;
				}
			}
			if (!File.Exists(GenFilePaths.ModsConfigFilePath) || flag)
			{
				ModsConfig.Reset();
			}
			else if (flag2)
			{
				ModsConfig.Save();
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0001591C File Offset: 0x00013B1C
		public static bool TryGetPackageIdWithoutExtraSteamPostfix(string packageId, out string nonSteamPackageId)
		{
			if (packageId.EndsWith(ModMetaData.SteamModPostfix))
			{
				nonSteamPackageId = packageId.Substring(0, packageId.Length - ModMetaData.SteamModPostfix.Length);
				return true;
			}
			nonSteamPackageId = null;
			return false;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x000D45F4 File Offset: 0x000D27F4
		public static void DeactivateNotInstalledMods(Action<string> logCallback = null)
		{
			for (int i = ModsConfig.data.activeMods.Count - 1; i >= 0; i--)
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false);
				string identifier;
				if (modWithIdentifier == null && ModsConfig.TryGetPackageIdWithoutExtraSteamPostfix(ModsConfig.data.activeMods[i], out identifier))
				{
					modWithIdentifier = ModLister.GetModWithIdentifier(identifier, false);
				}
				if (modWithIdentifier == null)
				{
					if (logCallback != null)
					{
						logCallback("Deactivating " + ModsConfig.data.activeMods[i]);
					}
					ModsConfig.data.activeMods.RemoveAt(i);
				}
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x000D4694 File Offset: 0x000D2894
		public static void Reset()
		{
			ModsConfig.data.activeMods.Clear();
			ModsConfig.data.activeMods.Add(ModContentPack.CoreModPackageId);
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				if (modMetaData.Official && !modMetaData.IsCoreMod && modMetaData.VersionCompatible)
				{
					ModsConfig.data.activeMods.Add(modMetaData.PackageId);
				}
			}
			ModsConfig.Save();
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x000D4734 File Offset: 0x000D2934
		public static void Reorder(int modIndex, int newIndex)
		{
			if (modIndex == newIndex)
			{
				return;
			}
			if (ModsConfig.ReorderConflict(modIndex, newIndex))
			{
				return;
			}
			ModsConfig.data.activeMods.Insert(newIndex, ModsConfig.data.activeMods[modIndex]);
			ModsConfig.data.activeMods.RemoveAt((modIndex < newIndex) ? modIndex : (modIndex + 1));
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x000D4790 File Offset: 0x000D2990
		private static bool ReorderConflict(int modIndex, int newIndex)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[modIndex], false);
			for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
			{
				if (i != modIndex)
				{
					ModMetaData modWithIdentifier2 = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false);
					if (modWithIdentifier.IsCoreMod && modWithIdentifier2.Source == ContentSource.OfficialModsFolder && i < newIndex)
					{
						return true;
					}
					if (modWithIdentifier.Source == ContentSource.OfficialModsFolder && modWithIdentifier2.IsCoreMod)
					{
						return i >= newIndex;
					}
				}
			}
			return false;
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x000D481C File Offset: 0x000D2A1C
		public static void Reorder(List<int> newIndices)
		{
			List<string> list = new List<string>();
			foreach (int index in newIndices)
			{
				list.Add(ModsConfig.data.activeMods[index]);
			}
			ModsConfig.data.activeMods = list;
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x0001594B File Offset: 0x00013B4B
		public static bool IsActive(ModMetaData mod)
		{
			return ModsConfig.IsActive(mod.PackageId);
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00015958 File Offset: 0x00013B58
		public static bool IsActive(string id)
		{
			return ModsConfig.activeModsHashSet.Contains(id.ToLower());
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0001596A File Offset: 0x00013B6A
		public static void SetActive(ModMetaData mod, bool active)
		{
			ModsConfig.SetActive(mod.PackageId, active);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x000D4890 File Offset: 0x000D2A90
		public static void SetActive(string modIdentifier, bool active)
		{
			string item = modIdentifier.ToLower();
			if (active)
			{
				if (!ModsConfig.data.activeMods.Contains(item))
				{
					ModsConfig.data.activeMods.Add(item);
				}
			}
			else if (ModsConfig.data.activeMods.Contains(item))
			{
				ModsConfig.data.activeMods.Remove(item);
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00015978 File Offset: 0x00013B78
		public static void SetActiveToList(List<string> mods)
		{
			ModsConfig.data.activeMods = (from mod in mods
			where ModLister.GetModWithIdentifier(mod, false) != null
			select mod).ToList<string>();
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x000159B3 File Offset: 0x00013BB3
		public static bool IsExpansionNew(string id)
		{
			return !ModsConfig.data.knownExpansions.Contains(id.ToLower());
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x000159CD File Offset: 0x00013BCD
		public static void AddKnownExpansion(string id)
		{
			if (ModsConfig.IsExpansionNew(id))
			{
				ModsConfig.data.knownExpansions.Add(id.ToLower());
			}
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x000159EC File Offset: 0x00013BEC
		public static void Save()
		{
			ModsConfig.data.version = VersionControl.CurrentVersionStringWithRev;
			DirectXmlSaver.SaveDataObject(ModsConfig.data, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x00015A0C File Offset: 0x00013C0C
		public static void SaveFromList(List<string> mods)
		{
			DirectXmlSaver.SaveDataObject(new ModsConfig.ModsConfigData
			{
				version = VersionControl.CurrentVersionStringWithRev,
				activeMods = mods,
				knownExpansions = ModsConfig.data.knownExpansions
			}, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x000D48F4 File Offset: 0x000D2AF4
		public static void RestartFromChangedMods()
		{
			Find.WindowStack.Add(new Dialog_MessageBox("ModsChanged".Translate(), null, delegate()
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null));
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x000D4940 File Offset: 0x000D2B40
		public static List<string> GetModWarnings()
		{
			List<string> list = new List<string>();
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			for (int i = 0; i < mods.Count; i++)
			{
				int index = i;
				ModMetaData modMetaData = mods[index];
				StringBuilder stringBuilder = new StringBuilder("");
				for (int j = 0; j < mods.Count; j++)
				{
					if (i != j && mods[j].PackageId != "" && mods[j].SamePackageId(mods[i].PackageId, false))
					{
						stringBuilder.AppendLine("ModWithSameIdAlreadyActive".Translate(mods[j].Name));
					}
				}
				List<string> list2 = ModsConfig.FindConflicts(mods, modMetaData.IncompatibleWith, null);
				if (list2.Any<string>())
				{
					stringBuilder.AppendLine("ModIncompatibleWithTip".Translate(list2.ToCommaList(true)));
				}
				List<string> list3 = ModsConfig.FindConflicts(mods, modMetaData.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index);
				if (list3.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadBefore".Translate(list3.ToCommaList(true)));
				}
				List<string> list4 = ModsConfig.FindConflicts(mods, modMetaData.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index);
				if (list4.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadAfter".Translate(list4.ToCommaList(true)));
				}
				if (modMetaData.Dependencies.Any<ModDependency>())
				{
					List<string> list5 = modMetaData.UnsatisfiedDependencies();
					if (list5.Any<string>())
					{
						stringBuilder.AppendLine("ModUnsatisfiedDependency".Translate(list5.ToCommaList(true)));
					}
				}
				list.Add(stringBuilder.ToString().TrimEndNewlines());
			}
			return list;
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x000D4BB4 File Offset: 0x000D2DB4
		public static bool ModHasAnyOrderingIssues(ModMetaData mod)
		{
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			int index = mods.IndexOf(mod);
			return index != -1 && (ModsConfig.FindConflicts(mods, mod.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index).Count > 0 || ModsConfig.FindConflicts(mods, mod.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index).Count > 0);
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x000D4C44 File Offset: 0x000D2E44
		private static List<string> FindConflicts(List<ModMetaData> allMods, List<string> modsToCheck, Func<ModMetaData, bool> predicate)
		{
			List<string> list = new List<string>();
			using (List<string>.Enumerator enumerator = modsToCheck.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string modId = enumerator.Current;
					ModMetaData modMetaData = allMods.FirstOrDefault((ModMetaData m) => m.SamePackageId(modId, true));
					if (modMetaData != null && (predicate == null || predicate(modMetaData)))
					{
						list.Add(modMetaData.Name);
					}
				}
			}
			return list;
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x000D4CCC File Offset: 0x000D2ECC
		public static void TrySortMods()
		{
			List<ModMetaData> list = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			DirectedAcyclicGraph directedAcyclicGraph = new DirectedAcyclicGraph(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				ModMetaData modMetaData = list[i];
				using (List<string>.Enumerator enumerator = modMetaData.LoadBefore.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string before = enumerator.Current;
						ModMetaData modMetaData2 = list.FirstOrDefault((ModMetaData m) => m.SamePackageId(before, true));
						if (modMetaData2 != null)
						{
							directedAcyclicGraph.AddEdge(list.IndexOf(modMetaData2), i);
						}
					}
				}
				using (List<string>.Enumerator enumerator = modMetaData.LoadAfter.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string after = enumerator.Current;
						ModMetaData modMetaData3 = list.FirstOrDefault((ModMetaData m) => m.SamePackageId(after, true));
						if (modMetaData3 != null)
						{
							directedAcyclicGraph.AddEdge(i, list.IndexOf(modMetaData3));
						}
					}
				}
			}
			int num = directedAcyclicGraph.FindCycle();
			if (num != -1)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("ModCyclicDependency".Translate(list[num].Name), null, null, null, null, null, false, null, null));
				return;
			}
			ModsConfig.Reorder(directedAcyclicGraph.TopologicalSort());
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x000D4E44 File Offset: 0x000D3044
		private static void RecacheActiveMods()
		{
			ModsConfig.activeModsHashSet.Clear();
			foreach (string item in ModsConfig.data.activeMods)
			{
				ModsConfig.activeModsHashSet.Add(item);
			}
			ModsConfig.royaltyActive = ModsConfig.IsActive(ModContentPack.RoyaltyModPackageId);
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		// Token: 0x040010D5 RID: 4309
		private static ModsConfig.ModsConfigData data;

		// Token: 0x040010D6 RID: 4310
		private static bool royaltyActive;

		// Token: 0x040010D7 RID: 4311
		private static HashSet<string> activeModsHashSet = new HashSet<string>();

		// Token: 0x040010D8 RID: 4312
		private static List<ModMetaData> activeModsInLoadOrderCached = new List<ModMetaData>();

		// Token: 0x040010D9 RID: 4313
		private static bool activeModsInLoadOrderCachedDirty;

		// Token: 0x02000353 RID: 851
		private class ModsConfigData
		{
			// Token: 0x040010DA RID: 4314
			[LoadAlias("buildNumber")]
			public string version;

			// Token: 0x040010DB RID: 4315
			public List<string> activeMods = new List<string>();

			// Token: 0x040010DC RID: 4316
			public List<string> knownExpansions = new List<string>();
		}
	}
}
