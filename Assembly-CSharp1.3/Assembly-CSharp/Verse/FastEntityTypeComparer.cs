using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001B9 RID: 441
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x06000CAF RID: 3247 RVA: 0x0001F024 File Offset: 0x0001D224
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x000210E7 File Offset: 0x0001F2E7
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}

		// Token: 0x04000A17 RID: 2583
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();
	}
}
