﻿using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000562 RID: 1378
	public class SoundParamTarget_PropertyReverb : SoundParamTarget
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060028BE RID: 10430 RVA: 0x000F7336 File Offset: 0x000F5536
		public override string Label
		{
			get
			{
				return "ReverbFilter-interpolant";
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x000F733D File Offset: 0x000F553D
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterReverb);
			}
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x000F734C File Offset: 0x000F554C
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

		// Token: 0x0400192B RID: 6443
		[Description("The base setup for the reverb.\n\nOnly used if no parameters are touching this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();

		// Token: 0x0400192C RID: 6444
		[Description("The interpolation target setup for this filter.\n\nWhen the interpolant parameter is at 1, these settings will be active.")]
		private ReverbSetup targetSetup = new ReverbSetup();
	}
}
