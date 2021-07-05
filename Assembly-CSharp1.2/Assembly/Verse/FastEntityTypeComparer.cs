using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000274 RID: 628
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x06001044 RID: 4164 RVA: 0x0000CDD4 File Offset: 0x0000AFD4
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0001037D File Offset: 0x0000E57D
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}

		// Token: 0x04000CEF RID: 3311
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();
	}
}
