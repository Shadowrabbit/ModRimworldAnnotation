using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200056E RID: 1390
	public class SampleSustainer : Sample
	{
		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002902 RID: 10498 RVA: 0x000F86F6 File Offset: 0x000F68F6
		public override float ParentStartRealTime
		{
			get
			{
				return this.subSustainer.creationRealTime;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002903 RID: 10499 RVA: 0x000F8703 File Offset: 0x000F6903
		public override float ParentStartTick
		{
			get
			{
				return (float)this.subSustainer.creationTick;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002904 RID: 10500 RVA: 0x000F8711 File Offset: 0x000F6911
		public override float ParentHashCode
		{
			get
			{
				return (float)this.subSustainer.GetHashCode();
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002905 RID: 10501 RVA: 0x000F871F File Offset: 0x000F691F
		public override SoundParams ExternalParams
		{
			get
			{
				return this.subSustainer.ExternalParams;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06002906 RID: 10502 RVA: 0x000F872C File Offset: 0x000F692C
		public override SoundInfo Info
		{
			get
			{
				return this.subSustainer.Info;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002907 RID: 10503 RVA: 0x000F873C File Offset: 0x000F693C
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

		// Token: 0x06002908 RID: 10504 RVA: 0x000F8885 File Offset: 0x000F6A85
		private SampleSustainer(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000F8890 File Offset: 0x000F6A90
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

		// Token: 0x0600290A RID: 10506 RVA: 0x000F8A43 File Offset: 0x000F6C43
		public override void SampleCleanup()
		{
			base.SampleCleanup();
			if (this.source != null && this.source.gameObject != null)
			{
				UnityEngine.Object.Destroy(this.source.gameObject);
			}
		}

		// Token: 0x0400196A RID: 6506
		public SubSustainer subSustainer;

		// Token: 0x0400196B RID: 6507
		public float scheduledEndTime;

		// Token: 0x0400196C RID: 6508
		public bool resolvedSkipAttack;
	}
}
