using System;

namespace RimWorld
{
	// Token: 0x02001C64 RID: 7268
	[DefOf]
	public static class StorytellerDefOf
	{
		// Token: 0x06009F67 RID: 40807 RVA: 0x0006A344 File Offset: 0x00068544
		static StorytellerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StorytellerDefOf));
		}

		// Token: 0x0400692B RID: 26923
		public static StorytellerDef Cassandra;

		// Token: 0x0400692C RID: 26924
		public static StorytellerDef Tutor;
	}
}
