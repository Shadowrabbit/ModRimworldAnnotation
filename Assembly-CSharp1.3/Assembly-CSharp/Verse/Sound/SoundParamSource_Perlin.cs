using System;
using UnityEngine;
using Verse.Noise;

namespace Verse.Sound
{
	// Token: 0x02000555 RID: 1365
	public class SoundParamSource_Perlin : SoundParamSource
	{
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x000F6FC2 File Offset: 0x000F51C2
		public override string Label
		{
			get
			{
				return "Perlin noise";
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000F6FCC File Offset: 0x000F51CC
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

		// Token: 0x04001914 RID: 6420
		[Description("The type of time on which this perlin randomizer will work. If you use Ticks, it will freeze when paused and speed up in fast forward.")]
		public TimeType timeType;

		// Token: 0x04001915 RID: 6421
		[Description("The frequency of the perlin output. The input time is multiplied by this amount.")]
		public float perlinFrequency = 1f;

		// Token: 0x04001916 RID: 6422
		[Description("Whether to synchronize the Perlin output across different samples. If set to desync, each playing sample will get a separate Perlin output.")]
		public PerlinMappingSyncType syncType;

		// Token: 0x04001917 RID: 6423
		private static Perlin perlin = new Perlin(0.009999999776482582, 2.0, 0.5, 4, Rand.Range(0, int.MaxValue), QualityMode.Medium);
	}
}
