using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001BA RID: 442
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		// Token: 0x06000CB3 RID: 3251 RVA: 0x00043759 File Offset: 0x00041959
		public bool Equals(ThingDef x, ThingDef y)
		{
			return (x == null && y == null) || (x != null && y != null && x.shortHash == y.shortHash);
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0001F02A File Offset: 0x0001D22A
		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000A18 RID: 2584
		public static readonly ThingDefComparer Instance = new ThingDefComparer();
	}
}
