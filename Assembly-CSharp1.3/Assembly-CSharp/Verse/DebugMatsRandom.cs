using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018D RID: 397
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x06000B32 RID: 2866 RVA: 0x0003CD20 File Offset: 0x0003AF20
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0003CD6C File Offset: 0x0003AF6C
		public static Material Mat(int ind)
		{
			ind %= 100;
			if (ind < 0)
			{
				ind *= -1;
			}
			return DebugMatsRandom.mats[ind];
		}

		// Token: 0x04000951 RID: 2385
		private static readonly Material[] mats = new Material[100];

		// Token: 0x04000952 RID: 2386
		public const int MaterialCount = 100;

		// Token: 0x04000953 RID: 2387
		private const float Opacity = 0.25f;
	}
}
