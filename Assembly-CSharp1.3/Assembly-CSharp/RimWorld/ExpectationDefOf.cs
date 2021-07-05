using System;

namespace RimWorld
{
	// Token: 0x02001461 RID: 5217
	[DefOf]
	public static class ExpectationDefOf
	{
		// Token: 0x06007D54 RID: 32084 RVA: 0x002C4BB0 File Offset: 0x002C2DB0
		static ExpectationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ExpectationDefOf));
		}

		// Token: 0x04004D4E RID: 19790
		public static ExpectationDef ExtremelyLow;

		// Token: 0x04004D4F RID: 19791
		public static ExpectationDef VeryLow;

		// Token: 0x04004D50 RID: 19792
		public static ExpectationDef Low;

		// Token: 0x04004D51 RID: 19793
		public static ExpectationDef Moderate;

		// Token: 0x04004D52 RID: 19794
		public static ExpectationDef High;

		// Token: 0x04004D53 RID: 19795
		public static ExpectationDef SkyHigh;
	}
}
