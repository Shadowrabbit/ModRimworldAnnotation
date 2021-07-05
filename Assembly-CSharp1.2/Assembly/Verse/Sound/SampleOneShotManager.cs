using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Sound
{
	// Token: 0x02000945 RID: 2373
	public class SampleOneShotManager
	{
		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06003A41 RID: 14913 RVA: 0x0002CDD8 File Offset: 0x0002AFD8
		public IEnumerable<SampleOneShot> PlayingOneShots
		{
			get
			{
				return this.samples;
			}
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x00169594 File Offset: 0x00167794
		private float CameraDistanceSquaredOf(SoundInfo info)
		{
			return (float)(Find.CameraDriver.MapPosition - info.Maker.Cell).LengthHorizontalSquared;
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x0002CDE0 File Offset: 0x0002AFE0
		private float ImportanceOf(SampleOneShot sample)
		{
			return this.ImportanceOf(sample.subDef.parentDef, sample.info, sample.AgeRealTime);
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x0002CDFF File Offset: 0x0002AFFF
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

		// Token: 0x06003A45 RID: 14917 RVA: 0x001695C8 File Offset: 0x001677C8
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

		// Token: 0x06003A46 RID: 14918 RVA: 0x0016965C File Offset: 0x0016785C
		public void TryAddPlayingOneShot(SampleOneShot newSample)
		{
			if ((from s in this.samples
			where s.subDef == newSample.subDef
			select s).Count<SampleOneShot>() >= newSample.subDef.parentDef.maxVoices)
			{
				SampleOneShot sampleOneShot = this.LeastImportantOf(newSample.subDef.parentDef);
				sampleOneShot.source.Stop();
				this.samples.Remove(sampleOneShot);
			}
			this.samples.Add(newSample);
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x001696EC File Offset: 0x001678EC
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

		// Token: 0x06003A48 RID: 14920 RVA: 0x00169744 File Offset: 0x00167944
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

		// Token: 0x04002869 RID: 10345
		private List<SampleOneShot> samples = new List<SampleOneShot>();

		// Token: 0x0400286A RID: 10346
		private List<SampleOneShot> cleanupList = new List<SampleOneShot>();
	}
}
