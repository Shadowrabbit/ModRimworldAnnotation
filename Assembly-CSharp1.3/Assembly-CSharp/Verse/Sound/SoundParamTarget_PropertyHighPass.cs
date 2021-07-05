using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200055F RID: 1375
	public class SoundParamTarget_PropertyHighPass : SoundParamTarget
	{
		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x000F7226 File Offset: 0x000F5426
		public override string Label
		{
			get
			{
				return "HighPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x000F723D File Offset: 0x000F543D
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterHighPass);
			}
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000F724C File Offset: 0x000F544C
		public override void SetOn(Sample sample, float value)
		{
			AudioHighPassFilter audioHighPassFilter = sample.source.GetComponent<AudioHighPassFilter>();
			if (audioHighPassFilter == null)
			{
				audioHighPassFilter = sample.source.gameObject.AddComponent<AudioHighPassFilter>();
			}
			if (this.filterProperty == HighPassFilterProperty.Cutoff)
			{
				audioHighPassFilter.cutoffFrequency = value;
			}
			if (this.filterProperty == HighPassFilterProperty.Resonance)
			{
				audioHighPassFilter.highpassResonanceQ = value;
			}
		}

		// Token: 0x04001924 RID: 6436
		private HighPassFilterProperty filterProperty;
	}
}
