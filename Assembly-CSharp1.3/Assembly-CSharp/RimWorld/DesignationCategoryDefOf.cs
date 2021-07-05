using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001405 RID: 5125
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		// Token: 0x06007CF9 RID: 31993 RVA: 0x002C45A5 File Offset: 0x002C27A5
		static DesignationCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationCategoryDefOf));
		}

		// Token: 0x04004716 RID: 18198
		public static DesignationCategoryDef Production;

		// Token: 0x04004717 RID: 18199
		public static DesignationCategoryDef Structure;

		// Token: 0x04004718 RID: 18200
		public static DesignationCategoryDef Security;

		// Token: 0x04004719 RID: 18201
		public static DesignationCategoryDef Floors;
	}
}
