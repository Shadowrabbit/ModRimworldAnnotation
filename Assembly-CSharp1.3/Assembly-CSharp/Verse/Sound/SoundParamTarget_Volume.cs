using System;

namespace Verse.Sound
{
	// Token: 0x02000559 RID: 1369
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x000F7146 File Offset: 0x000F5346
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000F714D File Offset: 0x000F534D
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
