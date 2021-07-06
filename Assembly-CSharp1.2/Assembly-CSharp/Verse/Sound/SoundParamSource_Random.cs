using System;

namespace Verse.Sound
{
	// Token: 0x0200092B RID: 2347
	public class SoundParamSource_Random : SoundParamSource
	{
		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x0002CA6C File Offset: 0x0002AC6C
		public override string Label
		{
			get
			{
				return "Random";
			}
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x0002CA73 File Offset: 0x0002AC73
		public override float ValueFor(Sample samp)
		{
			return Rand.Value;
		}
	}
}
