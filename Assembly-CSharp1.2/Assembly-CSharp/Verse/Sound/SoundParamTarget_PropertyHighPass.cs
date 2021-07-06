using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000934 RID: 2356
	public class SoundParamTarget_PropertyHighPass : SoundParamTarget
	{
		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060039F1 RID: 14833 RVA: 0x0002CB03 File Offset: 0x0002AD03
		public override string Label
		{
			get
			{
				return "HighPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x060039F2 RID: 14834 RVA: 0x0002CB1A File Offset: 0x0002AD1A
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterHighPass);
			}
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x00168478 File Offset: 0x00166678
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

		// Token: 0x04002821 RID: 10273
		private HighPassFilterProperty filterProperty;
	}
}
