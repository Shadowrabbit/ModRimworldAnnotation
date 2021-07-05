using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047D RID: 1149
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x06001D04 RID: 7428 RVA: 0x0001A2EA File Offset: 0x000184EA
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x0001A2F3 File Offset: 0x000184F3
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x040014AC RID: 5292
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();
	}
}
