using System;

namespace Verse.Sound
{
	// Token: 0x02000556 RID: 1366
	public class SoundParamSource_Random : SoundParamSource
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x000F70F2 File Offset: 0x000F52F2
		public override string Label
		{
			get
			{
				return "Random";
			}
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000F70F9 File Offset: 0x000F52F9
		public override float ValueFor(Sample samp)
		{
			return Rand.Value;
		}
	}
}
