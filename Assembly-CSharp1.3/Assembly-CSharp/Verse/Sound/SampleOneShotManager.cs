using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Sound
{
	// Token: 0x0200056D RID: 1389
	public class SampleOneShotManager
	{
		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x000F8354 File Offset: 0x000F6554
		public IEnumerable<SampleOneShot> PlayingOneShots
		{
			get
			{
				return this.samples;
			}
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x000F835C File Offset: 0x000F655C
		private float CameraDistanceSquaredOf(SoundInfo info)
		{
			return (float)(Find.CameraDriver.MapPosition - info.Maker.Cell).LengthHorizontalSquared;
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x000F8390 File Offset: 0x000F6590
		private float ImportanceOf(SampleOneShot sample)
		{
			return this.ImportanceOf(sample.subDef.parentDef, sample.info, sample.AgeRealTime);
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x000F83AF File Offset: 0x000F65AF
		private float ImportanceOf(SoundDef def, SoundInfo info, float ageRealTime)
		{
			if (def.priorityMode == VoicePriorityMode.PrioritizeNearest)
			{
				return 1f / (this.CameraDistanceSquaredOf(info) + 1f);
			}
			if (def.priorityMode == VoicePriorityMode.PrioritizeNewest)
			{
				return 1f / (ageRealTime + 1f);
			}
			throw new NotImplementedException();
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x000F83EC File Offset: 0x000F65EC
		public bool CanAddPlayingOneShot(SoundDef def, SoundInfo info)
		{
			if (!SoundDefHelper.CorrectContextNow(def, info.Maker.Map))
			{
				return false;
			}
			if ((from s in this.samples
			where s.subDef.parentDef == def && s.AgeRealTime < 0.05f
			select s).Count<SampleOneShot>() >= def.MaxSimultaneousSamples)
			{
				return false;
			}
			SampleOneShot sampleOneShot = this.LeastImportantOf(def);
			return sampleOneShot == null || this.ImportanceOf(def, info, 0f) >= this.ImportanceOf(sampleOneShot);
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x000F8480 File Offset: 0x000F6680
		public void TryAddPlayingOneShot(SampleOneShot newSample)
		{
			if ((from s in this.samples
			where s.subDef.IsSameOrHasSameTag(newSample.subDef)
			select s).Count<SampleOneShot>() >= newSample.subDef.parentDef.maxVoices)
			{
				SampleOneShot sampleOneShot = this.LeastImportantOf(newSample.subDef);
				sampleOneShot.source.Stop();
				this.samples.Remove(sampleOneShot);
			}
			this.samples.Add(newSample);
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x000F8508 File Offset: 0x000F6708
		private SampleOneShot LeastImportantOf(SubSoundDef def)
		{
			SampleOneShot sampleOneShot = null;
			for (int i = 0; i < this.samples.Count; i++)
			{
				SampleOneShot sampleOneShot2 = this.samples[i];
				if (sampleOneShot2.subDef.IsSameOrHasSameTag(def) && (sampleOneShot == null || this.ImportanceOf(sampleOneShot2) < this.ImportanceOf(sampleOneShot)))
				{
					sampleOneShot = sampleOneShot2;
				}
			}
			return sampleOneShot;
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x000F8560 File Offset: 0x000F6760
		private SampleOneShot LeastImportantOf(SoundDef def)
		{
			SampleOneShot sampleOneShot = null;
			for (int i = 0; i < this.samples.Count; i++)
			{
				SampleOneShot sampleOneShot2 = this.samples[i];
				if (sampleOneShot2.subDef.parentDef == def && (sampleOneShot == null || this.ImportanceOf(sampleOneShot2) < this.ImportanceOf(sampleOneShot)))
				{
					sampleOneShot = sampleOneShot2;
				}
			}
			return sampleOneShot;
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x000F85B8 File Offset: 0x000F67B8
		public void SampleOneShotManagerUpdate()
		{
			for (int i = 0; i < this.samples.Count; i++)
			{
				this.samples[i].Update();
			}
			this.cleanupList.Clear();
			for (int j = 0; j < this.samples.Count; j++)
			{
				SampleOneShot sampleOneShot = this.samples[j];
				if (sampleOneShot.source == null || (!sampleOneShot.source.isPlaying && (!sampleOneShot.subDef.tempoAffectedByGameSpeed || !Find.TickManager.Paused)) || !SoundDefHelper.CorrectContextNow(sampleOneShot.subDef.parentDef, sampleOneShot.Map))
				{
					if (sampleOneShot.source != null && sampleOneShot.source.isPlaying)
					{
						sampleOneShot.source.Stop();
					}
					sampleOneShot.SampleCleanup();
					this.cleanupList.Add(sampleOneShot);
				}
			}
			if (this.cleanupList.Count > 0)
			{
				this.samples.RemoveAll((SampleOneShot s) => this.cleanupList.Contains(s));
			}
		}

		// Token: 0x04001968 RID: 6504
		private List<SampleOneShot> samples = new List<SampleOneShot>();

		// Token: 0x04001969 RID: 6505
		private List<SampleOneShot> cleanupList = new List<SampleOneShot>();
	}
}
