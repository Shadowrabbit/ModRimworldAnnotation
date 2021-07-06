using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000275 RID: 629
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		// Token: 0x06001048 RID: 4168 RVA: 0x000121C3 File Offset: 0x000103C3
		public bool Equals(ThingDef x, ThingDef y)
		{
			return (x == null && y == null) || (x != null && y != null && x.shortHash == y.shortHash);
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0000CDDA File Offset: 0x0000AFDA
		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000CF0 RID: 3312
		public static readonly ThingDefComparer Instance = new ThingDefComparer();
	}
}
