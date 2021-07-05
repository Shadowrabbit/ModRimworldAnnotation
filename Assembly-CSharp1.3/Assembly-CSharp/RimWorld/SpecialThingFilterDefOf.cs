using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200142C RID: 5164
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		// Token: 0x06007D1F RID: 32031 RVA: 0x002C482B File Offset: 0x002C2A2B
		static SpecialThingFilterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SpecialThingFilterDefOf));
		}

		// Token: 0x04004A19 RID: 18969
		public static SpecialThingFilterDef AllowFresh;

		// Token: 0x04004A1A RID: 18970
		public static SpecialThingFilterDef AllowDeadmansApparel;

		// Token: 0x04004A1B RID: 18971
		public static SpecialThingFilterDef AllowNonDeadmansApparel;
	}
}
