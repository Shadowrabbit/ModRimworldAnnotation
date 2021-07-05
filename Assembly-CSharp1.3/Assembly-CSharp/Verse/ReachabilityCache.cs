using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001F7 RID: 503
	public class ReachabilityCache
	{
		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x0004F750 File Offset: 0x0004D950
		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0004F760 File Offset: 0x0004D960
		public BoolUnknown CachedResultFor(District A, District B, TraverseParms traverseParams)
		{
			bool flag;
			if (!this.cacheDict.TryGetValue(new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams), out flag))
			{
				return BoolUnknown.Unknown;
			}
			if (!flag)
			{
				return BoolUnknown.False;
			}
			return BoolUnknown.True;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0004F798 File Offset: 0x0004D998
		public void AddCachedResult(District A, District B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0004F7D5 File Offset: 0x0004D9D5
		public void Clear()
		{
			this.cacheDict.Clear();
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0004F7E4 File Offset: 0x0004D9E4
		public void ClearFor(Pawn p)
		{
			ReachabilityCache.tmpCachedEntries.Clear();
			foreach (KeyValuePair<ReachabilityCache.CachedEntry, bool> keyValuePair in this.cacheDict)
			{
				if (keyValuePair.Key.TraverseParms.pawn == p)
				{
					ReachabilityCache.tmpCachedEntries.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < ReachabilityCache.tmpCachedEntries.Count; i++)
			{
				this.cacheDict.Remove(ReachabilityCache.tmpCachedEntries[i]);
			}
			ReachabilityCache.tmpCachedEntries.Clear();
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0004F898 File Offset: 0x0004DA98
		public void ClearForHostile(Thing hostileTo)
		{
			ReachabilityCache.tmpCachedEntries.Clear();
			foreach (KeyValuePair<ReachabilityCache.CachedEntry, bool> keyValuePair in this.cacheDict)
			{
				Pawn pawn = keyValuePair.Key.TraverseParms.pawn;
				if (pawn != null && pawn.HostileTo(hostileTo))
				{
					ReachabilityCache.tmpCachedEntries.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < ReachabilityCache.tmpCachedEntries.Count; i++)
			{
				this.cacheDict.Remove(ReachabilityCache.tmpCachedEntries[i]);
			}
			ReachabilityCache.tmpCachedEntries.Clear();
		}

		// Token: 0x04000B83 RID: 2947
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		// Token: 0x04000B84 RID: 2948
		private static List<ReachabilityCache.CachedEntry> tmpCachedEntries = new List<ReachabilityCache.CachedEntry>();

		// Token: 0x0200197F RID: 6527
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			// Token: 0x1700192F RID: 6447
			// (get) Token: 0x060098B2 RID: 39090 RVA: 0x0035F75B File Offset: 0x0035D95B
			// (set) Token: 0x060098B3 RID: 39091 RVA: 0x0035F763 File Offset: 0x0035D963
			public int FirstID { get; private set; }

			// Token: 0x17001930 RID: 6448
			// (get) Token: 0x060098B4 RID: 39092 RVA: 0x0035F76C File Offset: 0x0035D96C
			// (set) Token: 0x060098B5 RID: 39093 RVA: 0x0035F774 File Offset: 0x0035D974
			public int SecondID { get; private set; }

			// Token: 0x17001931 RID: 6449
			// (get) Token: 0x060098B6 RID: 39094 RVA: 0x0035F77D File Offset: 0x0035D97D
			// (set) Token: 0x060098B7 RID: 39095 RVA: 0x0035F785 File Offset: 0x0035D985
			public TraverseParms TraverseParms { get; private set; }

			// Token: 0x060098B8 RID: 39096 RVA: 0x0035F78E File Offset: 0x0035D98E
			public CachedEntry(int firstID, int secondID, TraverseParms traverseParms)
			{
				this = default(ReachabilityCache.CachedEntry);
				if (firstID < secondID)
				{
					this.FirstID = firstID;
					this.SecondID = secondID;
				}
				else
				{
					this.FirstID = secondID;
					this.SecondID = firstID;
				}
				this.TraverseParms = traverseParms;
			}

			// Token: 0x060098B9 RID: 39097 RVA: 0x0035F7C0 File Offset: 0x0035D9C0
			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x060098BA RID: 39098 RVA: 0x0035F7CA File Offset: 0x0035D9CA
			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x060098BB RID: 39099 RVA: 0x0035F7D7 File Offset: 0x0035D9D7
			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			// Token: 0x060098BC RID: 39100 RVA: 0x0035F7EF File Offset: 0x0035D9EF
			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstID == other.FirstID && this.SecondID == other.SecondID && this.TraverseParms == other.TraverseParms;
			}

			// Token: 0x060098BD RID: 39101 RVA: 0x0035F823 File Offset: 0x0035DA23
			public override int GetHashCode()
			{
				return Gen.HashCombineStruct<TraverseParms>(Gen.HashCombineInt(this.FirstID, this.SecondID), this.TraverseParms);
			}
		}
	}
}
