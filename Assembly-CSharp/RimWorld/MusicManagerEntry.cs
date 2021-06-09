using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200162B RID: 5675
	public class MusicManagerEntry
	{
		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x06007B5A RID: 31578 RVA: 0x00052F02 File Offset: 0x00051102
		private float CurVolume
		{
			get
			{
				return Prefs.VolumeMusic * SongDefOf.EntrySong.volume;
			}
		}

		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x06007B5B RID: 31579 RVA: 0x00052F14 File Offset: 0x00051114
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerEntry");
			}
		}

		// Token: 0x06007B5C RID: 31580 RVA: 0x00052F26 File Offset: 0x00051126
		public void MusicManagerEntryUpdate()
		{
			if (this.audioSource == null || !this.audioSource.isPlaying)
			{
				this.StartPlaying();
			}
			this.audioSource.volume = this.CurSanitizedVolume;
		}

		// Token: 0x06007B5D RID: 31581 RVA: 0x0025087C File Offset: 0x0024EA7C
		private void StartPlaying()
		{
			if (this.audioSource != null && !this.audioSource.isPlaying)
			{
				this.audioSource.Play();
				return;
			}
			if (GameObject.Find("MusicAudioSourceDummy") != null)
			{
				Log.Error("MusicManagerEntry did StartPlaying but there is already a music source GameObject.", false);
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

		// Token: 0x040050C4 RID: 20676
		private AudioSource audioSource;

		// Token: 0x040050C5 RID: 20677
		private const string SourceGameObjectName = "MusicAudioSourceDummy";
	}
}
