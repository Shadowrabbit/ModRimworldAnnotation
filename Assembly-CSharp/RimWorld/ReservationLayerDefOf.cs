using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C89 RID: 7305
	[DefOf]
	public static class ReservationLayerDefOf
	{
		// Token: 0x06009F8C RID: 40844 RVA: 0x0006A5B9 File Offset: 0x000687B9
		static ReservationLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReservationLayerDefOf));
		}

		// Token: 0x04006BD5 RID: 27605
		public static ReservationLayerDef Floor;

		// Token: 0x04006BD6 RID: 27606
		public static ReservationLayerDef Ceiling;
	}
}
