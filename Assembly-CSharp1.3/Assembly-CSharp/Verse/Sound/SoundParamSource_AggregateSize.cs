using System;

namespace Verse.Sound
{
	// Token: 0x0200054E RID: 1358
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x0600288F RID: 10383 RVA: 0x000F6F08 File Offset: 0x000F5108
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000F6F0F File Offset: 0x000F510F
		public override float ValueFor(Sample samp)
		{
			if (samp.ExternalParams.sizeAggregator == null)
			{
				return 0f;
			}
			return samp.ExternalParams.sizeAggregator.AggregateSize;
		}
	}
}
