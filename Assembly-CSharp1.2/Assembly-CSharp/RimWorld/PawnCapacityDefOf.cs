using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C59 RID: 7257
	[DefOf]
	public static class PawnCapacityDefOf
	{
		// Token: 0x06009F5C RID: 40796 RVA: 0x0006A289 File Offset: 0x00068489
		static PawnCapacityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnCapacityDefOf));
		}

		// Token: 0x040068C3 RID: 26819
		public static PawnCapacityDef Consciousness;

		// Token: 0x040068C4 RID: 26820
		public static PawnCapacityDef Sight;

		// Token: 0x040068C5 RID: 26821
		public static PawnCapacityDef Hearing;

		// Token: 0x040068C6 RID: 26822
		public static PawnCapacityDef Moving;

		// Token: 0x040068C7 RID: 26823
		public static PawnCapacityDef Manipulation;

		// Token: 0x040068C8 RID: 26824
		public static PawnCapacityDef Talking;

		// Token: 0x040068C9 RID: 26825
		public static PawnCapacityDef Eating;

		// Token: 0x040068CA RID: 26826
		public static PawnCapacityDef Breathing;

		// Token: 0x040068CB RID: 26827
		public static PawnCapacityDef BloodFiltration;

		// Token: 0x040068CC RID: 26828
		public static PawnCapacityDef BloodPumping;

		// Token: 0x040068CD RID: 26829
		public static PawnCapacityDef Metabolism;
	}
}
