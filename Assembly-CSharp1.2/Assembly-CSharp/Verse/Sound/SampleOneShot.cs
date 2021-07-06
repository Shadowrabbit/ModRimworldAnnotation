using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000944 RID: 2372
	public class SampleOneShot : Sample
	{
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06003A3A RID: 14906 RVA: 0x0002CD9A File Offset: 0x0002AF9A
		public override float ParentStartRealTime
		{
			get
			{
				return this.startRealTime;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06003A3B RID: 14907 RVA: 0x0002CDA2 File Offset: 0x0002AFA2
		public override float ParentStartTick
		{
			get
			{
				return (float)this.startTick;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06003A3C RID: 14908 RVA: 0x0002CDAB File Offset: 0x0002AFAB
		public override float ParentHashCode
		{
			get
			{
				return (float)this.GetHashCode();
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06003A3D RID: 14909 RVA: 0x0002CDB4 File Offset: 0x0002AFB4
		public override SoundParams ExternalParams
		{
			get
			{
				return this.externalParams;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06003A3E RID: 14910 RVA: 0x0002CDBC File Offset: 0x0002AFBC
		public override SoundInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x0002CDC4 File Offset: 0x0002AFC4
		private SampleOneShot(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x0016935C File Offset: 0x0016755C
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
				}), 632321, false);
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

		// Token: 0x04002867 RID: 10343
		public SoundInfo info;

		// Token: 0x04002868 RID: 10344
		private SoundParams externalParams = new SoundParams();
	}
}
