using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000154 RID: 340
	public class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x060008AC RID: 2220 RVA: 0x0000CDD4 File Offset: 0x0000AFD4
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0000CDDA File Offset: 0x0000AFDA
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000709 RID: 1801
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();
	}
}
