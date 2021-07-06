using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000951 RID: 2385
	public static class AudioSourceMaker
	{
		// Token: 0x06003A7D RID: 14973 RVA: 0x0016A200 File Offset: 0x00168400
		public static AudioSource NewAudioSourceOn(GameObject go)
		{
			if (go.GetComponent<AudioSource>() != null)
			{
				Log.Warning("Adding audio source on " + go + " that already has one.", false);
				return go.GetComponent<AudioSource>();
			}
			AudioSource audioSource = go.AddComponent<AudioSource>();
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.dopplerLevel = 0f;
			audioSource.playOnAwake = false;
			return audioSource;
		}

		// Token: 0x04002883 RID: 10371
		private const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;
	}
}
