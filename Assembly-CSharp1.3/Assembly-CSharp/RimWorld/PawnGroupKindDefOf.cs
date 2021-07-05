using System;

namespace RimWorld
{
	// Token: 0x0200143E RID: 5182
	[DefOf]
	public static class PawnGroupKindDefOf
	{
		// Token: 0x06007D31 RID: 32049 RVA: 0x002C495D File Offset: 0x002C2B5D
		static PawnGroupKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnGroupKindDefOf));
		}

		// Token: 0x04004C89 RID: 19593
		public static PawnGroupKindDef Combat;

		// Token: 0x04004C8A RID: 19594
		public static PawnGroupKindDef Trader;

		// Token: 0x04004C8B RID: 19595
		public static PawnGroupKindDef Peaceful;

		// Token: 0x04004C8C RID: 19596
		public static PawnGroupKindDef Settlement;

		// Token: 0x04004C8D RID: 19597
		public static PawnGroupKindDef Settlement_RangedOnly;

		// Token: 0x04004C8E RID: 19598
		[MayRequireIdeology]
		public static PawnGroupKindDef Miners;

		// Token: 0x04004C8F RID: 19599
		[MayRequireIdeology]
		public static PawnGroupKindDef Farmers;

		// Token: 0x04004C90 RID: 19600
		[MayRequireIdeology]
		public static PawnGroupKindDef Loggers;

		// Token: 0x04004C91 RID: 19601
		[MayRequireIdeology]
		public static PawnGroupKindDef Hunters;
	}
}
