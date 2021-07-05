using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000948 RID: 2376
	public class SampleSustainer : Sample
	{
		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06003A4F RID: 14927 RVA: 0x0002CE9E File Offset: 0x0002B09E
		public override float ParentStartRealTime
		{
			get
			{
				return this.subSustainer.creationRealTime;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06003A50 RID: 14928 RVA: 0x0002CEAB File Offset: 0x0002B0AB
		public override float ParentStartTick
		{
			get
			{
				return (float)this.subSustainer.creationTick;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003A51 RID: 14929 RVA: 0x0002CEB9 File Offset: 0x0002B0B9
		public override float ParentHashCode
		{
			get
			{
				return (float)this.subSustainer.GetHashCode();
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003A52 RID: 14930 RVA: 0x0002CEC7 File Offset: 0x0002B0C7
		public override SoundParams ExternalParams
		{
			get
			{
				return this.subSustainer.ExternalParams;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06003A53 RID: 14931 RVA: 0x0002CED4 File Offset: 0x0002B0D4
		public override SoundInfo Info
		{
			get
			{
				return this.subSustainer.Info;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003A54 RID: 14932 RVA: 0x00169858 File Offset: 0x00167A58
		protected override float Volume
		{
			get
			{
				float num = base.Volume * this.subSustainer.parent.scopeFader.inScopePercent;
				float num2 = 1f;
				if (this.subSustainer.parent.Ended)
				{
					num2 = 1f - Mathf.Min(this.subSustainer.parent.TimeSinceEnd / this.subDef.parentDef.sustainFadeoutTime, 1f);
				}
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (base.AgeRealTime < this.subDef.sustainAttack)
				{
					if (this.resolvedSkipAttack || this.subDef.sustainAttack < 0.01f)
					{
						return num * num2;
					}
					float num3 = base.AgeRealTime / this.subDef.sustainAttack;
					num3 = Mathf.Sqrt(num3);
					return Mathf.Lerp(0f, num, num3) * num2;
				}
				else
				{
					if (realtimeSinceStartup > this.scheduledEndTime - this.subDef.sustainRelease)
					{
						float num4 = (realtimeSinceStartup - (this.scheduledEndTime - this.subDef.sustainRelease)) / this.subDef.sustainRelease;
						num4 = 1f - num4;
						num4 = Mathf.Max(num4, 0f);
						num4 = Mathf.Sqrt(num4);
						num4 = 1f - num4;
						return Mathf.Lerp(num, 0f, num4) * num2;
					}
					return num * num2;
				}
			}
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x0002CEE1 File Offset: 0x0002B0E1
		private SampleSustainer(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x001699A4 File Offset: 0x00167BA4
		public static SampleSustainer TryMakeAndPlay(SubSustainer subSus, AudioClip clip, float scheduledEndTime)
		{
			SampleSustainer sampleSustainer = new SampleSustainer(subSus.subDef);
			sampleSustainer.subSustainer = subSus;
			sampleSustainer.scheduledEndTime = scheduledEndTime;
			GameObject gameObject = new GameObject(string.Concat(new object[]
			{
				"SampleSource_",
				sampleSustainer.subDef.name,
				"_",
				sampleSustainer.startRealTime
			}));
			GameObject gameObject2 = subSus.subDef.onCamera ? Find.Camera.gameObject : subSus.parent.worldRootObject;
			gameObject.transform.parent = gameObject2.transform;
			gameObject.transform.localPosition = Vector3.zero;
			sampleSustainer.source = AudioSourceMaker.NewAudioSourceOn(gameObject);
			if (sampleSustainer.source == null)
			{
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				return null;
			}
			sampleSustainer.source.clip = clip;
			sampleSustainer.source.volume = sampleSustainer.SanitizedVolume;
			sampleSustainer.source.pitch = sampleSustainer.SanitizedPitch;
			sampleSustainer.source.minDistance = sampleSustainer.subDef.distRange.TrueMin;
			sampleSustainer.source.maxDistance = sampleSustainer.subDef.distRange.TrueMax;
			sampleSustainer.source.spatialBlend = 1f;
			List<SoundFilter> filters = sampleSustainer.subDef.filters;
			for (int i = 0; i < filters.Count; i++)
			{
				filters[i].SetupOn(sampleSustainer.source);
			}
			if (sampleSustainer.subDef.sustainLoop)
			{
				sampleSustainer.source.loop = true;
			}
			sampleSustainer.Update();
			sampleSustainer.source.Play();
			sampleSustainer.source.Play();
			return sampleSustainer;
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x0002CEEA File Offset: 0x0002B0EA
		public override void SampleCleanup()
		{
			base.SampleCleanup();
			if (this.source != null && this.source.gameObject != null)
			{
				UnityEngine.Object.Destroy(this.source.gameObject);
			}
		}

		// Token: 0x0400286D RID: 10349
		public SubSustainer subSustainer;

		// Token: 0x0400286E RID: 10350
		public float scheduledEndTime;

		// Token: 0x0400286F RID: 10351
		public bool resolvedSkipAttack;
	}
}
