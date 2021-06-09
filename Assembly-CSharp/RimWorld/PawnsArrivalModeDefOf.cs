using System;

namespace RimWorld
{
	// Token: 0x02001C99 RID: 7321
	[DefOf]
	public static class PawnsArrivalModeDefOf
	{
		// Token: 0x06009F9C RID: 40860 RVA: 0x0006A6C9 File Offset: 0x000688C9
		static PawnsArrivalModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnsArrivalModeDefOf));
		}

		// Token: 0x04006C34 RID: 27700
		public static PawnsArrivalModeDef EdgeWalkIn;

		// Token: 0x04006C35 RID: 27701
		public static PawnsArrivalModeDef CenterDrop;

		// Token: 0x04006C36 RID: 27702
		public static PawnsArrivalModeDef EdgeDrop;

		// Token: 0x04006C37 RID: 27703
		public static PawnsArrivalModeDef RandomDrop;

		// Token: 0x04006C38 RID: 27704
		public static PawnsArrivalModeDef Shuttle;
	}
}
