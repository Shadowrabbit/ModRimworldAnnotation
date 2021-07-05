using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200091E RID: 2334
	public class SoundFilterReverb : SoundFilter
	{
		// Token: 0x060039BF RID: 14783 RVA: 0x0016819C File Offset: 0x0016639C
		public override void SetupOn(AudioSource source)
		{
			AudioReverbFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioReverbFilter>(source);
			this.baseSetup.ApplyTo(orMakeFilterOn);
		}

		// Token: 0x04002801 RID: 10241
		[Description("The base setup for this filter.\n\nOnly used if no parameters ever affect this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();
	}
}
