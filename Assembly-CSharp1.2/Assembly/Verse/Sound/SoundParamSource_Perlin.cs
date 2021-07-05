using System;
using UnityEngine;
using Verse.Noise;

namespace Verse.Sound
{
	// Token: 0x0200092A RID: 2346
	public class SoundParamSource_Perlin : SoundParamSource
	{
		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x060039D9 RID: 14809 RVA: 0x0002CA1E File Offset: 0x0002AC1E
		public override string Label
		{
			get
			{
				return "Perlin noise";
			}
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x001682FC File Offset: 0x001664FC
		public override float ValueFor(Sample samp)
		{
			float num;
			if (this.syncType == PerlinMappingSyncType.Sync)
			{
				num = samp.ParentHashCode % 100f;
			}
			else
			{
				num = (float)(samp.GetHashCode() % 100);
			}
			if (this.timeType == TimeType.Ticks && Current.ProgramState == ProgramState.Playing)
			{
				float num2;
				if (this.syncType == PerlinMappingSyncType.Sync)
				{
					num2 = (float)Find.TickManager.TicksGame - samp.ParentStartTick;
				}
				else
				{
					num2 = (float)(Find.TickManager.TicksGame - samp.startTick);
				}
				num2 /= 60f;
				num += num2;
			}
			else
			{
				float num3;
				if (this.syncType == PerlinMappingSyncType.Sync)
				{
					num3 = Time.realtimeSinceStartup - samp.ParentStartRealTime;
				}
				else
				{
					num3 = Time.realtimeSinceStartup - samp.startRealTime;
				}
				num += num3;
			}
			num *= this.perlinFrequency;
			return ((float)SoundParamSource_Perlin.perlin.GetValue((double)num, 0.0, 0.0) * 2f + 1f) / 2f;
		}

		// Token: 0x04002811 RID: 10257
		[Description("The type of time on which this perlin randomizer will work. If you use Ticks, it will freeze when paused and speed up in fast forward.")]
		public TimeType timeType;

		// Token: 0x04002812 RID: 10258
		[Description("The frequency of the perlin output. The input time is multiplied by this amount.")]
		public float perlinFrequency = 1f;

		// Token: 0x04002813 RID: 10259
		[Description("Whether to synchronize the Perlin output across different samples. If set to desync, each playing sample will get a separate Perlin output.")]
		public PerlinMappingSyncType syncType;

		// Token: 0x04002814 RID: 10260
		private static Perlin perlin = new Perlin(0.009999999776482582, 2.0, 0.5, 4, Rand.Range(0, int.MaxValue), QualityMode.Medium);
	}
}
