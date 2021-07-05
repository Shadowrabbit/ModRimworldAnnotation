using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000576 RID: 1398
	public static class AudioSourceMaker
	{
		// Token: 0x06002927 RID: 10535 RVA: 0x000F9138 File Offset: 0x000F7338
		public static AudioSource NewAudioSourceOn(GameObject go)
		{
			if (go.GetComponent<AudioSource>() != null)
			{
				Log.Warning("Adding audio source on " + go + " that already has one.");
				return go.GetComponent<AudioSource>();
			}
			AudioSource audioSource = go.AddComponent<AudioSource>();
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.dopplerLevel = 0f;
			audioSource.playOnAwake = false;
			return audioSource;
		}

		// Token: 0x0400197B RID: 6523
		private const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;
	}
}
