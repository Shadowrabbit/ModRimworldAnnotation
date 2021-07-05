using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C4 RID: 708
	public class ReachabilityCache
	{
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x00012E8C File Offset: 0x0001108C
		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x000C3CDC File Offset: 0x000C1EDC
		public BoolUnknown CachedResultFor(Room A, Room B, TraverseParms traverseParams)
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

		// Token: 0x060011E2 RID: 4578 RVA: 0x000C3D14 File Offset: 0x000C1F14
		public void AddCachedResult(Room A, Room B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00012E99 File Offset: 0x00011099
		public void Clear()
		{
			this.cacheDict.Clear();
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x000C3D54 File Offset: 0x000C1F54
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

		// Token: 0x060011E5 RID: 4581 RVA: 0x000C3E08 File Offset: 0x000C2008
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

		// Token: 0x04000E72 RID: 3698
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		// Token: 0x04000E73 RID: 3699
		private static List<ReachabilityCache.CachedEntry> tmpCachedEntries = new List<ReachabilityCache.CachedEntry>();

		// Token: 0x020002C5 RID: 709
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			// Token: 0x1700034C RID: 844
			// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00012EC5 File Offset: 0x000110C5
			// (set) Token: 0x060011E9 RID: 4585 RVA: 0x00012ECD File Offset: 0x000110CD
			public int FirstRoomID { get; private set; }

			// Token: 0x1700034D RID: 845
			// (get) Token: 0x060011EA RID: 4586 RVA: 0x00012ED6 File Offset: 0x000110D6
			// (set) Token: 0x060011EB RID: 4587 RVA: 0x00012EDE File Offset: 0x000110DE
			public int SecondRoomID { get; private set; }

			// Token: 0x1700034E RID: 846
			// (get) Token: 0x060011EC RID: 4588 RVA: 0x00012EE7 File Offset: 0x000110E7
			// (set) Token: 0x060011ED RID: 4589 RVA: 0x00012EEF File Offset: 0x000110EF
			public TraverseParms TraverseParms { get; private set; }

			// Token: 0x060011EE RID: 4590 RVA: 0x00012EF8 File Offset: 0x000110F8
			public CachedEntry(int firstRoomID, int secondRoomID, TraverseParms traverseParms)
			{
				this = default(ReachabilityCache.CachedEntry);
				if (firstRoomID < secondRoomID)
				{
					this.FirstRoomID = firstRoomID;
					this.SecondRoomID = secondRoomID;
				}
				else
				{
					this.FirstRoomID = secondRoomID;
					this.SecondRoomID = firstRoomID;
				}
				this.TraverseParms = traverseParms;
			}

			// Token: 0x060011EF RID: 4591 RVA: 0x00012F2A File Offset: 0x0001112A
			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x060011F0 RID: 4592 RVA: 0x00012F34 File Offset: 0x00011134
			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x060011F1 RID: 4593 RVA: 0x00012F41 File Offset: 0x00011141
			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			// Token: 0x060011F2 RID: 4594 RVA: 0x00012F59 File Offset: 0x00011159
			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstRoomID == other.FirstRoomID && this.SecondRoomID == other.SecondRoomID && this.TraverseParms == other.TraverseParms;
			}

			// Token: 0x060011F3 RID: 4595 RVA: 0x00012F8D File Offset: 0x0001118D
			public override int GetHashCode()
			{
				return Gen.HashCombineStruct<TraverseParms>(Gen.HashCombineInt(this.FirstRoomID, this.SecondRoomID), this.TraverseParms);
			}
		}
	}
}
