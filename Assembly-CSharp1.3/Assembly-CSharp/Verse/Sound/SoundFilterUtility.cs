using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200056F RID: 1391
	public static class SoundFilterUtility
	{
		// Token: 0x0600290B RID: 10507 RVA: 0x000F8A7C File Offset: 0x000F6C7C
		public static void DisableAllFiltersOn(AudioSource source)
		{
			SoundFilterUtility.DisableFilterOn<AudioLowPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioHighPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioEchoFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioReverbFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioDistortionFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioChorusFilter>(source);
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000F8AA4 File Offset: 0x000F6CA4
		private static void DisableFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T component = source.GetComponent<T>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}
}
