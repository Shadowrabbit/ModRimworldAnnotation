using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E22 RID: 3618
	public static class BackstoryDatabase
	{
		// Token: 0x060053A1 RID: 21409 RVA: 0x001C5191 File Offset: 0x001C3391
		public static void Clear()
		{
			BackstoryDatabase.allBackstories.Clear();
		}

		// Token: 0x060053A2 RID: 21410 RVA: 0x001C51A0 File Offset: 0x001C33A0
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
					Log.Error(backstory.title + ": " + str);
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

		// Token: 0x060053A3 RID: 21411 RVA: 0x001C52B0 File Offset: 0x001C34B0
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
				Log.Error("Tried to add the same backstory twice " + bs.identifier);
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
			}));
		}

		// Token: 0x060053A4 RID: 21412 RVA: 0x001C5366 File Offset: 0x001C3566
		public static bool TryGetWithIdentifier(string identifier, out Backstory bs, bool closestMatchWarning = true)
		{
			identifier = BackstoryDatabase.GetIdentifierClosestMatch(identifier, closestMatchWarning);
			return BackstoryDatabase.allBackstories.TryGetValue(identifier, out bs);
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x001C5380 File Offset: 0x001C3580
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
						Log.Warning("Couldn't find exact match for backstory " + identifier + ", using closest match " + value.identifier);
					}
					return value.identifier;
				}
			}
			Log.Warning("Couldn't find exact match for backstory " + identifier + ", or any close match.");
			return identifier;
		}

		// Token: 0x060053A6 RID: 21414 RVA: 0x001C5440 File Offset: 0x001C3640
		public static Backstory RandomBackstory(BackstorySlot slot)
		{
			return (from bs in BackstoryDatabase.allBackstories
			where bs.Value.slot == slot
			select bs).RandomElement<KeyValuePair<string, Backstory>>().Value;
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x001C5480 File Offset: 0x001C3680
		public static List<Backstory> ShuffleableBackstoryList(BackstorySlot slot, BackstoryCategoryFilter group, BackstorySlot? mustBeCompatibleTo = null)
		{
			BackstoryDatabase.CacheKey key = new BackstoryDatabase.CacheKey(slot, group, mustBeCompatibleTo);
			if (!BackstoryDatabase.shuffleableBackstoryList.ContainsKey(key))
			{
				if (mustBeCompatibleTo == null)
				{
					BackstoryDatabase.shuffleableBackstoryList[key] = (from bs in BackstoryDatabase.allBackstories.Values
					where bs.shuffleable && bs.slot == slot && @group.Matches(bs)
					select bs).ToList<Backstory>();
				}
				else
				{
					List<Backstory> compatibleBackstories = BackstoryDatabase.ShuffleableBackstoryList(mustBeCompatibleTo.Value, group, null);
					BackstoryDatabase.shuffleableBackstoryList[key] = (from bs in BackstoryDatabase.allBackstories.Values
					where bs.shuffleable && bs.slot == slot && @group.Matches(bs) && compatibleBackstories.Any((Backstory b) => !b.requiredWorkTags.OverlapsWithOnAnyWorkType(bs.workDisables))
					select bs).ToList<Backstory>();
				}
			}
			return BackstoryDatabase.shuffleableBackstoryList[key];
		}

		// Token: 0x060053A8 RID: 21416 RVA: 0x001C5564 File Offset: 0x001C3764
		public static string StripNumericSuffix(string key)
		{
			return BackstoryDatabase.regex.Match(key).Captures[0].Value;
		}

		// Token: 0x0400312E RID: 12590
		public static Dictionary<string, Backstory> allBackstories = new Dictionary<string, Backstory>();

		// Token: 0x0400312F RID: 12591
		private static Dictionary<BackstoryDatabase.CacheKey, List<Backstory>> shuffleableBackstoryList = new Dictionary<BackstoryDatabase.CacheKey, List<Backstory>>();

		// Token: 0x04003130 RID: 12592
		private static Regex regex = new Regex("^[^0-9]*");

		// Token: 0x0200229A RID: 8858
		private struct CacheKey : IEquatable<BackstoryDatabase.CacheKey>
		{
			// Token: 0x0600C39F RID: 50079 RVA: 0x003D94EB File Offset: 0x003D76EB
			public CacheKey(BackstorySlot slot, BackstoryCategoryFilter filter, BackstorySlot? mustBeCompatibleTo = null)
			{
				this.slot = slot;
				this.filter = filter;
				this.mustBeCompatibleTo = null;
			}

			// Token: 0x0600C3A0 RID: 50080 RVA: 0x003D9507 File Offset: 0x003D7707
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(Gen.HashCombine<BackstoryCategoryFilter>(this.slot.GetHashCode(), this.filter), this.mustBeCompatibleTo.GetHashCode());
			}

			// Token: 0x0600C3A1 RID: 50081 RVA: 0x003D953C File Offset: 0x003D773C
			public bool Equals(BackstoryDatabase.CacheKey other)
			{
				if (this.slot == other.slot && this.filter == other.filter)
				{
					BackstorySlot? backstorySlot = this.mustBeCompatibleTo;
					BackstorySlot? backstorySlot2 = other.mustBeCompatibleTo;
					return backstorySlot.GetValueOrDefault() == backstorySlot2.GetValueOrDefault() & backstorySlot != null == (backstorySlot2 != null);
				}
				return false;
			}

			// Token: 0x0600C3A2 RID: 50082 RVA: 0x003D9598 File Offset: 0x003D7798
			public override bool Equals(object obj)
			{
				if (obj is BackstoryDatabase.CacheKey)
				{
					BackstoryDatabase.CacheKey other = (BackstoryDatabase.CacheKey)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x04008414 RID: 33812
			public BackstorySlot slot;

			// Token: 0x04008415 RID: 33813
			public BackstoryCategoryFilter filter;

			// Token: 0x04008416 RID: 33814
			public BackstorySlot? mustBeCompatibleTo;
		}
	}
}
