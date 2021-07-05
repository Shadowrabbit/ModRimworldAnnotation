using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001419 RID: 5145
	[DefOf]
	public static class PawnCapacityDefOf
	{
		// Token: 0x06007D0C RID: 32012 RVA: 0x002C46E8 File Offset: 0x002C28E8
		static PawnCapacityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnCapacityDefOf));
		}

		// Token: 0x04004937 RID: 18743
		public static PawnCapacityDef Consciousness;

		// Token: 0x04004938 RID: 18744
		public static PawnCapacityDef Sight;

		// Token: 0x04004939 RID: 18745
		public static PawnCapacityDef Hearing;

		// Token: 0x0400493A RID: 18746
		public static PawnCapacityDef Moving;

		// Token: 0x0400493B RID: 18747
		public static PawnCapacityDef Manipulation;

		// Token: 0x0400493C RID: 18748
		public static PawnCapacityDef Talking;

		// Token: 0x0400493D RID: 18749
		public static PawnCapacityDef Eating;

		// Token: 0x0400493E RID: 18750
		public static PawnCapacityDef Breathing;

		// Token: 0x0400493F RID: 18751
		public static PawnCapacityDef BloodFiltration;

		// Token: 0x04004940 RID: 18752
		public static PawnCapacityDef BloodPumping;

		// Token: 0x04004941 RID: 18753
		public static PawnCapacityDef Metabolism;
	}
}
