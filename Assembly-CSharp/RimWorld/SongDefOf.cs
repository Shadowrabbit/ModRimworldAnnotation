using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C6F RID: 7279
	[DefOf]
	public static class SongDefOf
	{
		// Token: 0x06009F72 RID: 40818 RVA: 0x0006A3FF File Offset: 0x000685FF
		static SongDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SongDefOf));
		}

		// Token: 0x04006A85 RID: 27269
		public static SongDef EntrySong;

		// Token: 0x04006A86 RID: 27270
		public static SongDef EndCreditsSong;
	}
}
