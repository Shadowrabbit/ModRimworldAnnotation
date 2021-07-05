using System;

namespace RimWorld
{
	// Token: 0x02001CA1 RID: 7329
	[DefOf]
	public static class ExpectationDefOf
	{
		// Token: 0x06009FA4 RID: 40868 RVA: 0x0006A751 File Offset: 0x00068951
		static ExpectationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ExpectationDefOf));
		}

		// Token: 0x04006C5B RID: 27739
		public static ExpectationDef ExtremelyLow;

		// Token: 0x04006C5C RID: 27740
		public static ExpectationDef VeryLow;

		// Token: 0x04006C5D RID: 27741
		public static ExpectationDef Low;

		// Token: 0x04006C5E RID: 27742
		public static ExpectationDef Moderate;

		// Token: 0x04006C5F RID: 27743
		public static ExpectationDef High;

		// Token: 0x04006C60 RID: 27744
		public static ExpectationDef SkyHigh;
	}
}
