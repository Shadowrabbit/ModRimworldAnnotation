using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200091B RID: 2331
	public class SoundFilterLowPass : SoundFilter
	{
		// Token: 0x060039B9 RID: 14777 RVA: 0x0002C82E File Offset: 0x0002AA2E
		public override void SetupOn(AudioSource source)
		{
			AudioLowPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioLowPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.lowpassResonanceQ = this.lowpassResonaceQ;
		}

		// Token: 0x040027F9 RID: 10233
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies above this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x040027FA RID: 10234
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float lowpassResonaceQ = 1f;
	}
}
