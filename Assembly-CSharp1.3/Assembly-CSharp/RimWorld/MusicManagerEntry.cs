using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001021 RID: 4129
	public class MusicManagerEntry
	{
		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06006176 RID: 24950 RVA: 0x00211955 File Offset: 0x0020FB55
		private float CurVolume
		{
			get
			{
				return Prefs.VolumeMusic * SongDefOf.EntrySong.volume;
			}
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x06006177 RID: 24951 RVA: 0x00211967 File Offset: 0x0020FB67
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerEntry");
			}
		}

		// Token: 0x06006178 RID: 24952 RVA: 0x0021197C File Offset: 0x0020FB7C
		public void MusicManagerEntryUpdate()
		{
			if (this.audioSource == null || !this.audioSource.isPlaying)
			{
				this.StartPlaying();
			}
			float curSanitizedVolume = this.CurSanitizedVolume;
			if (Time.frameCount > this.silentTillFrame)
			{
				this.silenceMultiplier = Mathf.Clamp01(this.silenceMultiplier + 1.75f * Time.deltaTime);
			}
			else if (Time.frameCount <= this.silentTillFrame)
			{
				this.silenceMultiplier = Mathf.Clamp01(this.silenceMultiplier - 1.75f * Time.deltaTime);
			}
			this.audioSource.volume = curSanitizedVolume * this.silenceMultiplier;
		}

		// Token: 0x06006179 RID: 24953 RVA: 0x00211A1C File Offset: 0x0020FC1C
		private void StartPlaying()
		{
			if (this.audioSource != null && !this.audioSource.isPlaying)
			{
				this.audioSource.Play();
				return;
			}
			if (GameObject.Find("MusicAudioSourceDummy") != null)
			{
				Log.Error("MusicManagerEntry did StartPlaying but there is already a music source GameObject.");
				return;
			}
			this.audioSource = new GameObject("MusicAudioSourceDummy")
			{
				transform = 
				{
					parent = Camera.main.transform
				}
			}.AddComponent<AudioSource>();
			this.audioSource.bypassEffects = true;
			this.audioSource.bypassListenerEffects = true;
			this.audioSource.bypassReverbZones = true;
			this.audioSource.priority = 0;
			this.audioSource.clip = SongDefOf.EntrySong.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.loop = true;
			this.audioSource.spatialBlend = 0f;
			this.audioSource.Play();
		}

		// Token: 0x0600617A RID: 24954 RVA: 0x00211B16 File Offset: 0x0020FD16
		public void MaintainSilence()
		{
			this.silentTillFrame = Time.frameCount + 1;
		}

		// Token: 0x04003783 RID: 14211
		private AudioSource audioSource;

		// Token: 0x04003784 RID: 14212
		private int silentTillFrame = -1;

		// Token: 0x04003785 RID: 14213
		private float silenceMultiplier = 1f;

		// Token: 0x04003786 RID: 14214
		private const string SourceGameObjectName = "MusicAudioSourceDummy";

		// Token: 0x04003787 RID: 14215
		private const float SilenceMultiplierChangePerSecond = 1.75f;
	}
}
