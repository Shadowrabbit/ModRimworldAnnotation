using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5E RID: 7262
	[DefOf]
	public static class RoofDefOf
	{
		// Token: 0x06009F61 RID: 40801 RVA: 0x0006A2DE File Offset: 0x000684DE
		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoofDefOf));
		}

		// Token: 0x04006902 RID: 26882
		public static RoofDef RoofConstructed;

		// Token: 0x04006903 RID: 26883
		public static RoofDef RoofRockThick;

		// Token: 0x04006904 RID: 26884
		public static RoofDef RoofRockThin;
	}
}
