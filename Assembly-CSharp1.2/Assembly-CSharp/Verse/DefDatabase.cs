using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C7 RID: 199
	public static class DefDatabase<T> where T : Def
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x0000B1F5 File Offset: 0x000093F5
		public static IEnumerable<T> AllDefs
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x0000B1F5 File Offset: 0x000093F5
		public static List<T> AllDefsListForReading
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x0000B1FC File Offset: 0x000093FC
		public static int DefCount
		{
			get
			{
				return DefDatabase<T>.defsList.Count;
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0008DA34 File Offset: 0x0008BC34
		public static void AddAllInMods()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModContentPack modContentPack in (from m in LoadedModManager.RunningMods
			orderby m.OverwritePriority
			select m).ThenBy((ModContentPack x) => LoadedModManager.RunningModsListForReading.IndexOf(x)))
			{
				hashSet.Clear();
				foreach (T t in GenDefDatabase.DefsToGoInDatabase<T>(modContentPack))
				{
					if (!hashSet.Add(t.defName))
					{
						Log.Error(string.Concat(new object[]
						{
							"Mod ",
							modContentPack,
							" has multiple ",
							typeof(T),
							"s named ",
							t.defName,
							". Skipping."
						}), false);
					}
					else
					{
						DefDatabase<T>.<AddAllInMods>g__AddDef|8_0(t, modContentPack.ToString());
					}
				}
			}
			foreach (!0 def in LoadedModManager.PatchedDefsForReading.OfType<T>())
			{
				DefDatabase<T>.<AddAllInMods>g__AddDef|8_0(def, "Patches");
			}
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0008DBC4 File Offset: 0x0008BDC4
		public static void Add(IEnumerable<T> defs)
		{
			foreach (!0 def in defs)
			{
				DefDatabase<T>.Add(def);
			}
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0008DC0C File Offset: 0x0008BE0C
		public static void Add(T def)
		{
			while (DefDatabase<T>.defsByName.ContainsKey(def.defName))
			{
				Log.Error(string.Concat(new object[]
				{
					"Adding duplicate ",
					typeof(T),
					" name: ",
					def.defName
				}), false);
				T t = def;
				t.defName += Mathf.RoundToInt(Rand.Value * 1000f);
			}
			DefDatabase<T>.defsList.Add(def);
			DefDatabase<T>.defsByName.Add(def.defName, def);
			if (DefDatabase<T>.defsList.Count > 65535)
			{
				Log.Error(string.Concat(new object[]
				{
					"Too many ",
					typeof(T),
					"; over ",
					ushort.MaxValue
				}), false);
			}
			def.index = (ushort)(DefDatabase<T>.defsList.Count - 1);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0000B208 File Offset: 0x00009408
		private static void Remove(T def)
		{
			DefDatabase<T>.defsByName.Remove(def.defName);
			DefDatabase<T>.defsList.Remove(def);
			DefDatabase<T>.SetIndices();
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0000B231 File Offset: 0x00009431
		public static void Clear()
		{
			DefDatabase<T>.defsList.Clear();
			DefDatabase<T>.defsByName.Clear();
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0008DD20 File Offset: 0x0008BF20
		public static void ClearCachedData()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].ClearCachedData();
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0008DD58 File Offset: 0x0008BF58
		public static void ResolveAllReferences(bool onlyExactlyMyType = true, bool parallel = false)
		{
			DeepProfiler.Start("SetIndices");
			try
			{
				DefDatabase<T>.SetIndices();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ResolveAllReferences " + typeof(T).FullName);
			try
			{
				Action<T> action = delegate(T def)
				{
					if (onlyExactlyMyType && def.GetType() != typeof(T))
					{
						return;
					}
					DeepProfiler.Start("Resolver call");
					try
					{
						def.ResolveReferences();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error while resolving references for def ",
							def,
							": ",
							ex
						}), false);
					}
					finally
					{
						DeepProfiler.End();
					}
				};
				if (parallel)
				{
					GenThreading.ParallelForEach<T>(DefDatabase<T>.defsList, action, -1);
				}
				else
				{
					for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
					{
						action(DefDatabase<T>.defsList[i]);
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("SetIndices");
			try
			{
				DefDatabase<T>.SetIndices();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0008DE34 File Offset: 0x0008C034
		private static void SetIndices()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].index = (ushort)i;
			}
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0008DE70 File Offset: 0x0008C070
		public static void ErrorCheckAllDefs()
		{
			foreach (T t in DefDatabase<T>.AllDefs)
			{
				try
				{
					if (!t.ignoreConfigErrors)
					{
						foreach (string text in t.ConfigErrors())
						{
							Log.Error(string.Concat(new object[]
							{
								"Config error in ",
								t,
								": ",
								text
							}), false);
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception in ConfigErrors() of ",
						t.defName,
						": ",
						ex
					}), false);
				}
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0008DF78 File Offset: 0x0008C178
		public static T GetNamed(string defName, bool errorOnFail = true)
		{
			if (errorOnFail)
			{
				T result;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result))
				{
					return result;
				}
				Log.Error(string.Concat(new object[]
				{
					"Failed to find ",
					typeof(T),
					" named ",
					defName,
					". There are ",
					DefDatabase<T>.defsList.Count,
					" defs of this type loaded."
				}), false);
				return default(T);
			}
			else
			{
				T result2;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result2))
				{
					return result2;
				}
				return default(T);
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0000B247 File Offset: 0x00009447
		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0008E014 File Offset: 0x0008C214
		public static T GetByShortHash(ushort shortHash)
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				if (DefDatabase<T>.defsList[i].shortHash == shortHash)
				{
					return DefDatabase<T>.defsList[i];
				}
			}
			return default(T);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0000B250 File Offset: 0x00009450
		public static T GetRandom()
		{
			return DefDatabase<T>.defsList.RandomElement<T>();
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0008E064 File Offset: 0x0008C264
		[CompilerGenerated]
		internal static void <AddAllInMods>g__AddDef|8_0(T def, string sourceName)
		{
			if (def.defName == "UnnamedDef")
			{
				string text = "Unnamed" + typeof(T).Name + Rand.Range(1, 100000).ToString() + "A";
				Log.Error(string.Concat(new string[]
				{
					typeof(T).Name,
					" in ",
					sourceName,
					" with label ",
					def.label,
					" lacks a defName. Giving name ",
					text
				}), false);
				def.defName = text;
			}
			T def2;
			if (DefDatabase<T>.defsByName.TryGetValue(def.defName, out def2))
			{
				DefDatabase<T>.Remove(def2);
			}
			DefDatabase<T>.Add(def);
		}

		// Token: 0x04000310 RID: 784
		private static List<T> defsList = new List<T>();

		// Token: 0x04000311 RID: 785
		private static Dictionary<string, T> defsByName = new Dictionary<string, T>();
	}
}
