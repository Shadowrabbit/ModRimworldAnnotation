using System;

namespace RimWorld
{
	// Token: 0x02001C63 RID: 7267
	[DefOf]
	public static class DifficultyDefOf
	{
		// Token: 0x06009F66 RID: 40806 RVA: 0x0006A333 File Offset: 0x00068533
		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}

		// Token: 0x04006928 RID: 26920
		public static DifficultyDef Peaceful;

		// Token: 0x04006929 RID: 26921
		public static DifficultyDef Easy;

		// Token: 0x0400692A RID: 26922
		public static DifficultyDef Rough;
	}
}
