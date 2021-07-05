using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200054A RID: 1354
	public class SoundFilterEcho : SoundFilter
	{
		// Token: 0x06002885 RID: 10373 RVA: 0x000F6E00 File Offset: 0x000F5000
		public override void SetupOn(AudioSource source)
		{
			AudioEchoFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioEchoFilter>(source);
			orMakeFilterOn.delay = this.delay;
			orMakeFilterOn.decayRatio = this.decayRatio;
			orMakeFilterOn.wetMix = this.wetMix;
			orMakeFilterOn.dryMix = this.dryMix;
		}

		// Token: 0x04001907 RID: 6407
		[EditSliderRange(10f, 5000f)]
		[Description("Echo delay in ms. 10 to 5000. Default = 500.")]
		private float delay = 500f;

		// Token: 0x04001908 RID: 6408
		[EditSliderRange(0f, 1f)]
		[Description("Echo decay per delay. 0 to 1. 1.0 = No decay, 0.0 = total decay (ie simple 1 line delay).")]
		private float decayRatio = 0.5f;

		// Token: 0x04001909 RID: 6409
		[EditSliderRange(0f, 1f)]
		[Description("The volume of the echo signal to pass to output.")]
		private float wetMix = 1f;

		// Token: 0x0400190A RID: 6410
		[EditSliderRange(0f, 1f)]
		[Description("The volume of the original signal to pass to output.")]
		private float dryMix = 1f;
	}
}
