using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000579 RID: 1401
	public class AudioSourcePoolWorld
	{
		// Token: 0x0600292C RID: 10540 RVA: 0x000F92D0 File Offset: 0x000F74D0
		public AudioSourcePoolWorld()
		{
			GameObject gameObject = new GameObject("OneShotSourcesWorldContainer");
			gameObject.transform.position = Vector3.zero;
			for (int i = 0; i < 32; i++)
			{
				GameObject gameObject2 = new GameObject("OneShotSource_" + i.ToString());
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				this.sourcesWorld.Add(AudioSourceMaker.NewAudioSourceOn(gameObject2));
			}
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x000F9360 File Offset: 0x000F7560
		public AudioSource GetSourceWorld()
		{
			foreach (AudioSource audioSource in this.sourcesWorld)
			{
				if (!audioSource.isPlaying)
				{
					SoundFilterUtility.DisableAllFiltersOn(audioSource);
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x04001981 RID: 6529
		private List<AudioSource> sourcesWorld = new List<AudioSource>();

		// Token: 0x04001982 RID: 6530
		private const int NumSourcesWorld = 32;
	}
}
