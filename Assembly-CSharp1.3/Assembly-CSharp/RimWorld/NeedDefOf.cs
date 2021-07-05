using System;

namespace RimWorld
{
	// Token: 0x02001408 RID: 5128
	[DefOf]
	public static class NeedDefOf
	{
		// Token: 0x06007CFB RID: 31995 RVA: 0x002C45C7 File Offset: 0x002C27C7
		static NeedDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
		}

		// Token: 0x04004758 RID: 18264
		public static NeedDef Food;

		// Token: 0x04004759 RID: 18265
		public static NeedDef Rest;

		// Token: 0x0400475A RID: 18266
		public static NeedDef Joy;

		// Token: 0x0400475B RID: 18267
		public static NeedDef Indoors;
	}
}
