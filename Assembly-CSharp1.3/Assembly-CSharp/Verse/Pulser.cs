using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000437 RID: 1079
	public static class Pulser
	{
		// Token: 0x06002070 RID: 8304 RVA: 0x000C8FBB File Offset: 0x000C71BB
		public static float PulseBrightness(float frequency, float amplitude)
		{
			return Pulser.PulseBrightness(frequency, amplitude, Time.realtimeSinceStartup);
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x000C8FCC File Offset: 0x000C71CC
		public static float PulseBrightness(float frequency, float amplitude, float time)
		{
			float num = time * 6.2831855f;
			num *= frequency;
			float t = (1f - Mathf.Cos(num)) * 0.5f;
			return Mathf.Lerp(1f - amplitude, 1f, t);
		}
	}
}
