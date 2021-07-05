using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000E3 RID: 227
	public class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x06000639 RID: 1593 RVA: 0x0001F024 File Offset: 0x0001D224
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001F02A File Offset: 0x0001D22A
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x0400050B RID: 1291
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();
	}
}
