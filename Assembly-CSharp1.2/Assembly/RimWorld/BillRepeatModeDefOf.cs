using System;

namespace RimWorld
{
	// Token: 0x02001C87 RID: 7303
	[DefOf]
	public static class BillRepeatModeDefOf
	{
		// Token: 0x06009F8A RID: 40842 RVA: 0x0006A597 File Offset: 0x00068797
		static BillRepeatModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillRepeatModeDefOf));
		}

		// Token: 0x04006BCF RID: 27599
		public static BillRepeatModeDef RepeatCount;

		// Token: 0x04006BD0 RID: 27600
		public static BillRepeatModeDef TargetCount;

		// Token: 0x04006BD1 RID: 27601
		public static BillRepeatModeDef Forever;
	}
}
