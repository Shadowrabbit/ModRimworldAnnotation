using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200030E RID: 782
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x0600167F RID: 5759 RVA: 0x00083280 File Offset: 0x00081480
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x00083289 File Offset: 0x00081489
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000F99 RID: 3993
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();
	}
}
