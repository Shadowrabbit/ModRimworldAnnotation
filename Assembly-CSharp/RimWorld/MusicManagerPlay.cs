using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200162C RID: 5676
	public class MusicManagerPlay
	{
		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x06007B5F RID: 31583 RVA: 0x00052F5A File Offset: 0x0005115A
		private float CurTime
		{
			get
			{
				return Time.time;
			}
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x06007B60 RID: 31584 RVA: 0x00250978 File Offset: 0x0024EB78
		private bool DangerMusicMode
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].dangerWatcher.DangerRating == StoryDanger.High)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x06007B61 RID: 31585 RVA: 0x002509B4 File Offset: 0x0024EBB4
		private float CurVolume
		{
			get
			{
				float num = this.ignorePrefsVolumeThisSong ? 1f : Prefs.VolumeMusic;
				if (this.lastStartedSong == null)
				{
					return num;
				}
				return this.lastStartedSong.volume * num * this.fadeoutFactor * this.instrumentProximityFadeFactor;
			}
		}

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06007B62 RID: 31586 RVA: 0x00052F61 File Offset: 0x00051161
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerPlay");
			}
		}

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x06007B63 RID: 31587 RVA: 0x00052F73 File Offset: 0x00051173
		public bool IsPlaying
		{
			get
			{
				return this.audioSource.isPlaying;
			}
		}

		// Token: 0x06007B64 RID: 31588 RVA: 0x00052F80 File Offset: 0x00051180
		public void ForceSilenceFor(float time)
		{
			this.nextSongStartTime = this.CurTime + time;
		}

		// Token: 0x06007B65 RID: 31589 RVA: 0x002509FC File Offset: 0x0024EBFC
		public void MusicUpdate()
		{
			if (!this.gameObjectCreated)
			{
				this.gameObjectCreated = true;
				this.audioSource = new GameObject("MusicAudioSourceDummy")
				{
					transform = 
					{
						parent = Find.Root.soundRoot.sourcePool.sourcePoolCamera.cameraSourcesContainer.transform
					}
				}.AddComponent<AudioSource>();
				this.audioSource.bypassEffects = true;
				this.audioSource.bypassListenerEffects = true;
				this.audioSource.bypassReverbZones = true;
				this.audioSource.priority = 0;
			}
			this.UpdateSubtleAmbienceSoundVolumeMultiplier();
			if (this.disabled)
			{
				return;
			}
			if (this.songWasForced)
			{
				this.state = MusicManagerPlay.MusicManagerState.Normal;
				this.fadeoutFactor = 1f;
			}
			if (this.audioSource.isPlaying && !this.songWasForced && ((this.DangerMusicMode && !this.lastStartedSong.tense) || (!this.DangerMusicMode && this.lastStartedSong.tense)))
			{
				this.state = MusicManagerPlay.MusicManagerState.Fadeout;
			}
			this.audioSource.volume = this.CurSanitizedVolume;
			if (!this.audioSource.isPlaying)
			{
				if (this.DangerMusicMode && this.nextSongStartTime > this.CurTime + MusicManagerPlay.SongIntervalTension.max)
				{
					this.nextSongStartTime = this.CurTime + MusicManagerPlay.SongIntervalTension.RandomInRange;
				}
				if (this.nextSongStartTime < this.CurTime - 5f)
				{
					float randomInRange;
					if (this.DangerMusicMode)
					{
						randomInRange = MusicManagerPlay.SongIntervalTension.RandomInRange;
					}
					else
					{
						randomInRange = MusicManagerPlay.SongIntervalRelax.RandomInRange;
					}
					this.nextSongStartTime = this.CurTime + randomInRange;
				}
				if (this.CurTime >= this.nextSongStartTime)
				{
					this.ignorePrefsVolumeThisSong = false;
					this.StartNewSong();
				}
				return;
			}
			if (this.state == MusicManagerPlay.MusicManagerState.Fadeout)
			{
				this.fadeoutFactor -= Time.deltaTime / 10f;
				if (this.fadeoutFactor <= 0f)
				{
					this.audioSource.Stop();
					this.state = MusicManagerPlay.MusicManagerState.Normal;
					this.fadeoutFactor = 1f;
				}
			}
			Map currentMap = Find.CurrentMap;
			if (currentMap != null && !WorldRendererUtility.WorldRenderedNow)
			{
				float num = 1f;
				Camera camera = Find.Camera;
				List<Thing> list = currentMap.listerThings.ThingsInGroup(ThingRequestGroup.MusicalInstrument);
				for (int i = 0; i < list.Count; i++)
				{
					Building_MusicalInstrument building_MusicalInstrument = (Building_MusicalInstrument)list[i];
					if (building_MusicalInstrument.IsBeingPlayed)
					{
						Vector3 vector = camera.transform.position - building_MusicalInstrument.Position.ToVector3Shifted();
						vector.y = Mathf.Max(vector.y - 15f, 0f);
						vector.y *= 3.5f;
						float magnitude = vector.magnitude;
						FloatRange soundRange = building_MusicalInstrument.SoundRange;
						float num2 = Mathf.Min(Mathf.Max(magnitude - soundRange.min, 0f) / (soundRange.max - soundRange.min), 1f);
						if (num2 < num)
						{
							num = num2;
						}
					}
				}
				this.instrumentProximityFadeFactor = num;
				return;
			}
			this.instrumentProximityFadeFactor = 1f;
		}

		// Token: 0x06007B66 RID: 31590 RVA: 0x00250D1C File Offset: 0x0024EF1C
		private void UpdateSubtleAmbienceSoundVolumeMultiplier()
		{
			if (this.IsPlaying && this.CurSanitizedVolume > 0.001f)
			{
				this.subtleAmbienceSoundVolumeMultiplier -= Time.deltaTime * 0.1f;
			}
			else
			{
				this.subtleAmbienceSoundVolumeMultiplier += Time.deltaTime * 0.1f;
			}
			this.subtleAmbienceSoundVolumeMultiplier = Mathf.Clamp01(this.subtleAmbienceSoundVolumeMultiplier);
		}

		// Token: 0x06007B67 RID: 31591 RVA: 0x00250D84 File Offset: 0x0024EF84
		private void StartNewSong()
		{
			this.lastStartedSong = this.ChooseNextSong();
			this.audioSource.clip = this.lastStartedSong.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.spatialBlend = 0f;
			this.audioSource.Play();
			this.recentSongs.Enqueue(this.lastStartedSong);
		}

		// Token: 0x06007B68 RID: 31592 RVA: 0x00052F90 File Offset: 0x00051190
		public void ForceStartSong(SongDef song, bool ignorePrefsVolume)
		{
			this.forcedNextSong = song;
			this.ignorePrefsVolumeThisSong = ignorePrefsVolume;
			this.StartNewSong();
		}

		// Token: 0x06007B69 RID: 31593 RVA: 0x00250DF0 File Offset: 0x0024EFF0
		private SongDef ChooseNextSong()
		{
			this.songWasForced = false;
			if (this.forcedNextSong != null)
			{
				SongDef result = this.forcedNextSong;
				this.forcedNextSong = null;
				this.songWasForced = true;
				return result;
			}
			IEnumerable<SongDef> source = from song in DefDatabase<SongDef>.AllDefs
			where this.AppropriateNow(song)
			select song;
			while (this.recentSongs.Count > 7)
			{
				this.recentSongs.Dequeue();
			}
			while (!source.Any<SongDef>() && this.recentSongs.Count > 0)
			{
				this.recentSongs.Dequeue();
			}
			if (!source.Any<SongDef>())
			{
				Log.Error("Could not get any appropriate song. Getting random and logging song selection data.", false);
				this.SongSelectionData();
				return DefDatabase<SongDef>.GetRandom();
			}
			return source.RandomElementByWeight((SongDef s) => s.commonality);
		}

		// Token: 0x06007B6A RID: 31594 RVA: 0x00250EBC File Offset: 0x0024F0BC
		private bool AppropriateNow(SongDef song)
		{
			if (!song.playOnMap)
			{
				return false;
			}
			if (this.DangerMusicMode)
			{
				if (!song.tense)
				{
					return false;
				}
			}
			else if (song.tense)
			{
				return false;
			}
			Map map = Find.AnyPlayerHomeMap ?? Find.CurrentMap;
			if (!song.allowedSeasons.NullOrEmpty<Season>())
			{
				if (map == null)
				{
					return false;
				}
				if (!song.allowedSeasons.Contains(GenLocalDate.Season(map)))
				{
					return false;
				}
			}
			if (song.minRoyalTitle != null && !PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Any((Pawn p) => p.royalty.AllTitlesForReading.Any<RoyalTitle>() && p.royalty.MostSeniorTitle.def.seniority >= song.minRoyalTitle.seniority && !p.IsQuestLodger()))
			{
				return false;
			}
			if (this.recentSongs.Contains(song))
			{
				return false;
			}
			if (song.allowedTimeOfDay == TimeOfDay.Any)
			{
				return true;
			}
			if (map == null)
			{
				return true;
			}
			if (song.allowedTimeOfDay == TimeOfDay.Night)
			{
				return GenLocalDate.DayPercent(map) < 0.2f || GenLocalDate.DayPercent(map) > 0.7f;
			}
			return GenLocalDate.DayPercent(map) > 0.2f && GenLocalDate.DayPercent(map) < 0.7f;
		}

		// Token: 0x06007B6B RID: 31595 RVA: 0x00250FE4 File Offset: 0x0024F1E4
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MusicManagerMap");
			stringBuilder.AppendLine("state: " + this.state);
			stringBuilder.AppendLine("lastStartedSong: " + this.lastStartedSong);
			stringBuilder.AppendLine("fadeoutFactor: " + this.fadeoutFactor);
			stringBuilder.AppendLine("nextSongStartTime: " + this.nextSongStartTime);
			stringBuilder.AppendLine("CurTime: " + this.CurTime);
			stringBuilder.AppendLine("recentSongs: " + (from s in this.recentSongs
			select s.defName).ToCommaList(true));
			stringBuilder.AppendLine("disabled: " + this.disabled.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x06007B6C RID: 31596 RVA: 0x002510EC File Offset: 0x0024F2EC
		[DebugOutput]
		public void SongSelectionData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Most recent song: " + ((this.lastStartedSong != null) ? this.lastStartedSong.defName : "None"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Songs appropriate to play now:");
			foreach (SongDef songDef in from s in DefDatabase<SongDef>.AllDefs
			where this.AppropriateNow(s)
			select s)
			{
				stringBuilder.AppendLine("   " + songDef.defName);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Recently played songs:");
			foreach (SongDef songDef2 in this.recentSongs)
			{
				stringBuilder.AppendLine("   " + songDef2.defName);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x040050C6 RID: 20678
		private AudioSource audioSource;

		// Token: 0x040050C7 RID: 20679
		private MusicManagerPlay.MusicManagerState state;

		// Token: 0x040050C8 RID: 20680
		private float fadeoutFactor = 1f;

		// Token: 0x040050C9 RID: 20681
		private float nextSongStartTime = 12f;

		// Token: 0x040050CA RID: 20682
		private float instrumentProximityFadeFactor = 1f;

		// Token: 0x040050CB RID: 20683
		private SongDef lastStartedSong;

		// Token: 0x040050CC RID: 20684
		private Queue<SongDef> recentSongs = new Queue<SongDef>();

		// Token: 0x040050CD RID: 20685
		public bool disabled;

		// Token: 0x040050CE RID: 20686
		private SongDef forcedNextSong;

		// Token: 0x040050CF RID: 20687
		private bool songWasForced;

		// Token: 0x040050D0 RID: 20688
		private bool ignorePrefsVolumeThisSong;

		// Token: 0x040050D1 RID: 20689
		public float subtleAmbienceSoundVolumeMultiplier = 1f;

		// Token: 0x040050D2 RID: 20690
		private bool gameObjectCreated;

		// Token: 0x040050D3 RID: 20691
		private static readonly FloatRange SongIntervalRelax = new FloatRange(85f, 105f);

		// Token: 0x040050D4 RID: 20692
		private static readonly FloatRange SongIntervalTension = new FloatRange(2f, 5f);

		// Token: 0x040050D5 RID: 20693
		private const float FadeoutDuration = 10f;

		// Token: 0x0200162D RID: 5677
		private enum MusicManagerState
		{
			// Token: 0x040050D7 RID: 20695
			Normal,
			// Token: 0x040050D8 RID: 20696
			Fadeout
		}
	}
}
