using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000578 RID: 1400
	public class AudioSourcePoolCamera
	{
		// Token: 0x0600292A RID: 10538 RVA: 0x000F91C8 File Offset: 0x000F73C8
		public AudioSourcePoolCamera()
		{
			this.cameraSourcesContainer = new GameObject("OneShotSourcesCameraContainer");
			this.cameraSourcesContainer.transform.parent = Find.Camera.transform;
			this.cameraSourcesContainer.transform.localPosition = Vector3.zero;
			for (int i = 0; i < 16; i++)
			{
				AudioSource audioSource = AudioSourceMaker.NewAudioSourceOn(new GameObject("OneShotSourceCamera_" + i.ToString())
				{
					transform = 
					{
						parent = this.cameraSourcesContainer.transform,
						localPosition = Vector3.zero
					}
				});
				audioSource.bypassReverbZones = true;
				this.sourcesCamera.Add(audioSource);
			}
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000F9288 File Offset: 0x000F7488
		public AudioSource GetSourceCamera()
		{
			for (int i = 0; i < this.sourcesCamera.Count; i++)
			{
				AudioSource audioSource = this.sourcesCamera[i];
				if (!audioSource.isPlaying)
				{
					audioSource.clip = null;
					SoundFilterUtility.DisableAllFiltersOn(audioSource);
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x0400197E RID: 6526
		public GameObject cameraSourcesContainer;

		// Token: 0x0400197F RID: 6527
		private List<AudioSource> sourcesCamera = new List<AudioSource>();

		// Token: 0x04001980 RID: 6528
		private const int NumSourcesCamera = 16;
	}
}
