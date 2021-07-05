using System;

namespace RimWorld
{
	// Token: 0x02001C48 RID: 7240
	[DefOf]
	public static class NeedDefOf
	{
		// Token: 0x06009F4B RID: 40779 RVA: 0x0006A168 File Offset: 0x00068368
		static NeedDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
		}

		// Token: 0x0400673A RID: 26426
		public static NeedDef Food;

		// Token: 0x0400673B RID: 26427
		public static NeedDef Rest;

		// Token: 0x0400673C RID: 26428
		public static NeedDef Joy;
	}
}
