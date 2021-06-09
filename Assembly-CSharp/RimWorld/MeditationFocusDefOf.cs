using System;

namespace RimWorld
{
	// Token: 0x02001CA8 RID: 7336
	[DefOf]
	public static class MeditationFocusDefOf
	{
		// Token: 0x06009FAB RID: 40875 RVA: 0x0006A7C8 File Offset: 0x000689C8
		static MeditationFocusDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MeditationFocusDefOf));
		}

		// Token: 0x04006C77 RID: 27767
		[MayRequireRoyalty]
		public static MeditationFocusDef Natural;
	}
}
