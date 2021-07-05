using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200091C RID: 2332
	public class SoundFilterHighPass : SoundFilter
	{
		// Token: 0x060039BB RID: 14779 RVA: 0x0002C86B File Offset: 0x0002AA6B
		public override void SetupOn(AudioSource source)
		{
			AudioHighPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioHighPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.highpassResonanceQ = this.highpassResonanceQ;
		}

		// Token: 0x040027FB RID: 10235
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies below this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x040027FC RID: 10236
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float highpassResonanceQ = 1f;
	}
}
