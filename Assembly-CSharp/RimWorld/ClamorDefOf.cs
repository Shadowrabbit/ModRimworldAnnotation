using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C9C RID: 7324
	[DefOf]
	public static class ClamorDefOf
	{
		// Token: 0x06009F9F RID: 40863 RVA: 0x0006A6FC File Offset: 0x000688FC
		static ClamorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ClamorDefOf));
		}

		// Token: 0x04006C4A RID: 27722
		public static ClamorDef Movement;

		// Token: 0x04006C4B RID: 27723
		public static ClamorDef Harm;

		// Token: 0x04006C4C RID: 27724
		public static ClamorDef Construction;

		// Token: 0x04006C4D RID: 27725
		public static ClamorDef Impact;

		// Token: 0x04006C4E RID: 27726
		public static ClamorDef Ability;
	}
}
