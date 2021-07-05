using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200055D RID: 1373
	public class SoundParamTarget_PropertyLowPass : SoundParamTarget
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060028B2 RID: 10418 RVA: 0x000F71B0 File Offset: 0x000F53B0
		public override string Label
		{
			get
			{
				return "LowPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060028B3 RID: 10419 RVA: 0x000F71C7 File Offset: 0x000F53C7
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterLowPass);
			}
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000F71D4 File Offset: 0x000F53D4
		public override void SetOn(Sample sample, float value)
		{
			AudioLowPassFilter audioLowPassFilter = sample.source.GetComponent<AudioLowPassFilter>();
			if (audioLowPassFilter == null)
			{
				audioLowPassFilter = sample.source.gameObject.AddComponent<AudioLowPassFilter>();
			}
			if (this.filterProperty == LowPassFilterProperty.Cutoff)
			{
				audioLowPassFilter.cutoffFrequency = value;
			}
			if (this.filterProperty == LowPassFilterProperty.Resonance)
			{
				audioLowPassFilter.lowpassResonanceQ = value;
			}
		}

		// Token: 0x04001920 RID: 6432
		private LowPassFilterProperty filterProperty;
	}
}
