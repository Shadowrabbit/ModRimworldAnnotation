using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C6C RID: 7276
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		// Token: 0x06009F6F RID: 40815 RVA: 0x0006A3CC File Offset: 0x000685CC
		static SpecialThingFilterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SpecialThingFilterDefOf));
		}

		// Token: 0x04006990 RID: 27024
		public static SpecialThingFilterDef AllowFresh;

		// Token: 0x04006991 RID: 27025
		public static SpecialThingFilterDef AllowDeadmansApparel;

		// Token: 0x04006992 RID: 27026
		public static SpecialThingFilterDef AllowNonDeadmansApparel;
	}
}
