using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200056C RID: 1388
	public class SampleOneShot : Sample
	{
		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060028F0 RID: 10480 RVA: 0x000F80DE File Offset: 0x000F62DE
		public override float ParentStartRealTime
		{
			get
			{
				return this.startRealTime;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060028F1 RID: 10481 RVA: 0x000F80E6 File Offset: 0x000F62E6
		public override float ParentStartTick
		{
			get
			{
				return (float)this.startTick;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060028F2 RID: 10482 RVA: 0x000F80EF File Offset: 0x000F62EF
		public override float ParentHashCode
		{
			get
			{
				return (float)this.GetHashCode();
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060028F3 RID: 10483 RVA: 0x000F80F8 File Offset: 0x000F62F8
		public override SoundParams ExternalParams
		{
			get
			{
				return this.externalParams;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060028F4 RID: 10484 RVA: 0x000F8100 File Offset: 0x000F6300
		public override SoundInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x000F8108 File Offset: 0x000F6308
		private SampleOneShot(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x000F811C File Offset: 0x000F631C
		public static SampleOneShot TryMakeAndPlay(SubSoundDef def, AudioClip clip, SoundInfo info)
		{
			if ((double)info.pitchFactor <= 0.0001)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Played sound with pitchFactor ",
					info.pitchFactor,
					": ",
					def,
					", ",
					info
				}), 632321);
				return null;
			}
			SampleOneShot sampleOneShot = new SampleOneShot(def);
			sampleOneShot.info = info;
			sampleOneShot.source = Find.SoundRoot.sourcePool.GetSource(def.onCamera);
			if (sampleOneShot.source == null)
			{
				return null;
			}
			sampleOneShot.source.clip = clip;
			sampleOneShot.source.volume = sampleOneShot.SanitizedVolume;
			sampleOneShot.source.pitch = sampleOneShot.SanitizedPitch;
			sampleOneShot.source.minDistance = sampleOneShot.subDef.distRange.TrueMin;
			sampleOneShot.source.maxDistance = sampleOneShot.subDef.distRange.TrueMax;
			if (!def.onCamera)
			{
				sampleOneShot.source.gameObject.transform.position = info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
				sampleOneShot.source.minDistance = def.distRange.TrueMin;
				sampleOneShot.source.maxDistance = def.distRange.TrueMax;
				sampleOneShot.source.spatialBlend = 1f;
			}
			else
			{
				sampleOneShot.source.spatialBlend = 0f;
			}
			for (int i = 0; i < def.filters.Count; i++)
			{
				def.filters[i].SetupOn(sampleOneShot.source);
			}
			foreach (KeyValuePair<string, float> keyValuePair in info.DefinedParameters)
			{
				sampleOneShot.externalParams[keyValuePair.Key] = keyValuePair.Value;
			}
			sampleOneShot.Update();
			sampleOneShot.source.Play();
			Find.SoundRoot.oneShotManager.TryAddPlayingOneShot(sampleOneShot);
			return sampleOneShot;
		}

		// Token: 0x04001966 RID: 6502
		public SoundInfo info;

		// Token: 0x04001967 RID: 6503
		private SoundParams externalParams = new SoundParams();
	}
}
