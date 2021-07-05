using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200142F RID: 5167
	[DefOf]
	public static class SongDefOf
	{
		// Token: 0x06007D22 RID: 32034 RVA: 0x002C485E File Offset: 0x002C2A5E
		static SongDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SongDefOf));
		}

		// Token: 0x04004B4D RID: 19277
		public static SongDef EntrySong;

		// Token: 0x04004B4E RID: 19278
		public static SongDef EndCreditsSong;

		// Token: 0x04004B4F RID: 19279
		[MayRequireIdeology]
		public static SongDef ArchonexusVictorySong;
	}
}
