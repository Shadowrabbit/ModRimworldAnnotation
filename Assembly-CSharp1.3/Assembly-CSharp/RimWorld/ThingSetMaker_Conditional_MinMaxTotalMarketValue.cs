using System;

namespace RimWorld
{
	// Token: 0x020010DA RID: 4314
	public class ThingSetMaker_Conditional_MinMaxTotalMarketValue : ThingSetMaker_Conditional
	{
		// Token: 0x0600673A RID: 26426 RVA: 0x0022E0CD File Offset: 0x0022C2CD
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return parms.totalMarketValueRange != null && parms.totalMarketValueRange.Value.max >= this.minMaxTotalMarketValue;
		}

		// Token: 0x04003A48 RID: 14920
		public float minMaxTotalMarketValue;
	}
}
