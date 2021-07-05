using System;

namespace RimWorld
{
	// Token: 0x02001448 RID: 5192
	[DefOf]
	public static class BillRepeatModeDefOf
	{
		// Token: 0x06007D3B RID: 32059 RVA: 0x002C4A07 File Offset: 0x002C2C07
		static BillRepeatModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillRepeatModeDefOf));
		}

		// Token: 0x04004CC7 RID: 19655
		public static BillRepeatModeDef RepeatCount;

		// Token: 0x04004CC8 RID: 19656
		public static BillRepeatModeDef TargetCount;

		// Token: 0x04004CC9 RID: 19657
		public static BillRepeatModeDef Forever;
	}
}
