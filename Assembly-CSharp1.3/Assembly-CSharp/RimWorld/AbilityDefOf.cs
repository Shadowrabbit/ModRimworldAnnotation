using System;

namespace RimWorld
{
	// Token: 0x02001467 RID: 5223
	[DefOf]
	public static class AbilityDefOf
	{
		// Token: 0x06007D5A RID: 32090 RVA: 0x002C4C16 File Offset: 0x002C2E16
		static AbilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AbilityDefOf));
		}

		// Token: 0x04004D6A RID: 19818
		[MayRequireRoyalty]
		public static AbilityDef Speech;

		// Token: 0x04004D6B RID: 19819
		[MayRequireRoyalty]
		public static AbilityDef AnimaTreeLinking;
	}
}
