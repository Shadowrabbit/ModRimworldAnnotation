using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200144A RID: 5194
	[DefOf]
	public static class ReservationLayerDefOf
	{
		// Token: 0x06007D3D RID: 32061 RVA: 0x002C4A29 File Offset: 0x002C2C29
		static ReservationLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReservationLayerDefOf));
		}

		// Token: 0x04004CCD RID: 19661
		public static ReservationLayerDef Floor;

		// Token: 0x04004CCE RID: 19662
		public static ReservationLayerDef Ceiling;
	}
}
