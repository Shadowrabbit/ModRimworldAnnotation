using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000549 RID: 1353
	public class SoundFilterHighPass : SoundFilter
	{
		// Token: 0x06002883 RID: 10371 RVA: 0x000F6DC3 File Offset: 0x000F4FC3
		public override void SetupOn(AudioSource source)
		{
			AudioHighPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioHighPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.highpassResonanceQ = this.highpassResonanceQ;
		}

		// Token: 0x04001905 RID: 6405
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies below this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x04001906 RID: 6406
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float highpassResonanceQ = 1f;
	}
}
