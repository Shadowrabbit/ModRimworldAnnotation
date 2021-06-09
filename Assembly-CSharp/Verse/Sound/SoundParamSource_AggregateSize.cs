using System;

namespace Verse.Sound
{
	// Token: 0x02000923 RID: 2339
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x0002C964 File Offset: 0x0002AB64
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x0002C96B File Offset: 0x0002AB6B
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
