using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200007A RID: 122
	public static class DefDatabase<T> where T : Def
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x000182CD File Offset: 0x000164CD
		public static IEnumerable<T> AllDefs
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x000182CD File Offset: 0x000164CD
		public static List<T> AllDefsListForReading
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x000182D4 File Offset: 0x000164D4
		public static int DefCount
		{
			get
			{
				return DefDatabase<T>.defsList.Count;
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000182E0 File Offset: 0x000164E0
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
						}));
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

		// Token: 0x06000497 RID: 1175 RVA: 0x00018470 File Offset: 0x00016670
		public static void Add(IEnumerable<T> defs)
		{
			foreach (!0 def in defs)
			{
				DefDatabase<T>.Add(def);
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000184B8 File Offset: 0x000166B8
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
				}));
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
				}));
			}
			def.index = (ushort)(DefDatabase<T>.defsList.Count - 1);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000185C8 File Offset: 0x000167C8
		private static void Remove(T def)
		{
			DefDatabase<T>.defsByName.Remove(def.defName);
			DefDatabase<T>.defsList.Remove(def);
			DefDatabase<T>.SetIndices();
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x000185F1 File Offset: 0x000167F1
		public static void Clear()
		{
			DefDatabase<T>.defsList.Clear();
			DefDatabase<T>.defsByName.Clear();
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00018608 File Offset: 0x00016808
		public static void ClearCachedData()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].ClearCachedData();
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00018640 File Offset: 0x00016840
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
						}));
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

		// Token: 0x0600049D RID: 1181 RVA: 0x0001871C File Offset: 0x0001691C
		private static void SetIndices()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].index = (ushort)i;
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00018758 File Offset: 0x00016958
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
							}));
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
					}));
				}
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001885C File Offset: 0x00016A5C
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
				}));
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

		// Token: 0x060004A0 RID: 1184 RVA: 0x000188F4 File Offset: 0x00016AF4
		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00018900 File Offset: 0x00016B00
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

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001894F File Offset: 0x00016B4F
		public static T GetRandom()
		{
			return DefDatabase<T>.defsList.RandomElement<T>();
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00018974 File Offset: 0x00016B74
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
				}));
				def.defName = text;
			}
			T def2;
			if (DefDatabase<T>.defsByName.TryGetValue(def.defName, out def2))
			{
				DefDatabase<T>.Remove(def2);
			}
			DefDatabase<T>.Add(def);
		}

		// Token: 0x04000196 RID: 406
		private static List<T> defsList = new List<T>();

		// Token: 0x04000197 RID: 407
		private static Dictionary<string, T> defsByName = new Dictionary<string, T>();
	}
}
