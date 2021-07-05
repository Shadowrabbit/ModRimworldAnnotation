using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000338 RID: 824
	public static class AudioSourceUtility
	{
		// Token: 0x0600175B RID: 5979 RVA: 0x0008B6B4 File Offset: 0x000898B4
		public static float GetSanitizedVolume(float volume, object debugInfo)
		{
			if (float.IsNegativeInfinity(volume))
			{
				Log.ErrorOnce("Volume was negative infinity (" + debugInfo + ")", 863653423);
				return 0f;
			}
			if (float.IsPositiveInfinity(volume))
			{
				Log.ErrorOnce("Volume was positive infinity (" + debugInfo + ")", 954354323);
				return 1f;
			}
			if (float.IsNaN(volume))
			{
				Log.ErrorOnce("Volume was NaN (" + debugInfo + ")", 231846572);
				return 1f;
			}
			return Mathf.Clamp(volume, 0f, 1000f);
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x0008B74C File Offset: 0x0008994C
		public static float GetSanitizedPitch(float pitch, object debugInfo)
		{
			if (float.IsNegativeInfinity(pitch))
			{
				Log.ErrorOnce("Pitch was negative infinity (" + debugInfo + ")", 546475990);
				return 0.0001f;
			}
			if (float.IsPositiveInfinity(pitch))
			{
				Log.ErrorOnce("Pitch was positive infinity (" + debugInfo + ")", 309856435);
				return 1f;
			}
			if (float.IsNaN(pitch))
			{
				Log.ErrorOnce("Pitch was NaN (" + debugInfo + ")", 800635427);
				return 1f;
			}
			if (pitch < 0f)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Pitch was negative ",
					pitch,
					" (",
					debugInfo,
					")"
				}), 384765707);
				return 0.0001f;
			}
			return Mathf.Clamp(pitch, 0.0001f, 1000f);
		}
	}
}
