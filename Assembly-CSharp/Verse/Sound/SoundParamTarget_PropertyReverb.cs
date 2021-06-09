using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000937 RID: 2359
	public class SoundParamTarget_PropertyReverb : SoundParamTarget
	{
		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060039F9 RID: 14841 RVA: 0x0002CB49 File Offset: 0x0002AD49
		public override string Label
		{
			get
			{
				return "ReverbFilter-interpolant";
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060039FA RID: 14842 RVA: 0x0002CB50 File Offset: 0x0002AD50
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterReverb);
			}
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x00168540 File Offset: 0x00166740
		public override void SetOn(Sample sample, float value)
		{
			AudioReverbFilter audioReverbFilter = sample.source.GetComponent<AudioReverbFilter>();
			if (audioReverbFilter == null)
			{
				audioReverbFilter = sample.source.gameObject.AddComponent<AudioReverbFilter>();
			}
			ReverbSetup reverbSetup;
			if (value < 0.001f)
			{
				reverbSetup = this.baseSetup;
			}
			if (value > 0.999f)
			{
				reverbSetup = this.targetSetup;
			}
			else
			{
				reverbSetup = ReverbSetup.Lerp(this.baseSetup, this.targetSetup, value);
			}
			reverbSetup.ApplyTo(audioReverbFilter);
		}

		// Token: 0x04002828 RID: 10280
		[Description("The base setup for the reverb.\n\nOnly used if no parameters are touching this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();

		// Token: 0x04002829 RID: 10281
		[Description("The interpolation target setup for this filter.\n\nWhen the interpolant parameter is at 1, these settings will be active.")]
		private ReverbSetup targetSetup = new ReverbSetup();
	}
}
