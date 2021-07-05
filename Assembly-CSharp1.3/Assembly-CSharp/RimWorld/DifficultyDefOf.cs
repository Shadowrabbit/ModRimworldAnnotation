using System;

namespace RimWorld
{
	// Token: 0x02001423 RID: 5155
	[DefOf]
	public static class DifficultyDefOf
	{
		// Token: 0x06007D16 RID: 32022 RVA: 0x002C4792 File Offset: 0x002C2992
		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}

		// Token: 0x040049A6 RID: 18854
		public static DifficultyDef Peaceful;

		// Token: 0x040049A7 RID: 18855
		public static DifficultyDef Easy;

		// Token: 0x040049A8 RID: 18856
		public static DifficultyDef Rough;
	}
}
