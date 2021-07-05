using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000953 RID: 2387
	public class AudioSourcePoolCamera
	{
		// Token: 0x06003A80 RID: 14976 RVA: 0x0016A258 File Offset: 0x00168458
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

		// Token: 0x06003A81 RID: 14977 RVA: 0x0016A318 File Offset: 0x00168518
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

		// Token: 0x04002886 RID: 10374
		public GameObject cameraSourcesContainer;

		// Token: 0x04002887 RID: 10375
		private List<AudioSource> sourcesCamera = new List<AudioSource>();

		// Token: 0x04002888 RID: 10376
		private const int NumSourcesCamera = 16;
	}
}
