using System;

namespace RimWorld
{
	// Token: 0x02001445 RID: 5189
	[DefOf]
	public static class FleshTypeDefOf
	{
		// Token: 0x06007D38 RID: 32056 RVA: 0x002C49D4 File Offset: 0x002C2BD4
		static FleshTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FleshTypeDefOf));
		}

		// Token: 0x04004CB7 RID: 19639
		public static FleshTypeDef Normal;

		// Token: 0x04004CB8 RID: 19640
		public static FleshTypeDef Mechanoid;

		// Token: 0x04004CB9 RID: 19641
		public static FleshTypeDef Insectoid;
	}
}
