using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200016E RID: 366
	public static class Altitudes
	{
		// Token: 0x06000A39 RID: 2617 RVA: 0x000385E8 File Offset: 0x000367E8
		static Altitudes()
		{
			for (int i = 0; i < 36; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.4054054f;
			}
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00038636 File Offset: 0x00036836
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0003863F File Offset: 0x0003683F
		public static float AltitudeFor(this AltitudeLayer alt, float incOffset)
		{
			return alt.AltitudeFor() + incOffset * 0.04054054f;
		}

		// Token: 0x040008C7 RID: 2247
		private const int NumAltitudeLayers = 36;

		// Token: 0x040008C8 RID: 2248
		private static readonly float[] Alts = new float[36];

		// Token: 0x040008C9 RID: 2249
		private const float LayerSpacing = 0.4054054f;

		// Token: 0x040008CA RID: 2250
		public const float AltInc = 0.04054054f;

		// Token: 0x040008CB RID: 2251
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.04054054f, 0f);
	}
}
