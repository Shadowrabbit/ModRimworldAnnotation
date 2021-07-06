using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000949 RID: 2377
	public static class SoundFilterUtility
	{
		// Token: 0x06003A58 RID: 14936 RVA: 0x0002CF23 File Offset: 0x0002B123
		public static void DisableAllFiltersOn(AudioSource source)
		{
			SoundFilterUtility.DisableFilterOn<AudioLowPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioHighPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioEchoFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioReverbFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioDistortionFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioChorusFilter>(source);
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x00169B58 File Offset: 0x00167D58
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
