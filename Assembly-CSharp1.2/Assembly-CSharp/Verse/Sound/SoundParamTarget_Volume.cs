using System;

namespace Verse.Sound
{
	// Token: 0x0200092E RID: 2350
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x060039E7 RID: 14823 RVA: 0x0002CAC0 File Offset: 0x0002ACC0
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x0002CAC7 File Offset: 0x0002ACC7
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
