using System;

namespace RimWorld
{
	// Token: 0x02001466 RID: 5222
	[DefOf]
	public static class RoyalTitleDefOf
	{
		// Token: 0x06007D59 RID: 32089 RVA: 0x002C4C05 File Offset: 0x002C2E05
		static RoyalTitleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoyalTitleDefOf));
		}

		// Token: 0x04004D68 RID: 19816
		[MayRequireRoyalty]
		public static RoyalTitleDef Knight;

		// Token: 0x04004D69 RID: 19817
		[MayRequireRoyalty]
		public static RoyalTitleDef Count;
	}
}
