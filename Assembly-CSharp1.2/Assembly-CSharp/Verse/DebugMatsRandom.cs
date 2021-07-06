using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024A RID: 586
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x06000EE2 RID: 3810 RVA: 0x000B47D4 File Offset: 0x000B29D4
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00011323 File Offset: 0x0000F523
		public static Material Mat(int ind)
		{
			ind %= 100;
			if (ind < 0)
			{
				ind *= -1;
			}
			return DebugMatsRandom.mats[ind];
		}

		// Token: 0x04000C42 RID: 3138
		private static readonly Material[] mats = new Material[100];

		// Token: 0x04000C43 RID: 3139
		public const int MaterialCount = 100;

		// Token: 0x04000C44 RID: 3140
		private const float Opacity = 0.25f;
	}
}
