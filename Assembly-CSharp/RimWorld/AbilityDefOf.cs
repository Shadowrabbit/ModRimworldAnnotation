using System;

namespace RimWorld
{
	// Token: 0x02001CA7 RID: 7335
	[DefOf]
	public static class AbilityDefOf
	{
		// Token: 0x06009FAA RID: 40874 RVA: 0x0006A7B7 File Offset: 0x000689B7
		static AbilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AbilityDefOf));
		}

		// Token: 0x04006C76 RID: 27766
		[MayRequireRoyalty]
		public static AbilityDef Speech;
	}
}
