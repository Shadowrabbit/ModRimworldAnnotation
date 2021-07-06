using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000954 RID: 2388
	public class AudioSourcePoolWorld
	{
		// Token: 0x06003A82 RID: 14978 RVA: 0x0016A360 File Offset: 0x00168560
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

		// Token: 0x06003A83 RID: 14979 RVA: 0x0016A3F0 File Offset: 0x001685F0
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

		// Token: 0x04002889 RID: 10377
		private List<AudioSource> sourcesWorld = new List<AudioSource>();

		// Token: 0x0400288A RID: 10378
		private const int NumSourcesWorld = 32;
	}
}
