using System;

namespace RimWorld
{
	// Token: 0x02001468 RID: 5224
	[DefOf]
	public static class MeditationFocusDefOf
	{
		// Token: 0x06007D5B RID: 32091 RVA: 0x002C4C27 File Offset: 0x002C2E27
		static MeditationFocusDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MeditationFocusDefOf));
		}

		// Token: 0x04004D6C RID: 19820
		[MayRequireRoyalty]
		public static MeditationFocusDef Natural;
	}
}
