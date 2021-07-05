using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200054B RID: 1355
	public class SoundFilterReverb : SoundFilter
	{
		// Token: 0x06002887 RID: 10375 RVA: 0x000F6E6C File Offset: 0x000F506C
		public override void SetupOn(AudioSource source)
		{
			AudioReverbFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioReverbFilter>(source);
			this.baseSetup.ApplyTo(orMakeFilterOn);
		}

		// Token: 0x0400190B RID: 6411
		[Description("The base setup for this filter.\n\nOnly used if no parameters ever affect this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();
	}
}
