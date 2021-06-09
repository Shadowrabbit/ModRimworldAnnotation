using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C45 RID: 7237
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		// Token: 0x06009F49 RID: 40777 RVA: 0x0006A146 File Offset: 0x00068346
		static DesignationCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationCategoryDefOf));
		}

		// Token: 0x04006710 RID: 26384
		public static DesignationCategoryDef Production;

		// Token: 0x04006711 RID: 26385
		public static DesignationCategoryDef Structure;

		// Token: 0x04006712 RID: 26386
		public static DesignationCategoryDef Security;

		// Token: 0x04006713 RID: 26387
		public static DesignationCategoryDef Floors;
	}
}
