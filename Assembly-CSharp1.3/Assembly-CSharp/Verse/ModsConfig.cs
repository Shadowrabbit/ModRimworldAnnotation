using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000244 RID: 580
	public static class ModsConfig
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x0005E8CC File Offset: 0x0005CACC
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

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060010C1 RID: 4289 RVA: 0x0005E934 File Offset: 0x0005CB34
		public static string LastInstalledExpansionId
		{
			get
			{
				for (int i = ModsConfig.data.knownExpansions.Count - 1; i >= 0; i--)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(ModsConfig.data.knownExpansions[i]);
					if (expansionWithIdentifier != null && !expansionWithIdentifier.isCore && expansionWithIdentifier.Status != ExpansionStatus.NotInstalled)
					{
						return ModsConfig.data.knownExpansions[i];
					}
				}
				return null;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x0005E999 File Offset: 0x0005CB99
		public static bool RoyaltyActive
		{
			get
			{
				return ModsConfig.royaltyActive;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x0005E9A0 File Offset: 0x0005CBA0
		public static bool IdeologyActive
		{
			get
			{
				return ModsConfig.ideologyActive;
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0005E9A8 File Offset: 0x0005CBA8
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
					}));
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
						Log.Warning("There was more than one enabled instance of mod with PackageID: " + modMetaData2.PackageIdNonUnique + ". Disabling the duplicates.");
						continue;
					}
					hashSet.Add(modMetaData2.PackageIdNonUnique);
				}
				if (!modMetaData2.IsCoreMod && modMetaData2.Official && ModsConfig.IsExpansionNew(modMetaData2.PackageId))
				{
					ModsConfig.SetActive(modMetaData2.PackageId, true);
					ModsConfig.AddKnownExpansion(modMetaData2.PackageId);
					int num3 = ModsConfig.data.activeMods.IndexOf(modMetaData2.PackageId);
					if (!modMetaData2.ForceLoadAfter.NullOrEmpty<string>())
					{
						foreach (string identifier in modMetaData2.ForceLoadAfter)
						{
							ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(identifier);
							if (activeModWithIdentifier != null)
							{
								int num4 = ModsConfig.data.activeMods.IndexOf(activeModWithIdentifier.PackageId);
								if (num4 != -1 && num4 > num3)
								{
									string text2;
									ModsConfig.TryReorder(num3, num4, out text2);
									Gen.Swap<int>(ref num4, ref num3);
								}
							}
						}
					}
					if (!modMetaData2.ForceLoadBefore.NullOrEmpty<string>())
					{
						foreach (string identifier2 in modMetaData2.ForceLoadBefore)
						{
							ModMetaData activeModWithIdentifier2 = ModLister.GetActiveModWithIdentifier(identifier2);
							if (activeModWithIdentifier2 != null)
							{
								int num5 = ModsConfig.data.activeMods.IndexOf(activeModWithIdentifier2.PackageId);
								if (num5 != -1 && num5 < num3)
								{
									string text2;
									ModsConfig.TryReorder(num3, num5, out text2);
									Gen.Swap<int>(ref num5, ref num3);
								}
							}
						}
					}
					Prefs.Notify_NewExpansion();
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

		// Token: 0x060010C5 RID: 4293 RVA: 0x0005EDE8 File Offset: 0x0005CFE8
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

		// Token: 0x060010C6 RID: 4294 RVA: 0x0005EE18 File Offset: 0x0005D018
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

		// Token: 0x060010C7 RID: 4295 RVA: 0x0005EEB8 File Offset: 0x0005D0B8
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

		// Token: 0x060010C8 RID: 4296 RVA: 0x0005EF58 File Offset: 0x0005D158
		public static bool TryReorder(int modIndex, int newIndex, out string errorMessage)
		{
			errorMessage = null;
			if (modIndex == newIndex)
			{
				return false;
			}
			if (ModsConfig.ReorderConflict(modIndex, newIndex, out errorMessage))
			{
				return false;
			}
			ModsConfig.data.activeMods.Insert(newIndex, ModsConfig.data.activeMods[modIndex]);
			ModsConfig.data.activeMods.RemoveAt((modIndex < newIndex) ? modIndex : (modIndex + 1));
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
			return true;
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0005EFBC File Offset: 0x0005D1BC
		private static bool ReorderConflict(int modIndex, int newIndex, out string errorMessage)
		{
			errorMessage = null;
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[modIndex], false);
			for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
			{
				if (i != modIndex)
				{
					ModMetaData modWithIdentifier2 = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false);
					if (modWithIdentifier.IsCoreMod && modWithIdentifier2.Source == ContentSource.OfficialModsFolder && i < newIndex)
					{
						errorMessage = "ModReorderConflict_MustLoadBefore".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false).Name);
						return true;
					}
					if (modWithIdentifier.Source == ContentSource.OfficialModsFolder && modWithIdentifier2.IsCoreMod && i >= newIndex)
					{
						errorMessage = "ModReorderConflict_MustLoadAfter".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false).Name);
						return true;
					}
				}
			}
			if (!modWithIdentifier.ForceLoadBefore.NullOrEmpty<string>())
			{
				foreach (string identifier in modWithIdentifier.ForceLoadBefore)
				{
					ModMetaData modWithIdentifier3 = ModLister.GetModWithIdentifier(identifier, false);
					if (modWithIdentifier3 != null)
					{
						for (int j = newIndex - 1; j >= 0; j--)
						{
							if (modWithIdentifier3.SamePackageId(ModsConfig.data.activeMods[j], false))
							{
								errorMessage = "ModReorderConflict_MustLoadBefore".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[j], false).Name);
								return true;
							}
						}
					}
				}
			}
			if (!modWithIdentifier.ForceLoadAfter.NullOrEmpty<string>())
			{
				foreach (string identifier2 in modWithIdentifier.ForceLoadAfter)
				{
					ModMetaData modWithIdentifier4 = ModLister.GetModWithIdentifier(identifier2, false);
					if (modWithIdentifier4 != null)
					{
						for (int k = newIndex; k < ModsConfig.data.activeMods.Count; k++)
						{
							if (modWithIdentifier4.SamePackageId(ModsConfig.data.activeMods[k], false))
							{
								errorMessage = "ModReorderConflict_MustLoadAfter".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[k], false).Name);
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0005F274 File Offset: 0x0005D474
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

		// Token: 0x060010CB RID: 4299 RVA: 0x0005F2E8 File Offset: 0x0005D4E8
		public static bool IsActive(ModMetaData mod)
		{
			return ModsConfig.IsActive(mod.PackageId);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0005F2F5 File Offset: 0x0005D4F5
		public static bool IsActive(string id)
		{
			return ModsConfig.activeModsHashSet.Contains(id.ToLower());
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0005F307 File Offset: 0x0005D507
		public static void SetActive(ModMetaData mod, bool active)
		{
			ModsConfig.SetActive(mod.PackageId, active);
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0005F318 File Offset: 0x0005D518
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

		// Token: 0x060010CF RID: 4303 RVA: 0x0005F37B File Offset: 0x0005D57B
		public static void SetActiveToList(List<string> mods)
		{
			ModsConfig.data.activeMods = (from mod in mods
			where ModLister.GetModWithIdentifier(mod, false) != null
			select mod).ToList<string>();
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0005F3B6 File Offset: 0x0005D5B6
		public static bool IsExpansionNew(string id)
		{
			return !ModsConfig.data.knownExpansions.Contains(id.ToLower());
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0005F3D0 File Offset: 0x0005D5D0
		public static void AddKnownExpansion(string id)
		{
			if (ModsConfig.IsExpansionNew(id))
			{
				ModsConfig.data.knownExpansions.Add(id.ToLower());
			}
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0005F3EF File Offset: 0x0005D5EF
		public static void Save()
		{
			ModsConfig.data.version = VersionControl.CurrentVersionStringWithRev;
			DirectXmlSaver.SaveDataObject(ModsConfig.data, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0005F40F File Offset: 0x0005D60F
		public static void SaveFromList(List<string> mods)
		{
			DirectXmlSaver.SaveDataObject(new ModsConfig.ModsConfigData
			{
				version = VersionControl.CurrentVersionStringWithRev,
				activeMods = mods,
				knownExpansions = ModsConfig.data.knownExpansions
			}, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0005F444 File Offset: 0x0005D644
		public static void RestartFromChangedMods()
		{
			Find.WindowStack.Add(new Dialog_MessageBox("ModsChanged".Translate(), null, delegate()
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null));
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0005F490 File Offset: 0x0005D690
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
					stringBuilder.AppendLine("ModIncompatibleWithTip".Translate(list2.ToCommaList(true, false)));
				}
				List<string> list3 = ModsConfig.FindConflicts(mods, modMetaData.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index);
				if (list3.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadBefore".Translate(list3.ToCommaList(true, false)));
				}
				List<string> list4 = ModsConfig.FindConflicts(mods, modMetaData.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index);
				if (list4.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadAfter".Translate(list4.ToCommaList(true, false)));
				}
				if (modMetaData.Dependencies.Any<ModDependency>())
				{
					List<string> list5 = modMetaData.UnsatisfiedDependencies();
					if (list5.Any<string>())
					{
						stringBuilder.AppendLine("ModUnsatisfiedDependency".Translate(list5.ToCommaList(true, false)));
					}
				}
				list.Add(stringBuilder.ToString().TrimEndNewlines());
			}
			return list;
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0005F708 File Offset: 0x0005D908
		public static bool ModHasAnyOrderingIssues(ModMetaData mod)
		{
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			int index = mods.IndexOf(mod);
			return index != -1 && (ModsConfig.FindConflicts(mods, mod.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index).Count > 0 || ModsConfig.FindConflicts(mods, mod.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index).Count > 0);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0005F798 File Offset: 0x0005D998
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

		// Token: 0x060010D8 RID: 4312 RVA: 0x0005F820 File Offset: 0x0005DA20
		public static void TrySortMods()
		{
			List<ModMetaData> list = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			DirectedAcyclicGraph directedAcyclicGraph = new DirectedAcyclicGraph(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				ModMetaData modMetaData = list[i];
				using (IEnumerator<string> enumerator = modMetaData.LoadBefore.Concat(modMetaData.ForceLoadBefore).GetEnumerator())
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
				using (IEnumerator<string> enumerator = modMetaData.LoadAfter.Concat(modMetaData.ForceLoadAfter).GetEnumerator())
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

		// Token: 0x060010D9 RID: 4313 RVA: 0x0005F9AC File Offset: 0x0005DBAC
		private static void RecacheActiveMods()
		{
			ModsConfig.activeModsHashSet.Clear();
			foreach (string item in ModsConfig.data.activeMods)
			{
				ModsConfig.activeModsHashSet.Add(item);
			}
			ModsConfig.royaltyActive = ModsConfig.IsActive(ModContentPack.RoyaltyModPackageId);
			ModsConfig.ideologyActive = ModsConfig.IsActive(ModContentPack.IdeologyModPackageId);
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		// Token: 0x04000CCF RID: 3279
		private static ModsConfig.ModsConfigData data;

		// Token: 0x04000CD0 RID: 3280
		private static bool royaltyActive;

		// Token: 0x04000CD1 RID: 3281
		private static bool ideologyActive;

		// Token: 0x04000CD2 RID: 3282
		private static HashSet<string> activeModsHashSet = new HashSet<string>();

		// Token: 0x04000CD3 RID: 3283
		private static List<ModMetaData> activeModsInLoadOrderCached = new List<ModMetaData>();

		// Token: 0x04000CD4 RID: 3284
		private static bool activeModsInLoadOrderCachedDirty;

		// Token: 0x020019BF RID: 6591
		private class ModsConfigData
		{
			// Token: 0x040062DF RID: 25311
			[LoadAlias("buildNumber")]
			public string version;

			// Token: 0x040062E0 RID: 25312
			public List<string> activeMods = new List<string>();

			// Token: 0x040062E1 RID: 25313
			public List<string> knownExpansions = new List<string>();
		}
	}
}
