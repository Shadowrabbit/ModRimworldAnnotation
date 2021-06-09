using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B8 RID: 5304
	public static class BackstoryDatabase
	{
		// Token: 0x06007237 RID: 29239 RVA: 0x0004CCCF File Offset: 0x0004AECF
		public static void Clear()
		{
			BackstoryDatabase.allBackstories.Clear();
		}

		// Token: 0x06007238 RID: 29240 RVA: 0x0022EF7C File Offset: 0x0022D17C
		public static void ReloadAllBackstories()
		{
			foreach (Backstory backstory in DirectXmlLoader.LoadXmlDataInResourcesFolder<Backstory>("Backstories/Shuffled"))
			{
				DeepProfiler.Start("Backstory.PostLoad");
				try
				{
					backstory.PostLoad();
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Backstory.ResolveReferences");
				try
				{
					backstory.ResolveReferences();
				}
				finally
				{
					DeepProfiler.End();
				}
				foreach (string str in backstory.ConfigErrors(false))
				{
					Log.Error(backstory.title + ": " + str, false);
				}
				DeepProfiler.Start("AddBackstory");
				try
				{
					BackstoryDatabase.AddBackstory(backstory);
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			SolidBioDatabase.LoadAllBios();
		}

		// Token: 0x06007239 RID: 29241 RVA: 0x0022F08C File Offset: 0x0022D28C
		public static void AddBackstory(Backstory bs)
		{
			if (!BackstoryDatabase.allBackstories.ContainsKey(bs.identifier))
			{
				BackstoryDatabase.allBackstories.Add(bs.identifier, bs);
				BackstoryDatabase.shuffleableBackstoryList.Clear();
				return;
			}
			if (bs == BackstoryDatabase.allBackstories[bs.identifier])
			{
				Log.Error("Tried to add the same backstory twice " + bs.identifier, false);
				return;
			}
			Log.Error(string.Concat(new string[]
			{
				"Backstory ",
				bs.title,
				" has same unique save key ",
				bs.identifier,
				" as old backstory ",
				BackstoryDatabase.allBackstories[bs.identifier].title
			}), false);
		}

		// Token: 0x0600723A RID: 29242 RVA: 0x0004CCDB File Offset: 0x0004AEDB
		public static bool TryGetWithIdentifier(string identifier, out Backstory bs, bool closestMatchWarning = true)
		{
			identifier = BackstoryDatabase.GetIdentifierClosestMatch(identifier, closestMatchWarning);
			return BackstoryDatabase.allBackstories.TryGetValue(identifier, out bs);
		}

		// Token: 0x0600723B RID: 29243 RVA: 0x0022F144 File Offset: 0x0022D344
		public static string GetIdentifierClosestMatch(string identifier, bool closestMatchWarning = true)
		{
			if (BackstoryDatabase.allBackstories.ContainsKey(identifier))
			{
				return identifier;
			}
			string b = BackstoryDatabase.StripNumericSuffix(identifier);
			foreach (KeyValuePair<string, Backstory> keyValuePair in BackstoryDatabase.allBackstories)
			{
				Backstory value = keyValuePair.Value;
				if (BackstoryDatabase.StripNumericSuffix(value.identifier) == b)
				{
					if (closestMatchWarning)
					{
						Log.Warning("Couldn't find exact match for backstory " + identifier + ", using closest match " + value.identifier, false);
					}
					return value.identifier;
				}
			}
			Log.Warning("Couldn't find exact match for backstory " + identifier + ", or any close match.", false);
			return identifier;
		}

		// Token: 0x0600723C RID: 29244 RVA: 0x0022F204 File Offset: 0x0022D404
		public static Backstory RandomBackstory(BackstorySlot slot)
		{
			return (from bs in BackstoryDatabase.allBackstories
			where bs.Value.slot == slot
			select bs).RandomElement<KeyValuePair<string, Backstory>>().Value;
		}

		// Token: 0x0600723D RID: 29245 RVA: 0x0022F244 File Offset: 0x0022D444
		public static List<Backstory> ShuffleableBackstoryList(BackstorySlot slot, BackstoryCategoryFilter group)
		{
			Pair<BackstorySlot, BackstoryCategoryFilter> key = new Pair<BackstorySlot, BackstoryCategoryFilter>(slot, group);
			if (!BackstoryDatabase.shuffleableBackstoryList.ContainsKey(key))
			{
				BackstoryDatabase.shuffleableBackstoryList[key] = (from bs in BackstoryDatabase.allBackstories.Values
				where bs.shuffleable && bs.slot == slot && @group.Matches(bs)
				select bs).ToList<Backstory>();
			}
			return BackstoryDatabase.shuffleableBackstoryList[key];
		}

		// Token: 0x0600723E RID: 29246 RVA: 0x0004CCF2 File Offset: 0x0004AEF2
		public static string StripNumericSuffix(string key)
		{
			return BackstoryDatabase.regex.Match(key).Captures[0].Value;
		}

		// Token: 0x04004B38 RID: 19256
		public static Dictionary<string, Backstory> allBackstories = new Dictionary<string, Backstory>();

		// Token: 0x04004B39 RID: 19257
		private static Dictionary<Pair<BackstorySlot, BackstoryCategoryFilter>, List<Backstory>> shuffleableBackstoryList = new Dictionary<Pair<BackstorySlot, BackstoryCategoryFilter>, List<Backstory>>();

		// Token: 0x04004B3A RID: 19258
		private static Regex regex = new Regex("^[^0-9]*");
	}
}
