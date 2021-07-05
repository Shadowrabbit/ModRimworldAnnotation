using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001022 RID: 4130
	public class MusicManagerPlay
	{
		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x0600617C RID: 24956 RVA: 0x00211B3F File Offset: 0x0020FD3F
		private float CurTime
		{
			get
			{
				return Time.time;
			}
		}

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x0600617D RID: 24957 RVA: 0x00211B48 File Offset: 0x0020FD48
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

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x0600617E RID: 24958 RVA: 0x00211B84 File Offset: 0x0020FD84
		private float CurVolume
		{
			get
			{
				float num = this.ignorePrefsVolumeThisSong ? 1f : Prefs.VolumeMusic;
				if (this.lastStartedSong == null)
				{
					return num;
				}
				return this.lastStartedSong.volume * num * this.fadeoutFactor * this.musicOverridenFadeFactor;
			}
		}

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x0600617F RID: 24959 RVA: 0x00211BCB File Offset: 0x0020FDCB
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerPlay");
			}
		}

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x06006180 RID: 24960 RVA: 0x00211BDD File Offset: 0x0020FDDD
		public bool IsPlaying
		{
			get
			{
				return this.audioSource.isPlaying;
			}
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x00211BEA File Offset: 0x0020FDEA
		public void ForceSilenceFor(float time)
		{
			this.nextSongStartTime = this.CurTime + time;
		}

		// Token: 0x06006182 RID: 24962 RVA: 0x00211BFC File Offset: 0x0020FDFC
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
				MusicManagerPlay.<>c__DisplayClass28_0 CS$<>8__locals1;
				CS$<>8__locals1.camera = Find.Camera;
				List<Thing> list = currentMap.listerThings.ThingsInGroup(ThingRequestGroup.MusicalInstrument);
				for (int i = 0; i < list.Count; i++)
				{
					Building_MusicalInstrument building_MusicalInstrument = (Building_MusicalInstrument)list[i];
					if (building_MusicalInstrument.IsBeingPlayed)
					{
						float num2 = MusicManagerPlay.<MusicUpdate>g__FadeAmount|28_0(building_MusicalInstrument.Position.ToVector3Shifted(), building_MusicalInstrument.SoundRange, ref CS$<>8__locals1);
						if (num2 < num)
						{
							num = num2;
						}
					}
				}
				List<Thing> list2 = currentMap.listerThings.ThingsInGroup(ThingRequestGroup.MusicSource);
				for (int j = 0; j < list2.Count; j++)
				{
					Thing thing = list2[j];
					CompPlaysMusic compPlaysMusic = thing.TryGetComp<CompPlaysMusic>();
					if (compPlaysMusic.Playing)
					{
						float num3 = MusicManagerPlay.<MusicUpdate>g__FadeAmount|28_0(thing.Position.ToVector3Shifted(), compPlaysMusic.SoundRange, ref CS$<>8__locals1);
						if (num3 < num)
						{
							num = num3;
						}
					}
				}
				using (List<Lord>.Enumerator enumerator = currentMap.lordManager.lords.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LordJob_Ritual lordJob_Ritual;
						if ((lordJob_Ritual = (enumerator.Current.LordJob as LordJob_Ritual)) != null && lordJob_Ritual.AmbiencePlaying != null && !lordJob_Ritual.AmbiencePlaying.def.subSounds.NullOrEmpty<SubSoundDef>())
						{
							float num4 = MusicManagerPlay.<MusicUpdate>g__FadeAmount|28_0(lordJob_Ritual.selectedTarget.CenterVector3, lordJob_Ritual.AmbiencePlaying.def.subSounds.First<SubSoundDef>().distRange, ref CS$<>8__locals1);
							if (num4 < num)
							{
								num = num4;
							}
						}
					}
				}
				this.musicOverridenFadeFactor = num;
				return;
			}
			this.musicOverridenFadeFactor = 1f;
		}

		// Token: 0x06006183 RID: 24963 RVA: 0x00211FC0 File Offset: 0x002101C0
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

		// Token: 0x06006184 RID: 24964 RVA: 0x00212028 File Offset: 0x00210228
		private void StartNewSong()
		{
			this.lastStartedSong = this.ChooseNextSong();
			this.audioSource.clip = this.lastStartedSong.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.spatialBlend = 0f;
			this.audioSource.Play();
			this.recentSongs.Enqueue(this.lastStartedSong);
		}

		// Token: 0x06006185 RID: 24965 RVA: 0x00212094 File Offset: 0x00210294
		public void ForceStartSong(SongDef song, bool ignorePrefsVolume)
		{
			this.forcedNextSong = song;
			this.ignorePrefsVolumeThisSong = ignorePrefsVolume;
			this.StartNewSong();
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x002120AC File Offset: 0x002102AC
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
				Log.Error("Could not get any appropriate song. Getting random and logging song selection data.");
				this.SongSelectionData();
				return DefDatabase<SongDef>.GetRandom();
			}
			return source.RandomElementByWeight((SongDef s) => s.commonality);
		}

		// Token: 0x06006187 RID: 24967 RVA: 0x00212178 File Offset: 0x00210378
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

		// Token: 0x06006188 RID: 24968 RVA: 0x002122A0 File Offset: 0x002104A0
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
			select s.defName).ToCommaList(true, false));
			stringBuilder.AppendLine("disabled: " + this.disabled.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x002123A8 File Offset: 0x002105A8
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
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600618C RID: 24972 RVA: 0x00212538 File Offset: 0x00210738
		[CompilerGenerated]
		internal static float <MusicUpdate>g__FadeAmount|28_0(Vector3 pos, FloatRange soundRange, ref MusicManagerPlay.<>c__DisplayClass28_0 A_2)
		{
			Vector3 vector = A_2.camera.transform.position - pos;
			vector.y = Mathf.Max(vector.y - 15f, 0f);
			vector.y *= 3.5f;
			return Mathf.Min(Mathf.Max(vector.magnitude - soundRange.min, 0f) / (soundRange.max - soundRange.min), 1f);
		}

		// Token: 0x04003788 RID: 14216
		private AudioSource audioSource;

		// Token: 0x04003789 RID: 14217
		private MusicManagerPlay.MusicManagerState state;

		// Token: 0x0400378A RID: 14218
		private float fadeoutFactor = 1f;

		// Token: 0x0400378B RID: 14219
		private float nextSongStartTime = 12f;

		// Token: 0x0400378C RID: 14220
		private float musicOverridenFadeFactor = 1f;

		// Token: 0x0400378D RID: 14221
		private SongDef lastStartedSong;

		// Token: 0x0400378E RID: 14222
		private Queue<SongDef> recentSongs = new Queue<SongDef>();

		// Token: 0x0400378F RID: 14223
		public bool disabled;

		// Token: 0x04003790 RID: 14224
		private SongDef forcedNextSong;

		// Token: 0x04003791 RID: 14225
		private bool songWasForced;

		// Token: 0x04003792 RID: 14226
		private bool ignorePrefsVolumeThisSong;

		// Token: 0x04003793 RID: 14227
		public float subtleAmbienceSoundVolumeMultiplier = 1f;

		// Token: 0x04003794 RID: 14228
		private bool gameObjectCreated;

		// Token: 0x04003795 RID: 14229
		private static readonly FloatRange SongIntervalRelax = new FloatRange(85f, 105f);

		// Token: 0x04003796 RID: 14230
		private static readonly FloatRange SongIntervalTension = new FloatRange(2f, 5f);

		// Token: 0x04003797 RID: 14231
		private const float FadeoutDuration = 10f;

		// Token: 0x02002473 RID: 9331
		private enum MusicManagerState
		{
			// Token: 0x04008A9F RID: 35487
			Normal,
			// Token: 0x04008AA0 RID: 35488
			Fadeout
		}
	}
}
