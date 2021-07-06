using System;

namespace RimWorld
{
	// Token: 0x02001C7E RID: 7294
	[DefOf]
	public static class PawnGroupKindDefOf
	{
		// Token: 0x06009F81 RID: 40833 RVA: 0x0006A4FE File Offset: 0x000686FE
		static PawnGroupKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnGroupKindDefOf));
		}

		// Token: 0x04006BA7 RID: 27559
		public static PawnGroupKindDef Combat;

		// Token: 0x04006BA8 RID: 27560
		public static PawnGroupKindDef Trader;

		// Token: 0x04006BA9 RID: 27561
		public static PawnGroupKindDef Peaceful;

		// Token: 0x04006BAA RID: 27562
		public static PawnGroupKindDef Settlement;
	}
}
