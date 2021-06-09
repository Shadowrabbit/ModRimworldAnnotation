using System;

namespace RimWorld
{
	// Token: 0x02001740 RID: 5952
	public class ThingSetMaker_Conditional_MinMaxTotalMarketValue : ThingSetMaker_Conditional
	{
		// Token: 0x06008346 RID: 33606 RVA: 0x0005825D File Offset: 0x0005645D
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return parms.totalMarketValueRange != null && parms.totalMarketValueRange.Value.max >= this.minMaxTotalMarketValue;
		}

		// Token: 0x04005513 RID: 21779
		public float minMaxTotalMarketValue;
	}
}
