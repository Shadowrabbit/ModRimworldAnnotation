using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200057C RID: 1404
	public class SubSustainer
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x000F9403 File Offset: 0x000F7603
		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002933 RID: 10547 RVA: 0x000F9410 File Offset: 0x000F7610
		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000F9420 File Offset: 0x000F7620
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

		// Token: 0x06002935 RID: 10549 RVA: 0x000F9478 File Offset: 0x000F7678
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
				}));
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
			this.subDef.Notify_GrainPlayed(resolvedGrain);
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

		// Token: 0x06002936 RID: 10550 RVA: 0x000F95A4 File Offset: 0x000F77A4
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

		// Token: 0x06002937 RID: 10551 RVA: 0x000F962D File Offset: 0x000F782D
		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000F9642 File Offset: 0x000F7842
		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x000F9666 File Offset: 0x000F7866
		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame;
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x000F9688 File Offset: 0x000F7888
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.creationRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000F96A0 File Offset: 0x000F78A0
		public string SamplesDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SampleSustainer sampleSustainer in this.samples)
			{
				stringBuilder.AppendLine(sampleSustainer.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001989 RID: 6537
		public Sustainer parent;

		// Token: 0x0400198A RID: 6538
		public SubSoundDef subDef;

		// Token: 0x0400198B RID: 6539
		private List<SampleSustainer> samples = new List<SampleSustainer>();

		// Token: 0x0400198C RID: 6540
		private float nextSampleStartTime;

		// Token: 0x0400198D RID: 6541
		public int creationFrame = -1;

		// Token: 0x0400198E RID: 6542
		public int creationTick = -1;

		// Token: 0x0400198F RID: 6543
		public float creationRealTime = -1f;

		// Token: 0x04001990 RID: 6544
		private const float MinSampleStartInterval = 0.01f;
	}
}
