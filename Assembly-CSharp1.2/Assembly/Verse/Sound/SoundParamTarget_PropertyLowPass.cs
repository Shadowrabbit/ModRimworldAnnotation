using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000932 RID: 2354
	public class SoundParamTarget_PropertyLowPass : SoundParamTarget
	{
		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060039ED RID: 14829 RVA: 0x0002CAE0 File Offset: 0x0002ACE0
		public override string Label
		{
			get
			{
				return "LowPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060039EE RID: 14830 RVA: 0x0002CAF7 File Offset: 0x0002ACF7
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterLowPass);
			}
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x00168424 File Offset: 0x00166624
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

		// Token: 0x0400281D RID: 10269
		private LowPassFilterProperty filterProperty;
	}
}
