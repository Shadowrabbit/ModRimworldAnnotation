using System;

namespace RimWorld
{
	// Token: 0x02001CA6 RID: 7334
	[DefOf]
	public static class RoyalTitleDefOf
	{
		// Token: 0x06009FA9 RID: 40873 RVA: 0x0006A7A6 File Offset: 0x000689A6
		static RoyalTitleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoyalTitleDefOf));
		}

		// Token: 0x04006C74 RID: 27764
		[MayRequireRoyalty]
		public static RoyalTitleDef Knight;

		// Token: 0x04006C75 RID: 27765
		[MayRequireRoyalty]
		public static RoyalTitleDef Count;
	}
}
