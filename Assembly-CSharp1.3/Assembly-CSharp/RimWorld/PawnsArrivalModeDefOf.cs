using System;

namespace RimWorld
{
	// Token: 0x02001459 RID: 5209
	[DefOf]
	public static class PawnsArrivalModeDefOf
	{
		// Token: 0x06007D4C RID: 32076 RVA: 0x002C4B28 File Offset: 0x002C2D28
		static PawnsArrivalModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnsArrivalModeDefOf));
		}

		// Token: 0x04004D28 RID: 19752
		public static PawnsArrivalModeDef EdgeWalkIn;

		// Token: 0x04004D29 RID: 19753
		public static PawnsArrivalModeDef CenterDrop;

		// Token: 0x04004D2A RID: 19754
		public static PawnsArrivalModeDef EdgeDrop;

		// Token: 0x04004D2B RID: 19755
		public static PawnsArrivalModeDef RandomDrop;
	}
}
