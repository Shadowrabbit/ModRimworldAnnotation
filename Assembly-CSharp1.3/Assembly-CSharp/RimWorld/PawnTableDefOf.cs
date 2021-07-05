using System;

namespace RimWorld
{
	// Token: 0x0200144D RID: 5197
	[DefOf]
	public static class PawnTableDefOf
	{
		// Token: 0x06007D40 RID: 32064 RVA: 0x002C4A5C File Offset: 0x002C2C5C
		static PawnTableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableDefOf));
		}

		// Token: 0x04004CD3 RID: 19667
		public static PawnTableDef Work;

		// Token: 0x04004CD4 RID: 19668
		public static PawnTableDef Assign;

		// Token: 0x04004CD5 RID: 19669
		public static PawnTableDef Restrict;

		// Token: 0x04004CD6 RID: 19670
		public static PawnTableDef Animals;

		// Token: 0x04004CD7 RID: 19671
		public static PawnTableDef Wildlife;
	}
}
