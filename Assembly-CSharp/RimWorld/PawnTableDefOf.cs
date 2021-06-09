using System;

namespace RimWorld
{
	// Token: 0x02001C8C RID: 7308
	[DefOf]
	public static class PawnTableDefOf
	{
		// Token: 0x06009F8F RID: 40847 RVA: 0x0006A5EC File Offset: 0x000687EC
		static PawnTableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableDefOf));
		}

		// Token: 0x04006BDC RID: 27612
		public static PawnTableDef Work;

		// Token: 0x04006BDD RID: 27613
		public static PawnTableDef Assign;

		// Token: 0x04006BDE RID: 27614
		public static PawnTableDef Restrict;

		// Token: 0x04006BDF RID: 27615
		public static PawnTableDef Animals;

		// Token: 0x04006BE0 RID: 27616
		public static PawnTableDef Wildlife;
	}
}
