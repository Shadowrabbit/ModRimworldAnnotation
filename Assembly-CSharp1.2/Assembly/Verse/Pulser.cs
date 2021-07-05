using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000778 RID: 1912
	public static class Pulser
	{
		// Token: 0x06003018 RID: 12312 RVA: 0x00025E76 File Offset: 0x00024076
		public static float PulseBrightness(float frequency, float amplitude)
		{
			return Pulser.PulseBrightness(frequency, amplitude, Time.realtimeSinceStartup);
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x0013D974 File Offset: 0x0013BB74
		public static float PulseBrightness(float frequency, float amplitude, float time)
		{
			float num = time * 6.2831855f;
			num *= frequency;
			float t = (1f - Mathf.Cos(num)) * 0.5f;
			return Mathf.Lerp(1f - amplitude, 1f, t);
		}
	}
}
