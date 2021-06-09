using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000234 RID: 564
	public static class Altitudes
	{
		// Token: 0x06000E6E RID: 3694 RVA: 0x000B3458 File Offset: 0x000B1658
		static Altitudes()
		{
			for (int i = 0; i < 34; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.42857143f;
			}
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00010D86 File Offset: 0x0000EF86
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}

		// Token: 0x04000BFC RID: 3068
		private const int NumAltitudeLayers = 34;

		// Token: 0x04000BFD RID: 3069
		private static readonly float[] Alts = new float[34];

		// Token: 0x04000BFE RID: 3070
		private const float LayerSpacing = 0.42857143f;

		// Token: 0x04000BFF RID: 3071
		public const float AltInc = 0.042857144f;

		// Token: 0x04000C00 RID: 3072
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.042857144f, 0f);
	}
}
