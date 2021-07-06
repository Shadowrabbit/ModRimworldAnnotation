using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000957 RID: 2391
	public class SubSustainer
	{
		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06003A88 RID: 14984 RVA: 0x0002D135 File Offset: 0x0002B335
		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06003A89 RID: 14985 RVA: 0x0002D142 File Offset: 0x0002B342
		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x0016A454 File Offset: 0x00168654
		public SubSustainer(Sustainer parent, SubSoundDef subSoundDef)
		{
			this.parent = parent;
			this.subDef = subSoundDef;
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.creationFrame = Time.frameCount;
				this.creationRealTime = Time.realtimeSinceStartup;
				if (Current.ProgramState == ProgramState.Playing)
				{
					this.creationTick = Find.TickManager.TicksGame;
				}
				if (this.subDef.startDelayRange.TrueMax < 0.001f)
				{
					this.StartSample();
					return;
				}
				this.nextSampleStartTime = Time.realtimeSinceStartup + this.subDef.startDelayRange.RandomInRange;
			});
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x0016A4AC File Offset: 0x001686AC
		private void StartSample()
		{
			ResolvedGrain resolvedGrain = this.subDef.RandomizedResolvedGrain();
			if (resolvedGrain == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"SubSustainer for ",
					this.subDef,
					" of ",
					this.parent.def,
					" could not resolve any grains."
				}), false);
				this.parent.End();
				return;
			}
			float num;
			if (this.subDef.sustainLoop)
			{
				num = this.subDef.sustainLoopDurationRange.RandomInRange;
			}
			else
			{
				num = resolvedGrain.duration;
			}
			float num2 = Time.realtimeSinceStartup + num;
			this.nextSampleStartTime = num2 + this.subDef.sustainIntervalRange.RandomInRange;
			if (this.nextSampleStartTime < Time.realtimeSinceStartup + 0.01f)
			{
				this.nextSampleStartTime = Time.realtimeSinceStartup + 0.01f;
			}
			if (resolvedGrain is ResolvedGrain_Silence)
			{
				return;
			}
			SampleSustainer sampleSustainer = SampleSustainer.TryMakeAndPlay(this, ((ResolvedGrain_Clip)resolvedGrain).clip, num2);
			if (sampleSustainer == null)
			{
				return;
			}
			if (this.subDef.sustainSkipFirstAttack && Time.frameCount == this.creationFrame)
			{
				sampleSustainer.resolvedSkipAttack = true;
			}
			this.samples.Add(sampleSustainer);
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x0016A5CC File Offset: 0x001687CC
		public void SubSustainerUpdate()
		{
			for (int i = this.samples.Count - 1; i >= 0; i--)
			{
				if (Time.realtimeSinceStartup > this.samples[i].scheduledEndTime)
				{
					this.EndSample(this.samples[i]);
				}
			}
			if (Time.realtimeSinceStartup > this.nextSampleStartTime)
			{
				this.StartSample();
			}
			for (int j = 0; j < this.samples.Count; j++)
			{
				this.samples[j].Update();
			}
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x0002D14F File Offset: 0x0002B34F
		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x0002D164 File Offset: 0x0002B364
		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x0002D188 File Offset: 0x0002B388
		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame;
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x0002D1AA File Offset: 0x0002B3AA
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.creationRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x0016A658 File Offset: 0x00168858
		public string SamplesDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SampleSustainer sampleSustainer in this.samples)
			{
				stringBuilder.AppendLine(sampleSustainer.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002891 RID: 10385
		public Sustainer parent;

		// Token: 0x04002892 RID: 10386
		public SubSoundDef subDef;

		// Token: 0x04002893 RID: 10387
		private List<SampleSustainer> samples = new List<SampleSustainer>();

		// Token: 0x04002894 RID: 10388
		private float nextSampleStartTime;

		// Token: 0x04002895 RID: 10389
		public int creationFrame = -1;

		// Token: 0x04002896 RID: 10390
		public int creationTick = -1;

		// Token: 0x04002897 RID: 10391
		public float creationRealTime = -1f;

		// Token: 0x04002898 RID: 10392
		private const float MinSampleStartInterval = 0.01f;
	}
}
