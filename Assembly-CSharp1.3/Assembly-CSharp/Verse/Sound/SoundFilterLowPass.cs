using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000548 RID: 1352
	public class SoundFilterLowPass : SoundFilter
	{
		// Token: 0x06002881 RID: 10369 RVA: 0x000F6D86 File Offset: 0x000F4F86
		public override void SetupOn(AudioSource source)
		{
			AudioLowPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioLowPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.lowpassResonanceQ = this.lowpassResonaceQ;
		}

		// Token: 0x04001903 RID: 6403
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies above this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x04001904 RID: 6404
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float lowpassResonaceQ = 1f;
	}
}
